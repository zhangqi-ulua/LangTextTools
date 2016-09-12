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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // 刚打开软件后，只有选择并打开了Excel母表文件，子功能才可以使用
            _ChangeStateWhenSetExcelPath(false);
            // 默认隐藏母表工具部分，并且只有检查过Excel母表文件后，子功能才可以使用
            _ShowExcelFileTools(false);
            _ChangeStateWhenSetLocalExcelPath(false);
        }

        // 点击“选择Excel母表”按钮
        private void btnChooseExcelPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "请选择国际化Excel母表所在路径";
            dialog.Multiselect = false;
            dialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (dialog.ShowDialog() == DialogResult.OK)
                txtExcelPath.Text = dialog.FileName;
        }

        // 点击“打开Excel母表”按钮
        private void btnOpenExcelFile_Click(object sender, EventArgs e)
        {
            string excelFilePath = txtExcelPath.Text.Trim();
            if (string.IsNullOrEmpty(excelFilePath))
            {
                MessageBox.Show("请先输入或选择国际化Excel母表所在路径", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FileState fileState = Utils.GetFileState(excelFilePath);
            if (fileState == FileState.Inexist)
            {
                MessageBox.Show("输入的国际化Excel母表所在路径不存在，建议点击\"选择\"按钮进行文件选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!AppValues.EXCEL_FILE_EXTENSION.Equals(Path.GetExtension(excelFilePath), StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBox.Show(string.Format("本工具仅支持读取扩展名为{0}的Excel文件", AppValues.EXCEL_FILE_EXTENSION), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (fileState == FileState.IsOpen)
            {
                MessageBox.Show("该Excel文件正被其他软件打开，请关闭后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 读取设置的注释行开头字符
            _SetCommentLineStartChar();

            // 解析Excel母表
            string errorString = null;
            LangExcelInfo langExcelInfo = AnalyzeHelper.AnalyzeLangExcelFile(excelFilePath, AppValues.CommentLineStartChar, out errorString);
            if (errorString == null)
            {
                AppValues.ExcelFullPath = Path.GetFullPath(excelFilePath);
                // 当重新选择了Excel母表文件后重置窗口控件
                _ChangeStateWhenSetExcelPath(false);
                AppValues.LangExcelInfo = langExcelInfo;
                // 设置了合法的Excel母表后，可以使用各个子模块功能
                _ChangeStateWhenSetExcelPath(true);
            }
            else
            {
                MessageBox.Show(string.Format("选定的Excel母表存在以下错误，请修正后重试\n\n{0}", errorString), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        // 修改了是否导出到统一目录单选框后触发
        private void rdoExportUnifiedDir_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rdo = sender as RadioButton;
            _ChangeStateWhenChooseExportLangFileToUnifiedDir(rdo.Checked == true);
        }

        // 点击“选择lang文件统一导出路径”按钮
        private void btnChooseExportUnifiedDirPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "请选择lang文件统一导出路径";
            dialog.ShowNewFolderButton = false;
            if (dialog.ShowDialog() == DialogResult.OK)
                txtExportUnifiedDirPath.Text = dialog.SelectedPath;
        }

        // 点击“导出lang文件”按钮
        private void btnExportLangFile_Click(object sender, EventArgs e)
        {
            // 检查是否输入了Key与Value的分隔字符
            string keyAndValueSplitChar = txtKeyValueSplitChar.Text;
            if (string.IsNullOrEmpty(keyAndValueSplitChar))
            {
                MessageBox.Show("必须输入导出lang文件中Key、Value的分隔字符", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            AppValues.KeyAndValueSplitChar = keyAndValueSplitChar;
            // 检查是否输入了合法的导出lang文件的扩展名
            string langFileExtension = txtLangFileExtension.Text.Trim();
            if (string.IsNullOrEmpty(langFileExtension))
            {
                MessageBox.Show("必须输入导出lang文件的扩展名", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (".".Equals(langFileExtension) || Utils.CheckFilename(langFileExtension) == false)
            {
                MessageBox.Show("输入导出lang文件的扩展名非法", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (langFileExtension.StartsWith("."))
                AppValues.LangFileExtension = langFileExtension.Substring(1);
            else
                AppValues.LangFileExtension = langFileExtension;

            if (rdoExportUnifiedDir.Checked == true)
            {
                string unifiedDirPath = txtExportUnifiedDirPath.Text.Trim();
                if (string.IsNullOrEmpty(unifiedDirPath))
                {
                    MessageBox.Show("必须输入lang文件的统一导出路径", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!Directory.Exists(unifiedDirPath))
                {
                    MessageBox.Show("输入的lang文件统一导出路径不存在", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                AppValues.ExportLangFileUnifiedDir = unifiedDirPath;
                ChooseExportLangFileForm chooseForm = new ChooseExportLangFileForm(true);
                chooseForm.ShowDialog(this);
            }
            else
            {
                ChooseExportLangFileForm chooseForm = new ChooseExportLangFileForm(false);
                chooseForm.ShowDialog(this);
            }
        }

        // 点击“选择与最新母表对比的旧版母表”按钮
        private void btnChooseOldExcelPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "请选择与最新母表对比的旧版母表所在路径";
            dialog.Multiselect = false;
            dialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string oldExcelPath = dialog.FileName;
                txtOldExcelPath.Text = oldExcelPath;
            }
        }

        // 点击“打开旧版母表”按钮
        private void btnOpenOldExcelFile_Click(object sender, EventArgs e)
        {
            string excelFilePath = txtOldExcelPath.Text.Trim();
            if (string.IsNullOrEmpty(excelFilePath))
            {
                MessageBox.Show("请先输入或选择与新版母表对比的旧版国际化Excel母表所在路径", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FileState fileState = Utils.GetFileState(excelFilePath);
            if (fileState == FileState.Inexist)
            {
                MessageBox.Show("输入的旧版国际化Excel母表所在路径不存在，建议点击\"选择\"按钮进行文件选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!AppValues.EXCEL_FILE_EXTENSION.Equals(Path.GetExtension(excelFilePath), StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBox.Show(string.Format("本工具仅支持读取扩展名为{0}的Excel文件", AppValues.EXCEL_FILE_EXTENSION), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (fileState == FileState.IsOpen)
            {
                MessageBox.Show("该Excel文件正被其他软件打开，请关闭后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 检查选择的旧表和新表不能为同一个文件
            string oldExcelFileFullPath = Path.GetFullPath(excelFilePath);
            if (oldExcelFileFullPath.Equals(AppValues.ExcelFullPath))
            {
                MessageBox.Show("你选择的旧表和新表是同一个文件", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 解析旧版Excel母表
            string errorString = null;
            LangExcelInfo langExcelInfo = AnalyzeHelper.AnalyzeLangExcelFile(excelFilePath, AppValues.CommentLineStartChar, out errorString);
            if (errorString == null)
            {
                // 检查新旧母表的主语言名称是否相同
                string excelDefaultLanguageName = AppValues.LangExcelInfo.DefaultLanguageInfo.Name;
                string oldExcelDefaultLanguageName = langExcelInfo.DefaultLanguageInfo.Name;
                if (!excelDefaultLanguageName.Equals(oldExcelDefaultLanguageName))
                {
                    MessageBox.Show(string.Format("新旧母表主语言名称不同，无法进行对比，请统一后重试\n新版母表中主语言名称：{0}，旧版中主语言名称：{1}", excelDefaultLanguageName, oldExcelDefaultLanguageName), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                // 检查新旧母表的所有外语名称是否相同
                bool isMatchOtherLanguageInfo = true;
                if (AppValues.LangExcelInfo.OtherLanguageInfo.Count == langExcelInfo.OtherLanguageInfo.Count)
                {
                    foreach (var item in AppValues.LangExcelInfo.OtherLanguageInfo)
                    {
                        if (!langExcelInfo.OtherLanguageInfo.ContainsKey(item.Key))
                        {
                            isMatchOtherLanguageInfo = false;
                            break;
                        }
                    }
                }
                else
                    isMatchOtherLanguageInfo = false;

                if (isMatchOtherLanguageInfo == false)
                {
                    MessageBox.Show("新旧母表的所有外语种类个数或者名称不完全匹配，无法进行对比", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                AppValues.OldLangExcelInfo = langExcelInfo;
                // 设置了合法的旧版Excel母表后，可以使用对比功能下属的各个子模块功能
                _ChangeStateWhenSetOldExcelPath(true);
            }
            else
            {
                MessageBox.Show(string.Format("选定的旧版Excel母表存在以下错误，请修正后重试\n\n{0}", errorString), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        // 点击“仅导出需要翻译行的Excel文件”按钮
        private void btnGenerateNeedTranslateExcelFile_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.ValidateNames = true;
            dialog.Title = "请选择保存Excel文件的路径";
            dialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.FileName;
                // 检查要导出的Excel文件是否已存在且正被其他程序使用
                if (Utils.GetFileState(filePath) == FileState.IsOpen)
                {
                    MessageBox.Show("要覆盖的Excel文件正被其他程序打开，请关闭后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                txtNeedTranslateExcelPath.Text = filePath;
                string errorString = null;
                string promptMessage = null;
                if (ExportExcelFileHelper.ExportNeedTranslateExcelFile(filePath, out errorString, out promptMessage) == true)
                {
                    string text = string.Format("已将新增Key、翻译变动Key所在行信息导出至{0}", filePath);
                    if (promptMessage != null)
                        text = string.Concat(text, "\n\n", promptMessage);

                    if (MessageBox.Show(text, "恭喜", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                        System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(filePath));
                }
                else
                {
                    if (errorString != null)
                    {
                        errorString = string.Concat("导出新增Key、翻译变动Key所在行信息至新建Excel文件失败：", errorString);
                        MessageBox.Show(errorString, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (promptMessage != null)
                    {
                        if (MessageBox.Show(promptMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                            System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(filePath));
                    }
                }
            }
        }

        // 点击“修改对比Excel文件中新增Key行的背景色”按钮
        private void btnChangeColorForAdd_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                lblShowColorForAdd.BackColor = dialog.Color;
        }

        // 点击“修改对比Excel文件中主语言翻译变动行的背景色”按钮
        private void btnChangeColorForChange_Click(object sender, EventArgs e)
        {
            ColorDialog dialog = new ColorDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
                lblShowColorForChange.BackColor = dialog.Color;
        }

        // 点击“生成对比Excel文件”按钮
        private void btnGenarateComparedExcelFile_Click(object sender, EventArgs e)
        {
            // 如果选择的标注新增Key、主语言翻译变动所在行的背景色相同弹出警告对话框
            Color colorForAdd = lblShowColorForAdd.BackColor;
            Color colorForChange = lblShowColorForChange.BackColor;
            if (colorForAdd == colorForChange)
            {
                if (MessageBox.Show("选择的标注新增Key、主语言翻译变动所在行的背景色相同，不容易分辨，建议选择不同的背景色\n\n确实要采用相同的颜色吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            }
            // 如果输入的用于填充空单元格（新增Key行的外语译文单元格）的字符串为纯空格弹出警告对话框
            string fillNullCellText = txtFillNullCellText.Text;
            if (!string.IsNullOrEmpty(fillNullCellText) && string.IsNullOrEmpty(fillNullCellText.Trim()))
            {
                if (MessageBox.Show("输入的用于填充未翻译的语种单元格的字符串为纯空格，不容易分辨，建议选用特殊字符\n\n确实要采用目前输入的纯空格字符串吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                    return;
            }

            // 选择保存路径并新建Excel文件
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.ValidateNames = true;
            dialog.Title = "请选择保存Excel文件的路径";
            dialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.FileName;
                // 检查要导出的Excel文件是否已存在且正被其他程序使用
                if (Utils.GetFileState(filePath) == FileState.IsOpen)
                {
                    MessageBox.Show("要覆盖的Excel文件正被其他程序打开，请关闭后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                txtComparedExcelPath.Text = filePath;
                string errorString = null;
                string promptMessage = null;
                if (ExportExcelFileHelper.ExportComparedExcelFile(colorForAdd, colorForChange, fillNullCellText, filePath, out errorString, out promptMessage) == true)
                {
                    string text = string.Format("已将用指定背景色标注后的复制Excel母表导出至{0}", filePath);
                    if (promptMessage != null)
                        text = string.Concat(text, "\n\n", promptMessage);

                    if (MessageBox.Show(text, "恭喜", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                        System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(filePath));
                }
                else
                {
                    if (errorString != null)
                    {
                        errorString = string.Concat("导出用指定背景色标注后的复制Excel母表失败：", errorString);
                        MessageBox.Show(errorString, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else if (promptMessage != null)
                    {
                        if (MessageBox.Show(promptMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                            System.Diagnostics.Process.Start("explorer.exe", Path.GetDirectoryName(filePath));
                    }
                }
            }
        }

        // 点击“选择翻译完的Excel文件”按钮
        private void btnChooseTranslatedExcelPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "请选择与最新母表合并的翻译完的Excel表所在路径";
            dialog.Multiselect = false;
            dialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string translatedExcelPath = dialog.FileName;
                txtTranslatedExcelPath.Text = translatedExcelPath;
            }
        }

        // 点击“选择合并后的Excel文件保存路径”按钮
        private void btnGenerateMergedExcelPath_Click(object sender, EventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.ValidateNames = true;
            dialog.Title = "请选择保存Excel文件的路径";
            dialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filePath = dialog.FileName;
                txtMergedExcelPath.Text = filePath;
            }
        }

        // 点击“合并翻译完的Excel文件”按钮
        private void btnMergeTranslatedExcelFile_Click(object sender, EventArgs e)
        {
            // 检查是否指定了合法的合并后的Excel文件的保存路径
            string mergedExcelSavePath = txtMergedExcelPath.Text.Trim();
            if (string.IsNullOrEmpty(mergedExcelSavePath))
            {
                MessageBox.Show("必须输入合并后的Excel文件的保存路径", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!AppValues.EXCEL_FILE_EXTENSION.Equals(Path.GetExtension(mergedExcelSavePath), StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBox.Show(string.Format("合并后的Excel文件扩展名必须为{0}", AppValues.EXCEL_FILE_EXTENSION), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 检查要导出的后的Excel文件是否已存在且正被其他程序使用
            if (Utils.GetFileState(mergedExcelSavePath) == FileState.IsOpen)
            {
                MessageBox.Show("要覆盖的合并后的Excel文件正被其他程序打开，请关闭后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 检查是否指定了合法的翻译完的Excel文件
            string translatedExcelPath = txtTranslatedExcelPath.Text.Trim();
            if (string.IsNullOrEmpty(translatedExcelPath))
            {
                MessageBox.Show("必须输入翻译完的Excel文件所在路径", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FileState fileState = Utils.GetFileState(translatedExcelPath);
            if (fileState == FileState.Inexist)
            {
                MessageBox.Show("输入的翻译完的Excel文件所在路径不存在，建议点击\"选择\"按钮进行文件选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!AppValues.EXCEL_FILE_EXTENSION.Equals(Path.GetExtension(translatedExcelPath), StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBox.Show(string.Format("本工具仅支持读取扩展名为{0}的Excel文件", AppValues.EXCEL_FILE_EXTENSION), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (fileState == FileState.IsOpen)
            {
                MessageBox.Show("指定的翻译完的Excel文件正被其他软件打开，请关闭后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 检查选择的翻译完的Excel文件和新版母表不能为同一个文件
            string translatedExcelFileFullPath = Path.GetFullPath(translatedExcelPath);
            if (translatedExcelFileFullPath.Equals(AppValues.ExcelFullPath))
            {
                MessageBox.Show("你选择的翻译完的Excel文件和新版母表是同一个文件", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 解析翻译完的Excel文件
            string errorString = null;
            LangExcelInfo translatedExcelInfo = AnalyzeHelper.AnalyzeLangExcelFile(translatedExcelPath, AppValues.CommentLineStartChar, out errorString);
            if (errorString != null)
            {
                MessageBox.Show(string.Format("选定的翻译完后的Excel文件存在以下错误，请修正后重试\n\n{0}", errorString), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 检查翻译完的Excel文件与新版母表的主语言名称是否相同
            string excelDefaultLanguageName = AppValues.LangExcelInfo.DefaultLanguageInfo.Name;
            string translatedExcelDefaultLanguageName = translatedExcelInfo.DefaultLanguageInfo.Name;
            if (!excelDefaultLanguageName.Equals(translatedExcelDefaultLanguageName))
            {
                MessageBox.Show(string.Format("翻译完的Excel文件与新版母表的主语言名称不同，无法进行对比，请统一后重试\n新版母表中主语言名称：{0}，翻译完的Excel文件中主语言名称：{1}", excelDefaultLanguageName, translatedExcelDefaultLanguageName), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 记录翻译完的Excel文件中存在的外语名
            List<string> translatedExcelFileOtherLanguageNames = new List<string>();
            // 检查翻译完的Excel文件中的外语在新版母表中是否都存在
            List<string> inexistentOtherLanguageName = new List<string>();
            foreach (string otherLanguageName in translatedExcelInfo.OtherLanguageInfo.Keys)
            {
                if (!AppValues.LangExcelInfo.OtherLanguageInfo.ContainsKey(otherLanguageName))
                    inexistentOtherLanguageName.Add(otherLanguageName);

                translatedExcelFileOtherLanguageNames.Add(otherLanguageName);
            }
            if (inexistentOtherLanguageName.Count > 0)
            {
                MessageBox.Show(string.Format("翻译完后的Excel文件中存在以下新版母表中不存在的外语名，无法进行合并\n{0}", Utils.CombineString<string>(inexistentOtherLanguageName, ",")), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 检查完毕执行合并功能并生成合并结果报告文件（保存路径与合并后的Excel文件相同）
            string reportExcelSavePath = Utils.CombinePath(Path.GetDirectoryName(mergedExcelSavePath), string.Format("合并报告 {0:yyyy年MM月dd日 HH时mm分ss秒}.xlsx", DateTime.Now));
            ExportExcelFileHelper.ExportMergedExcelFile(mergedExcelSavePath, reportExcelSavePath, AppValues.LangExcelInfo, translatedExcelInfo, translatedExcelFileOtherLanguageNames, out errorString);
            if (errorString == null)
            {
                DialogResult dialogResult = MessageBox.Show(string.Format("合并操作成功\n合并后的Excel文件存储路径为{0}\n报告文件存储路径为{1}\n\n点击“确定”按钮后将自动打开此报告文件", mergedExcelSavePath, reportExcelSavePath), "恭喜", MessageBoxButtons.OK, MessageBoxIcon.Information);
                if (dialogResult == DialogResult.OK)
                    System.Diagnostics.Process.Start(reportExcelSavePath);
            }
            else
            {
                errorString = string.Concat("合并操作失败：", errorString);
                MessageBox.Show(errorString, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 更改选择Excel母表路径前后界面部分控件状态
        private void _ChangeStateWhenSetExcelPath(bool isSet)
        {
            if (isSet == true)
            {
                grpExportLangFile.Enabled = true;
                grpCompare.Enabled = true;
                _ChangeStateWhenSetOldExcelPath(false);
                _ChangeStateWhenChooseExportLangFileToUnifiedDir(true);
                grpMerger.Enabled = true;
                txtCommentLineStartChar.Enabled = false;
            }
            else
            {
                grpExportLangFile.Enabled = false;
                grpCompare.Enabled = false;
                grpMerger.Enabled = false;
                txtCommentLineStartChar.Enabled = true;
            }
        }

        // 更改选择导出lang文件是否在相同目录选项后界面部分控件状态
        private void _ChangeStateWhenChooseExportLangFileToUnifiedDir(bool isChoose)
        {
            if (isChoose == true)
            {
                txtExportUnifiedDirPath.Enabled = true;
                btnChooseExportUnifiedDirPath.Enabled = true;
            }
            else
            {
                txtExportUnifiedDirPath.Enabled = false;
                btnChooseExportUnifiedDirPath.Enabled = false;
            }
        }

        // 更改选择旧版Excel母表路径前后界面部分控件状态
        private void _ChangeStateWhenSetOldExcelPath(bool isSet)
        {
            if (isSet == true)
            {
                grpExportNeedTranslateExcelFile.Enabled = true;
                grpExportComparedExcelFile.Enabled = true;
            }
            else
            {
                grpExportNeedTranslateExcelFile.Enabled = false;
                grpExportComparedExcelFile.Enabled = false;
            }
        }

        /// <summary>
        /// 设置注释行开头字符
        /// </summary>
        public void _SetCommentLineStartChar()
        {
            string commentLineStartChar = txtCommentLineStartChar.Text.Trim();
            if (string.IsNullOrEmpty(commentLineStartChar))
                AppValues.CommentLineStartChar = null;
            else
                AppValues.CommentLineStartChar = commentLineStartChar;
        }

        /**
         * 母表工具部分
         */

        // 点击“显示隐藏母表工具”按钮
        private void btnShowExcelFileTools_Click(object sender, EventArgs e)
        {
            _ShowExcelFileTools(AppValues.IsShowExcelFileTools == false);
        }

        // 点击“选择本地表路径”按钮
        private void btnChooseLocalExcelPath_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Title = "请选择本机Working Copy中Excel母表路径";
            dialog.Multiselect = false;
            dialog.Filter = "Excel files (*.xlsx)|*.xlsx";
            if (dialog.ShowDialog() == DialogResult.OK)
                txtLocalExcelFilePath.Text = dialog.FileName;
        }

        // 点击“检查本地表”按钮
        private void btnCheckLocalExcelFilePath_Click(object sender, EventArgs e)
        {
            string localExcelFilePath = txtLocalExcelFilePath.Text.Trim();
            if (string.IsNullOrEmpty(localExcelFilePath))
            {
                MessageBox.Show("请先输入或选择本机Working Copy中Excel母表所在路径", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            FileState fileState = Utils.GetFileState(localExcelFilePath);
            if (fileState == FileState.Inexist)
            {
                MessageBox.Show("输入的本机Working Copy中Excel母表所在路径不存在，建议点击\"选择\"按钮进行文件选择", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!AppValues.EXCEL_FILE_EXTENSION.Equals(Path.GetExtension(localExcelFilePath), StringComparison.CurrentCultureIgnoreCase))
            {
                MessageBox.Show(string.Format("本工具仅支持读取扩展名为{0}的Excel文件", AppValues.EXCEL_FILE_EXTENSION), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 检查指定的本地表是否处于SVN管理下
            SvnException svnException = null;
            string fileFullPath = Path.GetFullPath(localExcelFilePath);
            SvnInfoEventArgs localFileInfo = OperateSvnHelper.GetLocalFileInfo(fileFullPath, out svnException);
            if (svnException == null)
            {
                // 判断该文件相较SVN中的状态
                SvnStatusEventArgs localFileState = OperateSvnHelper.GetLocalFileState(fileFullPath, out svnException);
                if (svnException == null)
                {
                    if (localFileState.LocalContentStatus == SvnStatus.Normal || localFileState.LocalContentStatus == SvnStatus.Modified)
                    {
                        _ChangeStateWhenSetLocalExcelPath(true);
                        txtCommentLineStartChar.Enabled = false;
                        // 读取设置的注释行开头字符
                        _SetCommentLineStartChar();
                        AppValues.LocalExcelFilePath = fileFullPath;
                        AppValues.SvnExcelFilePath = localFileInfo.Uri.ToString();
                    }
                    else
                    {
                        MessageBox.Show(string.Format("本地表状态为{0}，本工具仅支持Normal或Modified状态", localFileState.LocalContentStatus), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    MessageBox.Show(string.Concat("无法获取本地表状态，错误原因为：", svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                if (svnException is SvnInvalidNodeKindException)
                {
                    MessageBox.Show("输入的本机Working Copy中Excel母表所在路径不在SVN管理下", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                else
                {
                    MessageBox.Show(string.Concat("输入的本机Working Copy中Excel母表所在路径无效，错误原因为：", svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
        }

        // 点击“打开本地表”按钮
        private void btnOpenLocalExcelFile_Click(object sender, EventArgs e)
        {
            FileState fileState = Utils.GetFileState(AppValues.LocalExcelFilePath);
            if (fileState == FileState.Inexist)
            {
                MessageBox.Show("本地表已不存在，请勿在使用本工具时对母表进行操作", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            System.Diagnostics.Process.Start(AppValues.LocalExcelFilePath);
        }

        // 点击“获取本地表状态”按钮
        private void btnGetLocalExcelFileState_Click(object sender, EventArgs e)
        {
            FileState fileState = Utils.GetFileState(AppValues.LocalExcelFilePath);
            if (fileState == FileState.Inexist)
            {
                MessageBox.Show("本地表已不存在，请勿在使用本工具时对母表进行操作", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            SvnException svnException = null;
            SvnStatusEventArgs localFileState = OperateSvnHelper.GetLocalFileState(AppValues.LocalExcelFilePath, out svnException);
            if (svnException == null)
            {
                SvnInfoEventArgs svnFileInfo = OperateSvnHelper.GetSvnFileInfo(AppValues.SvnExcelFilePath, out svnException);
                if (svnException == null)
                {
                    // 本地表的版本号
                    long localFileRevision = localFileState.LastChangeRevision;
                    // 本地表文件相较SVN中的状态
                    SvnStatus svnStatus = localFileState.LocalContentStatus;

                    // SVN中的母表的版本号
                    long svnFileRevision = svnFileInfo.LastChangeRevision;
                    // 最后修改时间
                    DateTime svnFileChangeTime = svnFileInfo.LastChangeTime;
                    // 最后修改者
                    string svnFileChangeAuthor = svnFileInfo.LastChangeAuthor;

                    StringBuilder infoStringBuilder = new StringBuilder();
                    infoStringBuilder.Append("本地路径：").AppendLine(AppValues.LocalExcelFilePath);
                    infoStringBuilder.Append("SVN路径：").AppendLine(AppValues.SvnExcelFilePath);
                    infoStringBuilder.Append("本地版本号：").AppendLine(localFileRevision.ToString());
                    infoStringBuilder.Append("SVN版本号：").AppendLine(svnFileRevision.ToString());
                    infoStringBuilder.Append("SVN版本最后修改时间：").AppendLine(svnFileChangeTime.ToLocalTime().ToString());
                    infoStringBuilder.Append("SVN版本最后修改者：").AppendLine(svnFileChangeAuthor);
                    infoStringBuilder.Append("本地文件是否被打开：").AppendLine(fileState == FileState.IsOpen ? "是" : "否");
                    if (svnFileInfo.Lock != null)
                    {
                        infoStringBuilder.AppendLine("SVN中此文件是否被锁定：是");
                        infoStringBuilder.Append("锁定者：").AppendLine(svnFileInfo.Lock.Owner);
                        infoStringBuilder.Append("锁定时间：").AppendLine(svnFileInfo.Lock.CreationTime.ToLocalTime().ToString());
                        infoStringBuilder.Append("锁定原因：").AppendLine(svnFileInfo.Lock.Comment);
                    }
                    infoStringBuilder.Append("本地文件相较SVN中的状态：").Append(svnStatus.ToString()).Append("(").Append(OperateSvnHelper.GetSvnStatusDescription(svnStatus)).AppendLine(")");
                    infoStringBuilder.Append("本地文件是否是SVN中最新版本且本地内容未作修改：").AppendLine(localFileRevision == svnFileRevision && svnStatus == SvnStatus.Normal ? "是" : "否");

                    MessageBox.Show(infoStringBuilder.ToString(), "本地表状态信息", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show(string.Concat("无法获取SVN中母表信息，错误原因为：", svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show(string.Concat("无法获取本地表状态，错误原因为：", svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        // 点击“Revert并Update本地表”按钮
        private void btnRevertAndUpdateLocalExcelFile_Click(object sender, EventArgs e)
        {
            FileState fileState = Utils.GetFileState(AppValues.LocalExcelFilePath);
            if (fileState == FileState.Inexist)
            {
                MessageBox.Show("本地表已不存在，请勿在使用本工具时对母表进行操作", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (fileState == FileState.IsOpen)
            {
                MessageBox.Show("本地表正被其他软件使用，请关闭后再重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // 判断本地文件是否相较SVN中发生变动，若有变动提示进行备份后执行Revert
            SvnException svnException = null;
            SvnStatusEventArgs localFileState = OperateSvnHelper.GetLocalFileState(AppValues.LocalExcelFilePath, out svnException);
            if (svnException == null)
            {
                if (localFileState.LocalContentStatus == SvnStatus.Modified)
                {
                    DialogResult dialogResult = MessageBox.Show("检测到本地文件相较于线上已发生变动，是否要进行备份？\n\n点击“是”选择存放本地表备份路径后进行备份\n点击“否”不进行备份，直接Revert后Update", "选择是否对本地表进行备份", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        SaveFileDialog dialog = new SaveFileDialog();
                        dialog.ValidateNames = true;
                        dialog.Title = "请选择本地表备份路径";
                        dialog.Filter = "Excel files (*.xlsx)|*.xlsx";
                        dialog.FileName = string.Format("Revert前本地母表备份 {0:yyyy年MM月dd日 HH时mm分ss秒} 对应SVN版本号{1}.xlsx", DateTime.Now, localFileState.LastChangeRevision);
                        if (dialog.ShowDialog() == DialogResult.OK)
                        {
                            string backupPath = dialog.FileName;
                            // 检查要覆盖备份的Excel文件是否已存在且正被其他程序使用
                            if (Utils.GetFileState(backupPath) == FileState.IsOpen)
                            {
                                MessageBox.Show("要覆盖的Excel文件正被其他程序打开，请关闭后重试\n\nRevert并Update功能被迫中止", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }

                            try
                            {
                                File.Copy(AppValues.LocalExcelFilePath, backupPath, true);
                                MessageBox.Show("备份本地母表成功，点击“确定”后开始执行Revert并Update操作", "恭喜", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                            catch (Exception exception)
                            {
                                string errorString = string.Format("备份本地母表（{0}）至指定路径（{1}）失败：{2}\n\nRevert并Update功能被迫中止", AppValues.LocalExcelFilePath, backupPath, exception.Message);
                                MessageBox.Show(errorString, "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                        else
                        {
                            MessageBox.Show("未选择备份路径，无法进行本地母表备份\n\nRevert并Update功能被迫中止", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }

                    bool result = OperateSvnHelper.Revert(AppValues.LocalExcelFilePath, out svnException);
                    if (svnException == null)
                    {
                        if (result == false)
                        {
                            MessageBox.Show("Revert失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                    else
                    {
                        MessageBox.Show(string.Concat("Revert失败，错误原因为：", svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else if (localFileState.LocalContentStatus != SvnStatus.Normal)
                {
                    MessageBox.Show(string.Format("本地表状态为{0}，本工具仅支持Normal或Modified状态", localFileState.LocalContentStatus), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 判断是否需要进行Update
                SvnInfoEventArgs svnFileInfo = OperateSvnHelper.GetSvnFileInfo(AppValues.SvnExcelFilePath, out svnException);
                if (svnException == null)
                {
                    // 本地表的版本号
                    long localFileRevision = localFileState.LastChangeRevision;
                    // SVN中的母表的版本号
                    long svnFileRevision = svnFileInfo.LastChangeRevision;
                    if (localFileRevision == svnFileRevision)
                    {
                        if (localFileState.LocalContentStatus == SvnStatus.Modified)
                        {
                            MessageBox.Show("成功进行Revert操作，本地表已是SVN最新版本无需Update\n\nRevert并Update功能完成", "恭喜", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else
                        {
                            MessageBox.Show("本地表已是SVN最新版本且与SVN中内容一致，无需进行此操作", "恭喜", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                    }
                    else
                    {
                        bool result = OperateSvnHelper.Update(AppValues.LocalExcelFilePath, out svnException);
                        if (svnException == null)
                        {
                            if (result == false)
                            {
                                MessageBox.Show("Update失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                MessageBox.Show("执行Revert并Update功能成功", "恭喜", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                return;
                            }
                        }
                        else
                        {
                            if (svnException is SvnAuthorizationException || svnException is SvnOperationCanceledException)
                            {
                                MessageBox.Show("没有权限进行Update操作，请输入合法的SVN账户信息后重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            else
                            {
                                MessageBox.Show(string.Concat("Update失败，错误原因为：", svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                        }
                    }
                }
                else
                {
                    MessageBox.Show(string.Concat("无法获取SVN中母表信息，错误原因为：", svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show(string.Concat("无法获取本地表状态，错误原因为：", svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        // 点击“提交到SVN”按钮
        private void btnCommit_Click(object sender, EventArgs e)
        {
            FileState fileState = Utils.GetFileState(AppValues.LocalExcelFilePath);
            if (fileState == FileState.Inexist)
            {
                MessageBox.Show("本地表已不存在，请勿在使用本工具时对母表进行操作", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (fileState == FileState.IsOpen)
            {
                MessageBox.Show("本地表正被其他软件使用，请关闭后再重试", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // 判断本地表是否相较SVN最新版本发生变动，如果本地就是SVN中最新版本且未进行改动则无需提交
            SvnException svnException = null;
            SvnStatusEventArgs localFileState = OperateSvnHelper.GetLocalFileState(AppValues.LocalExcelFilePath, out svnException);
            if (svnException == null)
            {
                SvnInfoEventArgs svnFileInfo = OperateSvnHelper.GetSvnFileInfo(AppValues.SvnExcelFilePath, out svnException);
                if (svnException == null)
                {
                    // 本地表的版本号
                    long localFileRevision = localFileState.LastChangeRevision;
                    // SVN中的母表的版本号
                    long svnFileRevision = svnFileInfo.LastChangeRevision;
                    if (localFileState.LocalContentStatus == SvnStatus.Normal)
                    {
                        if (localFileRevision == svnFileRevision)
                        {
                            MessageBox.Show("本地母表与SVN最新版本完全一致，无需进行提交操作", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }
                        else if (MessageBox.Show("本地母表不是SVN最新版本，但与同版本SVN表完全一致\n\n确定要在此情况下进行提交操作吗？", "警告", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.No)
                            return;
                    }
                    // 本地表与SVN最新版本不同，则要下载一份SVN中最新版本与本地表进行对比，在用户手工选择需要提交哪些改动后将合并后的新表进行提交操作
                    string svnCopySavePath = Utils.CombinePath(AppValues.PROGRAM_FOLDER_PATH, string.Format("SVN最新母表副本 {0:yyyy年MM月dd日 HH时mm分ss秒} 对应SVN版本号{1}.xlsx", DateTime.Now, svnFileInfo.LastChangeRevision));
                    Exception exportException;
                    bool result = OperateSvnHelper.ExportSvnFileToLocal(AppValues.SvnExcelFilePath, svnCopySavePath, svnFileInfo.LastChangeRevision, out exportException);
                    if (exportException != null)
                    {
                        MessageBox.Show(string.Concat("下载SVN中最新母表存到本地失败，错误原因为：", svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (result == false)
                    {
                        MessageBox.Show("下载SVN中最新母表存到本地失败", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // 解析本地母表
                    string errorString = null;
                    CommitExcelInfo localExcelInfo = CommitExcelFileHelper.AnalyzeCommitExcelFile(AppValues.LocalExcelFilePath, AppValues.CommentLineStartChar, localFileRevision, out errorString);
                    if (errorString != null)
                    {
                        MessageBox.Show(string.Concat("本地母表存在以下错误，请修正后重试\n\n", errorString), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    // 解析下载到本地的SVN最新母表的副本
                    CommitExcelInfo svnExcelInfo = CommitExcelFileHelper.AnalyzeCommitExcelFile(svnCopySavePath, AppValues.CommentLineStartChar, svnFileRevision, out errorString);
                    if (errorString != null)
                    {
                        MessageBox.Show(string.Concat("SVN母表存在以下错误，请修正后重试\n\n", errorString), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    // 对比本地母表与SVN中的母表
                    CommitCompareResult compareResult = CommitExcelFileHelper.CompareCommitExcelFile(localExcelInfo, svnExcelInfo);
                    if (compareResult.IsHasDiff() == false)
                    {
                        MessageBox.Show("经对比发现本地母表与SVN中内容完全相同，无需进行提交操作\n请注意本功能仅会对比本地母表与SVN中母表的Key及主语言译文变动，不对各语种进行比较", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                    else
                    {
                        // 弹出对比结果界面，让用户选择需要合并到SVN中母表的变动
                        ResolveConflictWhenCommitForm resolveForm = new ResolveConflictWhenCommitForm(compareResult, localExcelInfo, svnExcelInfo);
                        resolveForm.ShowDialog(this);
                    }
                }
                else
                {
                    MessageBox.Show(string.Concat("无法获取SVN中母表信息，错误原因为：", svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                MessageBox.Show(string.Concat("无法获取本地表状态，错误原因为：", svnException.Message), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        // 更改母表工具部分的显隐时调整界面
        private void _ShowExcelFileTools(bool isShow)
        {
            if (isShow == true)
            {
                btnShowExcelFileTools.Text = "←\n\n隐\n\n藏\n\n母\n\n表\n\n工\n\n具";
                this.Size = new Size(1104, 712);
            }
            else
            {
                btnShowExcelFileTools.Text = "→\n\n显\n\n示\n\n母\n\n表\n\n工\n\n具";
                this.Size = new Size(610, 712);
            }

            AppValues.IsShowExcelFileTools = isShow;
        }

        // 更改选择本地Excel母表路径前后界面部分控件状态
        private void _ChangeStateWhenSetLocalExcelPath(bool isSet)
        {
            if (isSet == true)
            {
                txtLocalExcelFilePath.ReadOnly = true;
                grpOperateLocalExcelFile.Enabled = true;
            }
            else
            {
                txtLocalExcelFilePath.ReadOnly = false;
                grpOperateLocalExcelFile.Enabled = false;
            }
        }
    }
}
