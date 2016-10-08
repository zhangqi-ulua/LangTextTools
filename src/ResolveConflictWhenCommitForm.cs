using SharpSvn;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace LangTextTools
{
    public partial class ResolveConflictWhenCommitForm : Form
    {
        // “编号”列的列名后缀
        private const string _NUM_COLUMN_NAME = "ColumnNum";
        // “版本变动”列的列名后缀
        private const string _IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME = "ColumnIsChangedBySvnRevision";
        // “处理方式”列的列名后缀
        private const string _RESOLVE_CONFLICT_WAY_COLUMN_NAME = "ColumnResolveConflictWay";
        // “SVN表主语言译文”列的列名后缀
        private const string _SVN_DEFAULT_LANGUAGE_COLUMN_NAME = "ColumnSvnDefaultLanguage";
        // “本地表行号”列的列名后缀
        private const string _LOCAL_LINE_NUM_COLUMN_NAME = "ColumnLocalLineNum";
        // “SVN表行号”列的列名后缀
        private const string _SVN_LINE_NUM_COLUMN_NAME = "ColumnSvnLineNum";

        // 各对比类型功能区的名称
        const string _PART_NAME_DIFF_INFO = "DiffInfo";
        const string _PART_NAME_LOCAL_ADD_KEY = "LocalAddKey";
        const string _PART_NAME_SVN_ADD_KEY = "SvnAddKey";

        // 需要提交到SVN上的差异信息
        private CommitCompareResult _compareResult = null;
        // 解析出的本地表信息
        private CommitExcelInfo _localExcelInfo = null;
        // 解析出的和本地表同版本号的SVN表信息
        private CommitExcelInfo _svnSameRevisionExcelInfo = null;
        // 刚进入此界面时与本地表进行比较的当时SVN最新版本表信息
        private CommitExcelInfo _lastSvnExcelInfo = null;
        // 最终SVN最新表版本表信息
        private CommitExcelInfo _newestSvnExcelInfo = null;

        // 记录各对比类型功能区所包含的控件
        private Dictionary<string, _PartControls> _partControls = null;

        public ResolveConflictWhenCommitForm(CommitCompareResult compareResult, CommitExcelInfo localExcelInfo, CommitExcelInfo svnExcelInfo)
        {
            InitializeComponent();

            // 初始化用于选择提交时是否已经导出提交主语言对应lang文件的ComboBox选项
            cmbCommitLangFile.Items.AddRange(AppValues.COMMIT_LANG_FILE_OPTINOS);
            cmbCommitLangFile.SelectedItem = AppValues.COMMIT_LANG_FILE_OPTINOS[0];

            // 记录各对比类型功能区所包含的控件
            _partControls = new Dictionary<string, _PartControls>();
            // 主语言译文不同功能区
            _PartControls diffInfoPartControls = new _PartControls();
            diffInfoPartControls.PartName = _PART_NAME_DIFF_INFO;
            diffInfoPartControls.DataGridView = dgvDiffDefaultLanguageInfo;
            diffInfoPartControls.ComboBox = cmbDiffInfoUnifiedResolveConflictWay;
            diffInfoPartControls.CheckBox = chkDiffInfoIgnoreSvnRevisionChange;
            _partControls.Add(_PART_NAME_DIFF_INFO, diffInfoPartControls);
            // 本地表新增Key功能区
            _PartControls localAddKeyPartControls = new _PartControls();
            localAddKeyPartControls.PartName = _PART_NAME_LOCAL_ADD_KEY;
            localAddKeyPartControls.DataGridView = dgvLocalAddKeyInfo;
            localAddKeyPartControls.ComboBox = cmbLocalAddKeyInfoUnifiedResolveConflictWay;
            localAddKeyPartControls.CheckBox = chkLocalAddKeyInfoIgnoreSvnRevisionChange;
            _partControls.Add(_PART_NAME_LOCAL_ADD_KEY, localAddKeyPartControls);
            // SVN表新增Key功能区
            _PartControls svnAddKeyPartControls = new _PartControls();
            svnAddKeyPartControls.PartName = _PART_NAME_SVN_ADD_KEY;
            svnAddKeyPartControls.DataGridView = dgvSvnAddKeyInfo;
            svnAddKeyPartControls.ComboBox = cmbSvnAddKeyInfoUnifiedResolveConflictWay;
            svnAddKeyPartControls.CheckBox = chkSvnAddKeyInfoIgnoreSvnRevisionChange;
            _partControls.Add(_PART_NAME_SVN_ADD_KEY, svnAddKeyPartControls);

            foreach (_PartControls onePartControls in _partControls.Values)
            {
                // 统一设置DataGridView中双击单元格的响应事件
                onePartControls.DataGridView.CellDoubleClick += _OnCellDoubleClick;

                // 设置用于统一处理差异处理方式的ComboBox选项
                // 设置选项内容
                onePartControls.ComboBox.Items.AddRange(AppValues.RESOLVE_COMMIT_DIFF_OPTINOS);
                // 绑定点击响应事件
                onePartControls.ComboBox.SelectedIndexChanged += _OnChangedUnifiedResolveConflictWay;

                // 设置用于让用户选择每条差异处理方式的DataGridViewComboBoxColumn选项
                DataGridViewComboBoxColumn comboBoxColumn = onePartControls.DataGridView.Columns[onePartControls.PartName + _RESOLVE_CONFLICT_WAY_COLUMN_NAME] as DataGridViewComboBoxColumn;
                comboBoxColumn.Items.AddRange(AppValues.RESOLVE_COMMIT_DIFF_OPTINOS);
            }

            _localExcelInfo = localExcelInfo;
            _lastSvnExcelInfo = svnExcelInfo;
            _newestSvnExcelInfo = svnExcelInfo;
            _compareResult = compareResult;
            _InitDataGridView(compareResult);
        }

        private void ResolveConflictWhenCommitForm_Load(object sender, EventArgs e)
        {
            if (AppValues.Config.ContainsKey(AppValues.CONFIG_KEY_COMMIT_LOG_MESSAGE))
                txtCommitLogMessage.Text = AppValues.Config[AppValues.CONFIG_KEY_COMMIT_LOG_MESSAGE];
            // Key与Value的分隔符为一个字符，优先使用主界面中lang文件导出功能设置的分隔符，其次使用配置文件的设置
            if (AppValues.KeyAndValueSplitChar != null)
                txtKeyValueSplitChar.Text = AppValues.KeyAndValueSplitChar;
            else if (AppValues.Config.ContainsKey(AppValues.CONFIG_KEY_KEY_VALUE_SPLIT_CHAR))
                txtKeyValueSplitChar.Text = AppValues.Config[AppValues.CONFIG_KEY_KEY_VALUE_SPLIT_CHAR][0].ToString();
        }

        // 点击“提交”按钮
        private void btnCommit_Click(object sender, EventArgs e)
        {
            // 检查本地表是否发生变动
            string newExcelMD5 = Utils.GetFileMD5(AppValues.LocalExcelFilePath);
            if (newExcelMD5 != null && !newExcelMD5.Equals(AppValues.ExcelMD5))
            {
                MessageBox.Show("检测到目前选择的本地表文件与点击“检查”按钮时内容已发生变动，请重新点击“检查”按钮读取最新的本地表内容", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                this.Close();
                return;
            }

            // 判断是否填写了提交所需的LogMessage信息
            string logMessage = txtCommitLogMessage.Text.Trim();
            if (string.IsNullOrEmpty(logMessage))
            {
                MessageBox.Show("执行SVN提交操作时必须填写说明信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 如果用户选择了一并导出、提交主语言对应的lang文件，需要检查配置文件中是否已设置路径，若没有需要用户手工指定
            string exportLangFilePath = null;
            string keyAndValueSplitChar = null;
            if (!AppValues.COMMIT_LANG_FILE_OPTINOS[0].Equals(cmbCommitLangFile.SelectedItem.ToString()))
            {
                // 检查是否输入了Key与Value的分隔字符
                keyAndValueSplitChar = txtKeyValueSplitChar.Text;
                if (string.IsNullOrEmpty(keyAndValueSplitChar))
                {
                    MessageBox.Show("必须输入导出lang文件中Key、Value的分隔字符", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            if (AppValues.COMMIT_LANG_FILE_OPTINOS[1].Equals(cmbCommitLangFile.SelectedItem.ToString()))
            {
                bool isPathCorrect = false;
                if (AppValues.Config.ContainsKey(AppValues.CONFIG_KEY_COMMIT_LANG_FILE_PATH))
                {
                    exportLangFilePath = AppValues.Config[AppValues.CONFIG_KEY_COMMIT_LANG_FILE_PATH];
                    try
                    {
                        exportLangFilePath = Path.GetFullPath(exportLangFilePath);
                        FileInfo fileInfo = new FileInfo(exportLangFilePath);
                        if (fileInfo.Attributes != FileAttributes.Directory)
                            isPathCorrect = true;
                        else
                            MessageBox.Show(string.Format("配置文件中{0}配置项所填写的主语言对应lang文件在本地Working Copy中的路径（{1}）无效，其为文件夹而不是文件，请手工选择合法路径", AppValues.CONFIG_KEY_COMMIT_LANG_FILE_PATH, exportLangFilePath), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    catch
                    {
                        MessageBox.Show(string.Format("配置文件中{0}配置项所填写的主语言对应lang文件在本地Working Copy中的路径（{1}）无效，请手工选择合法路径", AppValues.CONFIG_KEY_COMMIT_LANG_FILE_PATH, exportLangFilePath), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                    MessageBox.Show(string.Format("配置文件中不含用于配制主语言对应lang文件在本地Working Copy中的路径的配置项（{0}），请手工选择合法路径", AppValues.CONFIG_KEY_COMMIT_LANG_FILE_PATH), "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (isPathCorrect == false)
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.ValidateNames = true;
                    dialog.Title = "请选择主语言对应lang文件的导出路径";
                    if (dialog.ShowDialog() == DialogResult.OK)
                        exportLangFilePath = dialog.FileName;
                    else
                    {
                        MessageBox.Show("未选择主语言对应lang文件的导出路径，无法进行lang文件导出，请选择后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            else if (AppValues.COMMIT_LANG_FILE_OPTINOS[2].Equals(cmbCommitLangFile.SelectedItem.ToString()))
            {
                bool isPathCorrect = false;
                if (AppValues.Config.ContainsKey(AppValues.CONFIG_KEY_COMMIT_LANG_FILE_PATH))
                {
                    exportLangFilePath = AppValues.Config[AppValues.CONFIG_KEY_COMMIT_LANG_FILE_PATH];
                    // 检查文件是否存在
                    if (File.Exists(exportLangFilePath) == false)
                        MessageBox.Show(string.Format("配置文件中{0}配置项所填写的主语言对应lang文件在本地Working Copy中的路径（{1}）不存在，请手工选择合法路径", AppValues.CONFIG_KEY_COMMIT_LANG_FILE_PATH, exportLangFilePath), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    else
                    {
                        // 检查文件是否处于SVN管理下
                        SvnException getLangFileInfoSvnException = null;
                        SvnInfoEventArgs localFileInfo = OperateSvnHelper.GetLocalFileInfo(exportLangFilePath, out getLangFileInfoSvnException);
                        if (getLangFileInfoSvnException != null)
                        {
                            if (getLangFileInfoSvnException is SvnInvalidNodeKindException)
                                MessageBox.Show(string.Format("配置文件中{0}配置项所填写的主语言对应lang文件在本地Working Copy中的路径（{1}）不在SVN管理下，请手工选择合法路径", AppValues.CONFIG_KEY_COMMIT_LANG_FILE_PATH, exportLangFilePath), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            else
                                MessageBox.Show(string.Format("配置文件中{0}配置项所填写的主语言对应lang文件在本地Working Copy中的路径（{1}）无效，错误原因为：{2}，请手工选择合法路径", AppValues.CONFIG_KEY_COMMIT_LANG_FILE_PATH, exportLangFilePath, getLangFileInfoSvnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                            isPathCorrect = true;
                    }
                }
                else
                    MessageBox.Show(string.Format("配置文件中不含用于配制主语言对应lang文件在本地Working Copy中的路径的配置项（{0}），请手工选择合法路径", AppValues.CONFIG_KEY_COMMIT_LANG_FILE_PATH), "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                if (isPathCorrect == false)
                {
                    SaveFileDialog dialog = new SaveFileDialog();
                    dialog.ValidateNames = true;
                    dialog.Title = "请选择主语言对应lang文件在本地Working Copy的路径";
                    if (dialog.ShowDialog() == DialogResult.OK)
                    {
                        exportLangFilePath = dialog.FileName;
                        // 检查文件是否处于SVN管理下
                        SvnException getLangFileInfoSvnException = null;
                        SvnInfoEventArgs localFileInfo = OperateSvnHelper.GetLocalFileInfo(exportLangFilePath, out getLangFileInfoSvnException);
                        if (getLangFileInfoSvnException != null)
                        {
                            if (getLangFileInfoSvnException is SvnInvalidNodeKindException)
                            {
                                MessageBox.Show(string.Format("选择的主语言对应lang文件在本地Working Copy中的路径（{0}）不在SVN管理下，无法进行lang文件导出，请重新选择后重试", exportLangFilePath), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                MessageBox.Show(string.Format("选择的主语言对应lang文件在本地Working Copy中的路径（{0}）无效，错误原因为：{1}，请重新选择后重试", exportLangFilePath, getLangFileInfoSvnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("未选择主语言对应lang文件在本地Working Copy中的路径，无法进行lang文件导出，请选择后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }

            // 记录用户是否选择了至少一处需要合并到SVN中的差异内容
            bool isChooseCommitChange = false;
            // 记录未对主语言译文不同的差异条目进行处理方式选择的编号
            List<int> unresolvedDiffInfoNum = new List<int>();
            // 记录未对本地表新增Key条目进行处理方式选择的编号
            List<int> unresolvedLocalAddKeyNum = new List<int>();
            // 记录未对SVN表新增Key条目进行处理方式选择的编号
            List<int> unresolvedSvnAddKeyNum = new List<int>();
            // 记录要进行提交的差异项
            CommitCompareResult commitCompareResult = new CommitCompareResult();

            // 检查用户是否对每一条差异都选择了处理方式
            // 主语言译文不同
            const string PART_DIFF_INFO_RESOLVE_CONFLICT_WAY_COLUMN_NAME = _PART_NAME_DIFF_INFO + _RESOLVE_CONFLICT_WAY_COLUMN_NAME;
            int diffInfoCount = _compareResult.DiffInfo.Count;
            for (int i = 0; i < diffInfoCount; ++i)
            {
                CommitDifferentDefaultLanguageInfo oneDiffInfo = _compareResult.DiffInfo[i];
                string selectedValue = _partControls[_PART_NAME_DIFF_INFO].DataGridView.Rows[i].Cells[PART_DIFF_INFO_RESOLVE_CONFLICT_WAY_COLUMN_NAME].Value as string;
                if (AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[0].Equals(selectedValue))
                {
                    oneDiffInfo.ResolveConflictWay = ResolveConflictWays.UseLocal;
                    commitCompareResult.DiffInfo.Add(oneDiffInfo);
                    isChooseCommitChange = true;
                }
                else if (AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[1].Equals(selectedValue))
                    oneDiffInfo.ResolveConflictWay = ResolveConflictWays.UseSvn;
                else
                {
                    oneDiffInfo.ResolveConflictWay = ResolveConflictWays.NotChoose;
                    int num = i + 1;
                    unresolvedDiffInfoNum.Add(num);
                }
            }
            // 本地表新增Key
            const string PART_LOCAL_ADD_KEY_RESOLVE_CONFLICT_WAY_COLUMN_NAME = _PART_NAME_LOCAL_ADD_KEY + _RESOLVE_CONFLICT_WAY_COLUMN_NAME;
            int localAddKeyCount = _compareResult.LocalAddKeyInfo.Count;
            for (int i = 0; i < localAddKeyCount; ++i)
            {
                CommitDifferentKeyInfo oneLocalAddKey = _compareResult.LocalAddKeyInfo[i];
                string selectedValue = _partControls[_PART_NAME_LOCAL_ADD_KEY].DataGridView.Rows[i].Cells[PART_LOCAL_ADD_KEY_RESOLVE_CONFLICT_WAY_COLUMN_NAME].Value as string;
                if (AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[0].Equals(selectedValue))
                {
                    oneLocalAddKey.ResolveConflictWay = ResolveConflictWays.UseLocal;
                    commitCompareResult.LocalAddKeyInfo.Add(oneLocalAddKey);
                    isChooseCommitChange = true;
                }
                else if (AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[1].Equals(selectedValue))
                    oneLocalAddKey.ResolveConflictWay = ResolveConflictWays.UseSvn;
                else
                {
                    oneLocalAddKey.ResolveConflictWay = ResolveConflictWays.NotChoose;
                    int num = i + 1;
                    unresolvedLocalAddKeyNum.Add(num);
                }
            }
            // SVN表新增Key
            const string PART_SVN_ADD_KEY_RESOLVE_CONFLICT_WAY_COLUMN_NAME = _PART_NAME_SVN_ADD_KEY + _RESOLVE_CONFLICT_WAY_COLUMN_NAME;
            int svnAddKeyCount = _compareResult.SvnAddKeyInfo.Count;
            for (int i = 0; i < svnAddKeyCount; ++i)
            {
                CommitDifferentKeyInfo oneSvnAddKey = _compareResult.SvnAddKeyInfo[i];
                string selectedValue = _partControls[_PART_NAME_SVN_ADD_KEY].DataGridView.Rows[i].Cells[PART_SVN_ADD_KEY_RESOLVE_CONFLICT_WAY_COLUMN_NAME].Value as string;
                if (AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[0].Equals(selectedValue))
                {
                    oneSvnAddKey.ResolveConflictWay = ResolveConflictWays.UseLocal;
                    commitCompareResult.SvnAddKeyInfo.Add(oneSvnAddKey);
                    isChooseCommitChange = true;
                }
                else if (AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[1].Equals(selectedValue))
                    oneSvnAddKey.ResolveConflictWay = ResolveConflictWays.UseSvn;
                else
                {
                    oneSvnAddKey.ResolveConflictWay = ResolveConflictWays.NotChoose;
                    int num = i + 1;
                    unresolvedSvnAddKeyNum.Add(num);
                }
            }

            StringBuilder unresolvedConflictStringBuilder = new StringBuilder();
            if (unresolvedDiffInfoNum.Count > 0)
            {
                unresolvedConflictStringBuilder.Append("主语言译文不同的差异条目中，以下编号的行未选择处理方式：");
                unresolvedConflictStringBuilder.AppendLine(Utils.CombineString<int>(unresolvedDiffInfoNum, ","));
            }
            if (unresolvedLocalAddKeyNum.Count > 0)
            {
                unresolvedConflictStringBuilder.Append("本地表新增Key的差异条目中，以下编号的行未选择处理方式：");
                unresolvedConflictStringBuilder.AppendLine(Utils.CombineString<int>(unresolvedLocalAddKeyNum, ","));
            }
            if (unresolvedSvnAddKeyNum.Count > 0)
            {
                unresolvedConflictStringBuilder.Append("SVN表新增Key的差异条目中，以下编号的行未选择处理方式：");
                unresolvedConflictStringBuilder.AppendLine(Utils.CombineString<int>(unresolvedSvnAddKeyNum, ","));
            }
            string unresolvedConflictString = unresolvedConflictStringBuilder.ToString();
            if (!string.IsNullOrEmpty(unresolvedConflictString))
            {
                MessageBox.Show(string.Concat("存在以下未选择处理方式的差异条目，请全部选择处理方式后重试\n\n", unresolvedConflictString), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 判断用户是否选择了至少一处需要合并到SVN中的差异内容
            if (isChooseCommitChange == false)
            {
                MessageBox.Show("未选择将任一差异提交到SVN表，无需进行提交操作", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 即将提交时判断此时SVN中最新版本号是否还是进入此界面时与本地表对比的版本号，如果不是需要重新对比
            SvnException svnException = null;
            SvnInfoEventArgs svnFileInfo = OperateSvnHelper.GetSvnFileInfo(AppValues.SvnExcelFilePath, out svnException);
            if (svnException == null)
            {
                // 若此时SVN最新版本仍旧为之前与之对比的版本，则根据用户选择将结果合并到最新SVN母表副本中然后上传
                if (svnFileInfo.LastChangeRevision == _compareResult.SvnFileRevision)
                {
                    string errorString = null;
                    string mergedExcelFilePath = CommitExcelFileHelper.GenerateCommitExcelFile(_compareResult, _localExcelInfo, _newestSvnExcelInfo, out errorString);
                    if (errorString != null)
                    {
                        MessageBox.Show(string.Format("生成合并之后的Excel母表文件失败，错误原因为：{0}\n\n提交操作被迫中止", errorString), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    // 进行SVN提交操作（将本地Working Copy文件备份至本工具所在路径，然后用合并后的Excel母表替换Working Copy文件后执行SVN提交操作）
                    string backupFilePath = Utils.CombinePath(AppValues.PROGRAM_FOLDER_PATH, string.Format("备份自己修改的本地表 {0:yyyy年MM月dd日 HH时mm分ss秒} 对应SVN版本号{1}.xlsx", DateTime.Now, _localExcelInfo.Revision));
                    try
                    {
                        File.Copy(AppValues.LocalExcelFilePath, backupFilePath, true);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(string.Format("自动备份本地表至本工具所在路径失败，错误原因为：{0}\n\n为了防止因不备份导致自己编辑的原始本地表丢失，提交SVN操作被迫中止", exception.ToString()), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    // 因为SVN提交文件需要在同一版本号下进行且SVN无法在Update时对Excel进行Merge操作，则如果本地表的版本号低于SVN最新版本，必须将本地表执行Revert和Update操作后才可以提交
                    if (_compareResult.LocalFileRevision != _compareResult.SvnFileRevision)
                    {
                        // Revert操作
                        bool revertResult = OperateSvnHelper.Revert(AppValues.LocalExcelFilePath, out svnException);
                        if (svnException == null)
                        {
                            if (revertResult == false)
                            {
                                MessageBox.Show("因本地表版本不是SVN中最新的，必须执行Revert以及Update操作后才可以提交\n但因为Revert失败，提交SVN操作被迫中止", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show(string.Format("因本地表版本不是SVN中最新的，必须执行Revert以及Update操作后才可以提交\n但因为Revert失败，错误原因为：{0}\n提交SVN操作被迫中止", svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        // Update操作
                        bool updateResult = OperateSvnHelper.Update(AppValues.LocalExcelFilePath, out svnException);
                        if (svnException == null)
                        {
                            if (updateResult == false)
                            {
                                MessageBox.Show("因本地表版本不是SVN中最新的，必须执行Revert以及Update操作后才可以提交\n但因为Update失败，提交SVN操作被迫中止", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            if (svnException is SvnAuthorizationException || svnException is SvnOperationCanceledException)
                            {
                                MessageBox.Show("因本地表版本不是SVN中最新的，必须执行Revert以及Update操作后才可以提交\n但因为没有权限进行Update操作，提交SVN操作被迫中止，请输入合法的SVN账户信息后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                MessageBox.Show(string.Format("因本地表版本不是SVN中最新的，必须执行Revert以及Update操作后才可以提交\n但因为Update失败，错误原因为：{0}\n提交SVN操作被迫中止", svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                    // 用合并后的Excel文件覆盖掉原有的本地表
                    try
                    {
                        File.Copy(mergedExcelFilePath, AppValues.LocalExcelFilePath, true);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(string.Format("将合并后的Excel母表覆盖到本地表所在路径失败，错误原因为：{0}\n\n提交SVN操作被迫中止", exception), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    // 执行提交操作
                    bool commitResult = OperateSvnHelper.Commit(AppValues.LocalExcelFilePath, logMessage, out svnException);
                    if (svnException == null)
                    {
                        if (commitResult == false)
                        {
                            MessageBox.Show(string.Concat("执行提交操作失败，原因为", svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        else
                        {
                            string commitExcelFileSuccessTips = string.Format("执行提交操作成功，自己修改的原始本地表备份至：{0}\n\n", backupFilePath);
                            // 如果选择了一并导出、提交主语言对应的lang文件
                            if (!AppValues.COMMIT_LANG_FILE_OPTINOS[0].Equals(cmbCommitLangFile.SelectedItem.ToString()))
                            {
                                // 进行lang文件导出
                                // 解析Excel母表
                                LangExcelInfo langExcelInfo = AnalyzeHelper.AnalyzeLangExcelFile(AppValues.LocalExcelFilePath, AppValues.CommentLineStartChar, out errorString);
                                if (errorString == null)
                                {
                                    if (ExportLangFileHelper.ExportLangFile(langExcelInfo, langExcelInfo.DefaultLanguageInfo.Name, exportLangFilePath, keyAndValueSplitChar, out errorString) == true)
                                    {
                                        // 提交lang文件
                                        if (AppValues.COMMIT_LANG_FILE_OPTINOS[2].Equals(cmbCommitLangFile.SelectedItem.ToString()))
                                        {
                                            // 先执行Revert和Update操作
                                            string exportLangFileSuccessTips = string.Format("导出主语言对应的lang文件成功，导出路径为：{0}\n\n", exportLangFilePath);
                                            bool revertResult = OperateSvnHelper.Revert(exportLangFilePath, out svnException);
                                            if (svnException == null)
                                            {
                                                if (revertResult == false)
                                                {
                                                    MessageBox.Show(string.Format("{0}{1}但提交至SVN失败，因为对Working Copy中的lang文件执行Revert失败", commitExcelFileSuccessTips, exportLangFileSuccessTips), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show(string.Format("{0}{1}但提交至SVN失败，因为对Working Copy中的lang文件执行Revert失败，错误原因为：{2}", commitExcelFileSuccessTips, exportLangFileSuccessTips, svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                return;
                                            }
                                            bool updateResult = OperateSvnHelper.Update(exportLangFilePath, out svnException);
                                            if (svnException == null)
                                            {
                                                if (updateResult == false)
                                                {
                                                    MessageBox.Show(string.Format("{0}{1}但提交至SVN失败，因为对Working Copy中的lang文件执行Update失败", commitExcelFileSuccessTips, exportLangFileSuccessTips), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                    return;
                                                }
                                            }
                                            else
                                            {
                                                MessageBox.Show(string.Format("{0}{1}但提交至SVN失败，因为对Working Copy中的lang文件执行Update失败，错误原因为：{2}", commitExcelFileSuccessTips, exportLangFileSuccessTips, svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                return;
                                            }
                                            // 执行提交操作
                                            commitResult = OperateSvnHelper.Commit(exportLangFilePath, logMessage, out svnException);
                                            if (commitResult == true)
                                            {
                                                MessageBox.Show(string.Format("{0}{1}提交lang文件至SVN成功", commitExcelFileSuccessTips, exportLangFileSuccessTips), "恭喜", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                                this.Close();
                                                return;
                                            }
                                            else
                                            {
                                                MessageBox.Show(string.Format("{0}{1}但提交至SVN失败，错误原因为{2}", commitExcelFileSuccessTips, exportLangFileSuccessTips, svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                                return;
                                            }
                                        }
                                        else
                                        {
                                            MessageBox.Show(string.Format("{0}导出主语言对应的lang文件成功，导出路径为：{1}", commitExcelFileSuccessTips, exportLangFilePath), "恭喜", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                            this.Close();
                                            return;
                                        }
                                    }
                                    else
                                    {
                                        MessageBox.Show(string.Format("{0}导出主语言对应的lang文件失败，错误原因为：{1}，请修正后尝试用主界面中的lang文件导出功能手工导出", commitExcelFileSuccessTips, errorString), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        return;
                                    }
                                }
                                else
                                {
                                    MessageBox.Show(string.Format("{0}导出主语言对应的lang文件失败，因为解析母表时发现以下错误：{1}，请修正后尝试用主界面中的lang文件导出功能手工导出", commitExcelFileSuccessTips, errorString), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }
                            }
                            else
                            {
                                MessageBox.Show(commitExcelFileSuccessTips, "恭喜", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                this.Close();
                                return;
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show(string.Format("执行提交操作失败，错误原因为：{0}\n\n请修正错误后重试", svnException.RootCause.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(string.Format("很遗憾，在提交时发现了更新的SVN表的版本（{0}），需要重新对比与新表的差异（本次新发现的差异项会将“编号”列的单元格背景调为橙色突出显示）然后选择处理方式后再提交\n\n但之前已选择的差异项的处理方式将被保留，直接在下拉列表中默认选中刚才你选择的处理方式\n\n注意：在展示主语言不同（第1个表格）以及SVN表新增Key（第2个表格）的表格中，若两次SVN表主语言译文发生变动，会将“SVN表主语言译文”列的单元格背景调为橙色突出显示", svnFileInfo.LastChangeRevision), "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    // 需要下载SVN最新版本表与本地表进行对比
                    string svnNewRevisionCopySavePath = Utils.CombinePath(AppValues.PROGRAM_FOLDER_PATH, string.Format("SVN最新母表副本 {0:yyyy年MM月dd日 HH时mm分ss秒} 对应SVN版本号{1}.xlsx", DateTime.Now, svnFileInfo.LastChangeRevision));
                    Exception exportException;
                    bool result = OperateSvnHelper.ExportSvnFileToLocal(AppValues.SvnExcelFilePath, svnNewRevisionCopySavePath, svnFileInfo.LastChangeRevision, out exportException);
                    if (exportException != null)
                    {
                        MessageBox.Show(string.Format("下载SVN中最新版本号（{0}）的母表文件存到本地失败，错误原因为：{1}", svnFileInfo.LastChangeRevision, exportException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                    else if (result == false)
                    {
                        MessageBox.Show(string.Format("下载SVN中最新版本号（{0}）的母表文件存到本地失败", svnFileInfo.LastChangeRevision), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                    // 解析SVN中最新的母表文件
                    string errorString;
                    CommitExcelInfo newRevisionExcelInfo = CommitExcelFileHelper.AnalyzeCommitExcelFile(svnNewRevisionCopySavePath, AppValues.CommentLineStartChar, svnFileInfo.LastChangeRevision, out errorString);
                    if (errorString != null)
                    {
                        MessageBox.Show(string.Format("下载的SVN中最新版本号（{0}）的母表文件存在以下错误，请修正后重试\n\n{1}", svnFileInfo.LastChangeRevision, errorString), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Close();
                        return;
                    }
                    _newestSvnExcelInfo = newRevisionExcelInfo;
                    // 对比本地表与SVN中最新母表文件
                    CommitCompareResult newCompareResult = CommitExcelFileHelper.CompareCommitExcelFile(_localExcelInfo, newRevisionExcelInfo);
                    // 如果最新的SVN中母表与本地表反而没有任何差异，则无需提交
                    if (newCompareResult.IsHasDiff() == false)
                    {
                        _compareResult = newCompareResult;
                        // 清空DataGridView中的内容
                        _CleanDataGridView();
                        // 更新SVN版本号显示
                        txtSvnFileRevision.Text = svnFileInfo.LastChangeRevision.ToString();
                        txtSvnFileRevision.BackColor = Color.Orange;

                        MessageBox.Show("经对比发现本地母表与SVN中最新版本内容完全相同，无需进行提交操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    _RefreshDataGridView(newCompareResult);
                }
            }
            else
            {
                MessageBox.Show(string.Format("无法获取SVN中最新母表信息，错误原因为：{0}\n\n提交操作被迫中止", svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        // 用本地表与最新SVN表重新对比的结果刷新DataGridView中的数据
        private void _RefreshDataGridView(CommitCompareResult newCompareResult)
        {
            // 更新SVN版本号显示
            txtSvnFileRevision.Text = newCompareResult.SvnFileRevision.ToString();
            txtSvnFileRevision.BackColor = Color.Orange;
            // 清空DataGridView中的内容
            _CleanDataGridView();
            // 显示“版本变动”列
            _ShowIsChangedBySvnRevisionColumn(true);

            // 重新生成DataGridView中的数据
            // 主语言译文不同
            const string PART_DIFF_INFO_NUM_COLUMN_NAME = _PART_NAME_DIFF_INFO + _NUM_COLUMN_NAME;
            const string PART_DIFF_INFO_RESOLVE_CONFLICT_WAY_COLUMN_NAME = _PART_NAME_DIFF_INFO + _RESOLVE_CONFLICT_WAY_COLUMN_NAME;
            const string PART_DIFF_INFO_IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME = _PART_NAME_DIFF_INFO + _IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME;
            const string PART_DIFF_INFO_SVN_DEFAULT_LANGUAGE_COLUMN_NAME = _PART_NAME_DIFF_INFO + _SVN_DEFAULT_LANGUAGE_COLUMN_NAME;
            int diffInfoCount = newCompareResult.DiffInfo.Count;
            for (int i = 0; i < diffInfoCount; ++i)
            {
                CommitDifferentDefaultLanguageInfo oneDiffInfo = newCompareResult.DiffInfo[i];
                List<object> showDataList = new List<object>();
                // 编号
                showDataList.Add(i + 1);
                // Key
                showDataList.Add(oneDiffInfo.Key);
                // 本地表主语言译文
                showDataList.Add(oneDiffInfo.LocalFileDefaultLanguageValue);
                // SVN表主语言译文
                showDataList.Add(oneDiffInfo.SvnFileDefaultLanguageValue);
                // 本地表行号
                showDataList.Add(oneDiffInfo.LocalFileLineNum);
                // SVN表行号
                showDataList.Add(oneDiffInfo.SvnFileLineNum);

                // 判断此差异是否为SVN中两版本本身存在的差异
                bool isChangedBySvnRevision = false;
                if (_svnSameRevisionExcelInfo.Keys.Contains(oneDiffInfo.Key) && _localExcelInfo.GetDataByKey(oneDiffInfo.Key).Equals(_svnSameRevisionExcelInfo.GetDataByKey(oneDiffInfo.Key)))
                    isChangedBySvnRevision = true;

                showDataList.Add(isChangedBySvnRevision == true ? "是" : string.Empty);

                DataGridView dataGridView = _partControls[_PART_NAME_DIFF_INFO].DataGridView;
                int index = dataGridView.Rows.Add(showDataList.ToArray());

                // 判断此条差异是否属于上次对比已经发现并选择了处理方式的项，如果与之前对比结果有完全相同的差异项，直接使用用户上次设置的处理结果（但如果两次SVN表的译文不同，则处理方式下拉列表不进行默认选择）
                bool hasChooseResolveConflictWay = false;
                // 判断此项差异的SVN最新表的主语言译文与上次SVN版本表的译文是否一致
                bool isSameValue = false;
                ResolveConflictWays lastChooseResolveConflictWay = ResolveConflictWays.NotChoose;
                int lastDiffInfoCount = _compareResult.DiffInfo.Count;
                for (int j = 0; j < lastDiffInfoCount; ++j)
                {
                    CommitDifferentDefaultLanguageInfo oneLastDiffInfo = _compareResult.DiffInfo[j];
                    if (oneLastDiffInfo.Key.Equals(oneDiffInfo.Key))
                    {
                        hasChooseResolveConflictWay = true;

                        if (oneLastDiffInfo.SvnFileDefaultLanguageValue.Equals(oneDiffInfo.SvnFileDefaultLanguageValue))
                        {
                            isSameValue = true;
                            lastChooseResolveConflictWay = oneLastDiffInfo.ResolveConflictWay;
                        }

                        break;
                    }
                }

                if (hasChooseResolveConflictWay == true)
                {
                    // 如果此条差异存在于上一次的对比结果中，两次SVN表的主语言译文也相同，直接将ComboBox选中用户之前的处理方式
                    if (isSameValue == true)
                    {
                        if (lastChooseResolveConflictWay == ResolveConflictWays.UseLocal)
                            dataGridView.Rows[index].Cells[PART_DIFF_INFO_RESOLVE_CONFLICT_WAY_COLUMN_NAME].Value = AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[0];
                        else
                            dataGridView.Rows[index].Cells[PART_DIFF_INFO_RESOLVE_CONFLICT_WAY_COLUMN_NAME].Value = AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[1];
                    }
                    // 两次SVN表主语言译文不同，上次对比后的选择无法沿用需重选，将“SVN表主语言”以及“编号”列的背景设为橙色突出显示
                    else
                    {
                        dataGridView.Rows[index].Cells[PART_DIFF_INFO_SVN_DEFAULT_LANGUAGE_COLUMN_NAME].Style.BackColor = Color.Orange;
                        dataGridView.Rows[index].Cells[PART_DIFF_INFO_NUM_COLUMN_NAME].Style.BackColor = Color.Orange;
                    }
                }
                // 对于新的差异项，将“编号”列的单元格设为橙色，提示用户进行处理方式的选择
                else
                    dataGridView.Rows[index].Cells[PART_DIFF_INFO_NUM_COLUMN_NAME].Style.BackColor = Color.Orange;

                // 如果差异来自SVN版本的变动，则“版本变动”列的单元格背景色变为浅灰色，并且如果用户没有做出处理方式选择就默认将“处理方式”的ComboBox选择为“使用SVN表”
                if (isChangedBySvnRevision == true)
                {
                    dataGridView.Rows[index].Cells[PART_DIFF_INFO_IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME].Style.BackColor = Color.LightGray;
                    if (hasChooseResolveConflictWay == false)
                        dataGridView.Rows[index].Cells[PART_DIFF_INFO_RESOLVE_CONFLICT_WAY_COLUMN_NAME].Value = AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[1];
                }
            }

            // 本地表新增Key信息（本地表含有但最新SVN表中没有某个Key）
            const string PART_LOCAL_ADD_KEY_NUM_COLUMN_NAME = _PART_NAME_LOCAL_ADD_KEY + _NUM_COLUMN_NAME;
            const string PART_LOCAL_ADD_KEY_RESOLVE_CONFLICT_WAY_COLUMN_NAME = _PART_NAME_LOCAL_ADD_KEY + _RESOLVE_CONFLICT_WAY_COLUMN_NAME;
            const string PART_LOCAL_ADD_KEY_IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME = _PART_NAME_LOCAL_ADD_KEY + _IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME;
            int localAddKeyCount = newCompareResult.LocalAddKeyInfo.Count;
            for (int i = 0; i < localAddKeyCount; ++i)
            {
                CommitDifferentKeyInfo oneLocalAddKeyInfo = newCompareResult.LocalAddKeyInfo[i];
                List<object> showDataList = new List<object>();
                // 编号
                showDataList.Add(i + 1);
                // Key
                showDataList.Add(oneLocalAddKeyInfo.Key);
                // 本地表主语言译文
                showDataList.Add(oneLocalAddKeyInfo.DefaultLanguageValue);
                // 本地表行号
                showDataList.Add(oneLocalAddKeyInfo.ExcelLineNum);

                // 判断此差异是否为SVN中两版本本身存在的差异
                bool isChangedBySvnRevision = false;
                if (_svnSameRevisionExcelInfo.Keys.Contains(oneLocalAddKeyInfo.Key))
                    isChangedBySvnRevision = true;

                showDataList.Add(isChangedBySvnRevision == true ? "是" : string.Empty);

                DataGridView dataGridView = _partControls[_PART_NAME_LOCAL_ADD_KEY].DataGridView;
                int index = dataGridView.Rows.Add(showDataList.ToArray());

                // 判断此条差异是否属于上次对比已经发现并选择了处理方式的本地表新增Key项，如果是则直接使用用户上次设置的处理结果
                ResolveConflictWays lastChooseResolveConflictWay = ResolveConflictWays.NotChoose;
                int lastLocalAddKeyCount = _compareResult.LocalAddKeyInfo.Count;
                for (int j = 0; j < lastLocalAddKeyCount; ++j)
                {
                    CommitDifferentKeyInfo oneLastLocalAddKey = _compareResult.LocalAddKeyInfo[j];
                    if (oneLastLocalAddKey.Key.Equals(oneLocalAddKeyInfo.Key))
                    {
                        lastChooseResolveConflictWay = oneLastLocalAddKey.ResolveConflictWay;
                        break;
                    }
                }
                // 如果此条差异存在于上一次的对比结果中，直接将ComboBox选中用户之前的处理方式
                if (lastChooseResolveConflictWay != ResolveConflictWays.NotChoose)
                {
                    if (lastChooseResolveConflictWay == ResolveConflictWays.UseLocal)
                        dataGridView.Rows[index].Cells[PART_LOCAL_ADD_KEY_RESOLVE_CONFLICT_WAY_COLUMN_NAME].Value = AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[0];
                    else
                        dataGridView.Rows[index].Cells[PART_LOCAL_ADD_KEY_RESOLVE_CONFLICT_WAY_COLUMN_NAME].Value = AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[1];
                }
                // 对于新的差异项，将“编号”列的单元格设为橙色，提示用户进行处理方式的选择
                else
                    dataGridView.Rows[index].Cells[PART_LOCAL_ADD_KEY_NUM_COLUMN_NAME].Style.BackColor = Color.Orange;

                // 如果差异来自SVN版本的变动，则“版本变动”列的单元格背景色变为浅灰色，并且如果用户没有做出处理方式选择就默认将“处理方式”的ComboBox选择为“使用SVN表”
                if (isChangedBySvnRevision == true)
                {
                    dataGridView.Rows[index].Cells[PART_LOCAL_ADD_KEY_IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME].Style.BackColor = Color.LightGray;
                    if (lastChooseResolveConflictWay == ResolveConflictWays.NotChoose)
                        dataGridView.Rows[index].Cells[PART_LOCAL_ADD_KEY_RESOLVE_CONFLICT_WAY_COLUMN_NAME].Value = AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[1];
                }
            }

            // SVN表新增Key信息（本地表没有但最新SVN表中含有某个Key）
            const string PART_SVN_ADD_KEY_NUM_COLUMN_NAME = _PART_NAME_SVN_ADD_KEY + _NUM_COLUMN_NAME;
            const string PART_SVN_ADD_KEY_RESOLVE_CONFLICT_WAY_COLUMN_NAME = _PART_NAME_SVN_ADD_KEY + _RESOLVE_CONFLICT_WAY_COLUMN_NAME;
            const string PART_SVN_ADD_KEY_IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME = _PART_NAME_SVN_ADD_KEY + _IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME;
            const string PART_SVN_ADD_KEY_SVN_DEFAULT_LANGUAGE_COLUMN_NAME = _PART_NAME_SVN_ADD_KEY + _SVN_DEFAULT_LANGUAGE_COLUMN_NAME;
            int svnAddKeyCount = newCompareResult.SvnAddKeyInfo.Count;
            for (int i = 0; i < svnAddKeyCount; ++i)
            {
                CommitDifferentKeyInfo oneSvnAddKeyInfo = newCompareResult.SvnAddKeyInfo[i];
                List<object> showDataList = new List<object>();
                // 编号
                showDataList.Add(i + 1);
                // Key
                showDataList.Add(oneSvnAddKeyInfo.Key);
                // SVN表主语言译文
                showDataList.Add(oneSvnAddKeyInfo.DefaultLanguageValue);
                // SVN表行号
                showDataList.Add(oneSvnAddKeyInfo.ExcelLineNum);

                // 判断此差异是否为SVN中两版本本身存在的差异
                bool isChangedBySvnRevision = true;
                if (_svnSameRevisionExcelInfo.Keys.Contains(oneSvnAddKeyInfo.Key))
                    isChangedBySvnRevision = false;

                showDataList.Add(isChangedBySvnRevision == true ? "是" : string.Empty);

                DataGridView dataGridView = _partControls[_PART_NAME_SVN_ADD_KEY].DataGridView;
                int index = dataGridView.Rows.Add(showDataList.ToArray());

                // 判断此条差异是否属于上次对比已经发现并选择了处理方式的SVN表新增Key项，如果是则直接使用用户上次设置的处理结果（但如果两次SVN表均存在此Key且两次主语言不同，则处理方式下拉列表不进行默认选择）
                bool hasChooseResolveConflictWay = false;
                // 判断此项差异的SVN最新表的主语言译文与上次SVN版本表的译文是否一致
                bool isSameValue = false;
                ResolveConflictWays lastChooseResolveConflictWay = ResolveConflictWays.NotChoose;
                int lastSvnAddKeyCount = _compareResult.SvnAddKeyInfo.Count;
                for (int j = 0; j < lastSvnAddKeyCount; ++j)
                {
                    CommitDifferentKeyInfo oneLastSvnAddKey = _compareResult.SvnAddKeyInfo[j];
                    if (oneLastSvnAddKey.Key.Equals(oneSvnAddKeyInfo.Key))
                    {
                        hasChooseResolveConflictWay = true;

                        if (oneLastSvnAddKey.DefaultLanguageValue.Equals(oneSvnAddKeyInfo.DefaultLanguageValue))
                        {
                            isSameValue = true;
                            lastChooseResolveConflictWay = oneLastSvnAddKey.ResolveConflictWay;
                        }

                        break;
                    }
                }
                if (hasChooseResolveConflictWay == true)
                {
                    // 如果此条差异存在于上一次的对比结果中，两次SVN表的主语言译文也相同，直接将ComboBox选中用户之前的处理方式
                    if (isSameValue == true)
                    {
                        if (lastChooseResolveConflictWay == ResolveConflictWays.UseLocal)
                            dataGridView.Rows[index].Cells[PART_SVN_ADD_KEY_RESOLVE_CONFLICT_WAY_COLUMN_NAME].Value = AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[0];
                        else
                            dataGridView.Rows[index].Cells[PART_SVN_ADD_KEY_RESOLVE_CONFLICT_WAY_COLUMN_NAME].Value = AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[1];
                    }
                    // 两次SVN表主语言译文不同，上次对比后的选择无法沿用需重选，将“SVN表主语言”以及“编号”列的背景设为橙色突出显示
                    else
                    {
                        dataGridView.Rows[index].Cells[PART_SVN_ADD_KEY_SVN_DEFAULT_LANGUAGE_COLUMN_NAME].Style.BackColor = Color.Orange;
                        dataGridView.Rows[index].Cells[PART_SVN_ADD_KEY_NUM_COLUMN_NAME].Style.BackColor = Color.Orange;
                    }
                }
                // 对于新的差异项，将“编号”列的单元格设为橙色，提示用户进行处理方式的选择
                else
                    dataGridView.Rows[index].Cells[PART_SVN_ADD_KEY_NUM_COLUMN_NAME].Style.BackColor = Color.Orange;

                // 如果差异来自SVN版本的变动，则“版本变动”列的单元格背景色变为浅灰色，并且如果用户没有做出处理方式选择就默认将“处理方式”的ComboBox选择为“使用SVN表”
                if (isChangedBySvnRevision == true)
                {
                    dataGridView.Rows[index].Cells[PART_SVN_ADD_KEY_IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME].Style.BackColor = Color.LightGray;
                    if (hasChooseResolveConflictWay == false)
                        dataGridView.Rows[index].Cells[PART_SVN_ADD_KEY_RESOLVE_CONFLICT_WAY_COLUMN_NAME].Value = AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[1];
                }
            }

            _compareResult = newCompareResult;
        }

        // 用刚进入此界面时的对比结果初始化DataGridView中的数据
        private void _InitDataGridView(CommitCompareResult compareResult)
        {
            // 本地表以及SVN表的版本号
            txtLocalFileRevision.Text = compareResult.LocalFileRevision.ToString();
            txtSvnFileRevision.Text = compareResult.SvnFileRevision.ToString();
            bool isSameRevision = (compareResult.LocalFileRevision == compareResult.SvnFileRevision);
            // 若版本号相同，SVN表版本号TextBox背景色设为绿色否则为橙色
            if (isSameRevision == true)
                txtSvnFileRevision.BackColor = Color.LightGreen;
            else
                txtSvnFileRevision.BackColor = Color.Orange;
            // 若版本号不同，每个DataGridView中倒数第2列显示“版本变动”列，否则不显示
            _ShowIsChangedBySvnRevisionColumn(isSameRevision == false);

            // 如果版本号不同，需要下载SVN中与本地表相同版本号的母表文件与本地表进行对比
            CommitCompareResult compareSameRevisionResult = null;
            if (isSameRevision == false)
            {
                // 下载SVN中与本地表相同版本号的母表
                string svnSameRevisionCopySavePath = Utils.CombinePath(AppValues.PROGRAM_FOLDER_PATH, string.Format("SVN母表副本 {0:yyyy年MM月dd日 HH时mm分ss秒} 对应SVN版本号{1}.xlsx", DateTime.Now, compareResult.LocalFileRevision));
                Exception exportException;
                bool result = OperateSvnHelper.ExportSvnFileToLocal(AppValues.SvnExcelFilePath, svnSameRevisionCopySavePath, compareResult.LocalFileRevision, out exportException);
                if (exportException != null)
                {
                    MessageBox.Show(string.Format("下载SVN中与本地表相同版本号（{0}）的母表文件存到本地失败，错误原因为：{1}", compareResult.LocalFileRevision, exportException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                else if (result == false)
                {
                    MessageBox.Show(string.Format("下载SVN中与本地表相同版本号（{0}）的母表文件存到本地失败", compareResult.LocalFileRevision), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }

                // 解析SVN中与本地表相同版本号的母表
                string errorString;
                CommitExcelInfo svnSameRevisionExcelInfo = CommitExcelFileHelper.AnalyzeCommitExcelFile(svnSameRevisionCopySavePath, AppValues.CommentLineStartChar, compareResult.LocalFileRevision, out errorString);
                if (errorString != null)
                {
                    MessageBox.Show(string.Format("下载的SVN中与本地表相同版本号（{0}）的母表文件存在以下错误，请修正后重试\n\n{1}", compareResult.LocalFileRevision, errorString), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    this.Close();
                    return;
                }
                _svnSameRevisionExcelInfo = svnSameRevisionExcelInfo;

                // 对比两文件
                compareSameRevisionResult = CommitExcelFileHelper.CompareCommitExcelFile(_localExcelInfo, svnSameRevisionExcelInfo);
            }
            else
                _svnSameRevisionExcelInfo = _lastSvnExcelInfo;

            // Key相同但主语言译文不同的信息
            const string PART_DIFF_INFO_RESOLVE_CONFLICT_WAY_COLUMN_NAME = _PART_NAME_DIFF_INFO + _RESOLVE_CONFLICT_WAY_COLUMN_NAME;
            const string PART_DIFF_INFO_IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME = _PART_NAME_DIFF_INFO + _IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME;
            int diffInfoCount = compareResult.DiffInfo.Count;
            for (int i = 0; i < diffInfoCount; ++i)
            {
                CommitDifferentDefaultLanguageInfo oneDiffInfo = compareResult.DiffInfo[i];
                List<object> showDataList = new List<object>();
                // 编号
                showDataList.Add(i + 1);
                // Key
                showDataList.Add(oneDiffInfo.Key);
                // 本地表主语言译文
                showDataList.Add(oneDiffInfo.LocalFileDefaultLanguageValue);
                // SVN表主语言译文
                showDataList.Add(oneDiffInfo.SvnFileDefaultLanguageValue);
                // 本地表行号
                showDataList.Add(oneDiffInfo.LocalFileLineNum);
                // SVN表行号
                showDataList.Add(oneDiffInfo.SvnFileLineNum);

                // 版本变动
                bool isChangedBySvnRevision = false;
                if (isSameRevision == false)
                {
                    // 判断此差异是否为SVN中两版本本身存在的差异，如本地表与同版本SVN表相同，但与最新SVN表不同，则说明该项差异来自SVN版本的变动
                    if (_svnSameRevisionExcelInfo.Keys.Contains(oneDiffInfo.Key) && _localExcelInfo.GetDataByKey(oneDiffInfo.Key).Equals(_svnSameRevisionExcelInfo.GetDataByKey(oneDiffInfo.Key)))
                        isChangedBySvnRevision = true;

                    showDataList.Add(isChangedBySvnRevision == true ? "是" : string.Empty);
                }

                DataGridView dataGridView = _partControls[_PART_NAME_DIFF_INFO].DataGridView;
                int index = dataGridView.Rows.Add(showDataList.ToArray());
                // 如果差异来自SVN版本的变动，则“版本变动”列的单元格背景色变为浅灰色，并默认将“处理方式”的ComboBox选择为“使用SVN表”
                if (isChangedBySvnRevision == true)
                {
                    dataGridView.Rows[index].Cells[PART_DIFF_INFO_IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME].Style.BackColor = Color.LightGray;
                    dataGridView.Rows[index].Cells[PART_DIFF_INFO_RESOLVE_CONFLICT_WAY_COLUMN_NAME].Value = AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[1];
                }
            }

            // 本地表新增Key信息（本地表含有但最新SVN表中没有某个Key）
            const string PART_LOCAL_ADD_KEY_RESOLVE_CONFLICT_WAY_COLUMN_NAME = _PART_NAME_LOCAL_ADD_KEY + _RESOLVE_CONFLICT_WAY_COLUMN_NAME;
            const string PART_LOCAL_ADD_KEY_IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME = _PART_NAME_LOCAL_ADD_KEY + _IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME;
            int localAddKeyCount = compareResult.LocalAddKeyInfo.Count;
            for (int i = 0; i < localAddKeyCount; ++i)
            {
                CommitDifferentKeyInfo oneLocalAddKeyInfo = compareResult.LocalAddKeyInfo[i];
                List<object> showDataList = new List<object>();
                // 编号
                showDataList.Add(i + 1);
                // Key
                showDataList.Add(oneLocalAddKeyInfo.Key);
                // 本地表主语言译文
                showDataList.Add(oneLocalAddKeyInfo.DefaultLanguageValue);
                // 本地表行号
                showDataList.Add(oneLocalAddKeyInfo.ExcelLineNum);

                // 版本变动
                bool isChangedBySvnRevision = false;
                if (isSameRevision == false)
                {
                    // 判断此差异是否为SVN中两版本本身存在的差异，如果与本地表、同版本的SVN表还存在此Key，但最新版本SVN表已经没有了，说明是因为SVN版本变动导致此Key被删除，而非用户有意新增Key
                    if (_svnSameRevisionExcelInfo.Keys.Contains(oneLocalAddKeyInfo.Key))
                        isChangedBySvnRevision = true;

                    showDataList.Add(isChangedBySvnRevision == true ? "是" : string.Empty);
                }

                DataGridView dataGridView = _partControls[_PART_NAME_LOCAL_ADD_KEY].DataGridView;
                int index = dataGridView.Rows.Add(showDataList.ToArray());
                // 如果差异来自SVN版本的变动，则“版本变动”列的单元格背景色变为浅灰色，并默认将“处理方式”的ComboBox选择为“使用SVN表”
                if (isChangedBySvnRevision == true)
                {
                    dataGridView.Rows[index].Cells[PART_LOCAL_ADD_KEY_IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME].Style.BackColor = Color.LightGray;
                    dataGridView.Rows[index].Cells[PART_LOCAL_ADD_KEY_RESOLVE_CONFLICT_WAY_COLUMN_NAME].Value = AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[1];
                }
            }

            // SVN表新增Key信息（最新SVN表含有但本地表中没有某个Key）
            const string PART_SVN_ADD_KEY_RESOLVE_CONFLICT_WAY_COLUMN_NAME = _PART_NAME_SVN_ADD_KEY + _RESOLVE_CONFLICT_WAY_COLUMN_NAME;
            const string PART_SVN_ADD_KEY_IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME = _PART_NAME_SVN_ADD_KEY + _IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME;
            int svnAddKeyCount = compareResult.SvnAddKeyInfo.Count;
            for (int i = 0; i < svnAddKeyCount; ++i)
            {
                CommitDifferentKeyInfo oneSvnAddKeyInfo = compareResult.SvnAddKeyInfo[i];
                List<object> showDataList = new List<object>();
                // 编号
                showDataList.Add(i + 1);
                // Key
                showDataList.Add(oneSvnAddKeyInfo.Key);
                // SVN表主语言译文
                showDataList.Add(oneSvnAddKeyInfo.DefaultLanguageValue);
                // SVN表行号
                showDataList.Add(oneSvnAddKeyInfo.ExcelLineNum);

                // 版本变动
                bool isChangedBySvnRevision = true;
                if (isSameRevision == false)
                {
                    // 判断此差异是否为SVN中两版本本身存在的差异，如果与本地表、同版本的SVN表均没有此Key，但最新版本SVN表中有，说明是因为SVN版本变动导致此Key被新增，而非用户有意删除Key
                    if (_svnSameRevisionExcelInfo.Keys.Contains(oneSvnAddKeyInfo.Key))
                        isChangedBySvnRevision = false;

                    showDataList.Add(isChangedBySvnRevision == true ? "是" : string.Empty);
                }
                else
                    isChangedBySvnRevision = false;

                DataGridView dataGridView = _partControls[_PART_NAME_SVN_ADD_KEY].DataGridView;
                int index = dataGridView.Rows.Add(showDataList.ToArray());
                // 如果差异来自SVN版本的变动，则“版本变动”列的单元格背景色变为浅灰色，并默认将“处理方式”的ComboBox选择为“使用SVN表”
                if (isChangedBySvnRevision == true)
                {
                    dataGridView.Rows[index].Cells[PART_SVN_ADD_KEY_IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME].Style.BackColor = Color.LightGray;
                    dataGridView.Rows[index].Cells[PART_SVN_ADD_KEY_RESOLVE_CONFLICT_WAY_COLUMN_NAME].Value = AppValues.RESOLVE_COMMIT_DIFF_OPTINOS[1];
                }
            }
        }

        /// <summary>
        /// 控制各个DataGridView中“版本变动”列的显隐
        /// </summary>
        private void _ShowIsChangedBySvnRevisionColumn(bool isShow)
        {
            if (isShow == true)
            {
                // 如果不含“版本变动”列，需要添加
                foreach (_PartControls onePartControls in _partControls.Values)
                {
                    if (!onePartControls.DataGridView.Columns.Contains(onePartControls.PartName + _IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME))
                    {
                        DataGridViewTextBoxColumn column = new DataGridViewTextBoxColumn();
                        column.HeaderText = "版本变动";
                        column.Name = onePartControls.PartName + _IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME;
                        column.ReadOnly = true;

                        onePartControls.DataGridView.Columns.Insert(onePartControls.DataGridView.Columns.Count - 1, column);
                    }
                }
            }
            else
            {
                // 如果含有“版本变动”列，需要去掉
                foreach (_PartControls onePartControls in _partControls.Values)
                {
                    if (onePartControls.DataGridView.Columns.Contains(onePartControls.PartName + _IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME))
                        onePartControls.DataGridView.Columns.Remove(onePartControls.PartName + _IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME);
                }
            }

            // 如果显示“版本变动”列，需给批量修改处理方式提供忽略“版本变动”为“是”的条目的选项
            foreach (_PartControls onePartControls in _partControls.Values)
                onePartControls.CheckBox.Visible = isShow;

            // 控制“版本变动”列说明Tips的显隐
            lblChangedBySvnRevisionTips.Visible = isShow;
        }

        /// <summary>
        /// 清空所有DataGridView中的内容
        /// </summary>
        private void _CleanDataGridView()
        {
            foreach (_PartControls onePartControls in _partControls.Values)
                onePartControls.DataGridView.Rows.Clear();
        }

        // 当更改了ComboBox的统一处理方式时触发
        private void _OnChangedUnifiedResolveConflictWay(object sender, EventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            _PartControls partControl = _GetPartControlsByOneControl(comboBox);
            DataGridView dataGridView = partControl.DataGridView;
            object selectedValue = comboBox.SelectedItem;
            bool isIgnoreChangeBySvnRevision = (_compareResult.LocalFileRevision != _compareResult.SvnFileRevision && partControl.CheckBox.Checked == true);
            int rowCount = dataGridView.Rows.Count;
            string PART_RESOLVE_CONFLICT_WAY_COLUMN_NAME = partControl.PartName + _RESOLVE_CONFLICT_WAY_COLUMN_NAME;
            string PART_IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME = partControl.PartName + _IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME;
            for (int i = 0; i < rowCount; ++i)
            {
                if (isIgnoreChangeBySvnRevision == true && !string.IsNullOrEmpty(dataGridView.Rows[i].Cells[PART_IS_CHANGED_BY_SVN_REVISION_COLUMN_NAME].Value as string))
                    continue;

                DataGridViewComboBoxCell comboBoxCell = dataGridView.Rows[i].Cells[PART_RESOLVE_CONFLICT_WAY_COLUMN_NAME] as DataGridViewComboBoxCell;
                comboBoxCell.Value = selectedValue;
            }
        }

        // 当双击DataGridView中的单元格时触发
        private void _OnCellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView dataGridView = sender as DataGridView;
            DataGridViewColumn column = dataGridView.Columns[e.ColumnIndex];
            if (column.GetType() == typeof(DataGridViewTextBoxColumn))
            {
                // 如果双击的是“本地表行号”列下的单元格
                if (column.Name.EndsWith(_LOCAL_LINE_NUM_COLUMN_NAME))
                {
                    int lineNum = int.Parse(dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    // 因为选中Excel文件中的对应行并在打开时滚动到此行会涉及到Excel文件的修改和保存，为了不对本地表做出改变，需要另建副本后对副本进行操作
                    string localFileCopyPath = Utils.CombinePath(AppValues.PROGRAM_FOLDER_PATH, string.Format("用于查看定位的本地表副本 {0:yyyy年MM月dd日 HH时mm分ss秒} 对应SVN版本号{1}.xlsx", DateTime.Now, _localExcelInfo.Revision));
                    try
                    {
                        File.Copy(AppValues.LocalExcelFilePath, localFileCopyPath, true);
                    }
                    catch (Exception exception)
                    {
                        MessageBox.Show(string.Format("建立本地表副本失败，错误原因为：{0}\n\n因为选中Excel文件中的对应行并在打开时滚动到此行会涉及到Excel文件的修改和保存，为了不对本地表做出改变，需要另建副本后对副本进行操作。但建立副本失败，该操作被迫中止", exception.ToString()), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    // 操作Excel文件，对Excel文件中的对应行进行选中并在打开时滚动到此行
                    string errorString = null;
                    CommitExcelFileHelper.SetExcelVisibleRow(localFileCopyPath, lineNum, out errorString);
                    if (string.IsNullOrEmpty(errorString))
                        System.Diagnostics.Process.Start(localFileCopyPath);
                    else
                        MessageBox.Show(string.Format("选中Excel文件中的对应行并在打开时滚动到此行的设置失败，错误原因为：{0}\n\n该操作被迫中止", errorString), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                // 如果双击的是“SVN表行号”列下的单元格
                else if (column.Name.EndsWith(_SVN_LINE_NUM_COLUMN_NAME))
                {
                    int lineNum = int.Parse(dataGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value.ToString());
                    // 操作Excel文件，对Excel文件中的对应行进行选中并在打开时滚动到此行
                    string errorString = null;
                    CommitExcelFileHelper.SetExcelVisibleRow(_newestSvnExcelInfo.FilePath, lineNum, out errorString);
                    if (string.IsNullOrEmpty(errorString))
                        System.Diagnostics.Process.Start(_newestSvnExcelInfo.FilePath);
                    else
                        MessageBox.Show(string.Format("选中Excel文件中的对应行并在打开时滚动到此行的设置失败，错误原因为：{0}\n\n该操作被迫中止", errorString), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                    MessageBox.Show("只有双击“本地表行号”或“SVN表行号”列中的单元格才会打开对应的Excel文件并定位到指定行号", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        // 通过某模块的控件找到对应的模块控件类
        private _PartControls _GetPartControlsByOneControl(Control control)
        {
            Type controlType = control.GetType();
            foreach (_PartControls onePartControls in _partControls.Values)
            {
                if (controlType == typeof(DataGridView) && onePartControls.DataGridView == control)
                    return onePartControls;
                else if (controlType == typeof(ComboBox) && onePartControls.ComboBox == control)
                    return onePartControls;
                else if (controlType == typeof(CheckBox) && onePartControls.CheckBox == control)
                    return onePartControls;
            }

            return null;
        }

        /// <summary>
        /// 该类用于记录3个DataGridView功能区所包含的相关控件
        /// </summary>
        private class _PartControls
        {
            // 模块的名称（分显示主语言译文不同的模块、本地表新增Key模块、SVN表新增Key模块）
            public string PartName { get; set; }
            // 展示差异项的DataGridView
            public DataGridView DataGridView { get; set; }
            // 选择批量处理方式的ComboBox
            public ComboBox ComboBox { get; set; }
            // 选择批量处理时是否忽略“版本变动”为“是”的差异项的CheckBox
            public CheckBox CheckBox { get; set; }
        }
    }
}
