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
    public partial class ChooseExportLangFileForm : Form
    {
        // 根据项目语种个数，动态生成控件外观的相关设置
        private const int _CHECKBOX_POSITION_X = 24;
        private const int _CHECKBOX_POSITION_START_Y = 92;
        private Size _CHECKBOX_SIZE = new Size(94, 24);

        private const int _TEXTBOX_POSITION_X = 124;
        private const int _TEXTBOX_POSITION_START_Y = 94;
        private Size _TEXTBOX_SIZE = new Size(318, 21);

        private const int _BUTTON_POSITION_X = 457;
        private const int _BUTTON_POSITION_START_Y = 92;
        private Size _BUTTON_SIZE = new Size(54, 23);
        // 相邻两行控件的垂直距离
        private const int _DISTANCE_Y = 30;
        // 未生成各语种控件前窗口的高度
        private const int _INIT_FORM_HEIGHT = 140;
        // 控件名称开头
        private const string _CHECKBOX_NAME_START_STRING = "chk_";
        private const string _TEXTBOX_NAME_START_STRING = "txt_";
        private const string _BUTTON_NAME_START_STRING = "btn_";

        private bool _isExportUnifiedDir;
        private List<LanguageInfo> _languageInfoList = null;

        public ChooseExportLangFileForm(bool isExportUnifiedDir)
        {
            InitializeComponent();

            _isExportUnifiedDir = isExportUnifiedDir;
            _languageInfoList = AppValues.LangExcelInfo.GetAllLanguageInfoList();
        }

        private void ChooseExportLangFileForm_Load(object sender, EventArgs e)
        {
            int languageCount = _languageInfoList.Count;
            // 根据语种个数，调整窗口高度
            this.Height = _INIT_FORM_HEIGHT + languageCount * _DISTANCE_Y;
            // 根据母表中语种信息，生成操作控件
            for (int i = 0; i < languageCount; ++i)
            {
                LanguageInfo info = _languageInfoList[i];
                // 复选框
                CheckBox chk = new CheckBox();
                chk.Name = string.Concat(_CHECKBOX_NAME_START_STRING, info.Name);
                chk.AutoSize = false;
                chk.Size = _CHECKBOX_SIZE;
                chk.Text = info.Name;
                chk.Location = new Point(_CHECKBOX_POSITION_X, _CHECKBOX_POSITION_START_Y + i * _DISTANCE_Y);
                this.Controls.Add(chk);
                // 路径输入文本框
                TextBox txt = new TextBox();
                txt.Name = string.Concat(_TEXTBOX_NAME_START_STRING, info.Name);
                txt.Size = _TEXTBOX_SIZE;
                txt.Location = new Point(_TEXTBOX_POSITION_X, _TEXTBOX_POSITION_START_Y + i * _DISTANCE_Y);
                this.Controls.Add(txt);
                // 选择路径按钮
                Button btnExport = new Button();
                btnExport.Name = string.Concat(_BUTTON_NAME_START_STRING, info.Name);
                btnExport.Size = _BUTTON_SIZE;
                btnExport.Text = "选择";
                btnExport.Location = new Point(_BUTTON_POSITION_X, _BUTTON_POSITION_START_Y + i * _DISTANCE_Y);
                btnExport.Click += new System.EventHandler(_HandleExportButtonClicked);
                this.Controls.Add(btnExport);
            }

            // 如果选择的是导出lang文件到统一路径，自动填写好导出路径以及文件名
            if (_isExportUnifiedDir == true)
            {
                foreach (LanguageInfo info in _languageInfoList)
                {
                    string textBoxName = string.Concat(_TEXTBOX_NAME_START_STRING, info.Name);
                    TextBox txt = this.Controls[textBoxName] as TextBox;
                    txt.Text = Path.Combine(AppValues.ExportLangFileUnifiedDir, string.Concat(info.Name, ".", AppValues.LangFileExtension));
                }
            }
        }

        // 修改“全选/全不选”复选框选中状态时触发
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chkSelectAll = sender as CheckBox;
            int languageCount = _languageInfoList.Count;
            foreach (LanguageInfo info in _languageInfoList)
            {
                string checkBoxName = string.Concat(_CHECKBOX_NAME_START_STRING, info.Name);
                CheckBox chk = this.Controls[checkBoxName] as CheckBox;
                chk.Checked = chkSelectAll.Checked;
            }
        }

        // 各语种“选择”按钮通用的点击事件响应函数
        private void _HandleExportButtonClicked(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string languageName = btn.Name.Substring(_BUTTON_NAME_START_STRING.Length);
            string textBoxName = string.Concat(_TEXTBOX_NAME_START_STRING, languageName);
            TextBox txt = this.Controls[textBoxName] as TextBox;
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.ValidateNames = true;
            dialog.Title = string.Format("请选择{0}语种对应的lang文件保存路径", languageName);
            dialog.Filter = string.Format("Lang files (*.{0})|*.{0}", AppValues.LangFileExtension);
            dialog.FileName = languageName;
            if (_isExportUnifiedDir == true)
                dialog.InitialDirectory = AppValues.ExportLangFileUnifiedDir;

            if (dialog.ShowDialog() == DialogResult.OK)
                txt.Text = dialog.FileName;
        }

        // 点击“导出”按钮
        private void btnExport_Click(object sender, EventArgs e)
        {
            // 用于记录写入log文件的内容
            StringBuilder logStringBuilder = new StringBuilder();
            // 每个语种是否导出成功（key：语种名称，value：是否导出成功）
            Dictionary<string, bool> exportResult = new Dictionary<string, bool>();
            // 记录导出失败的lang文件个数
            int failCount = 0;

            int languageCount = _languageInfoList.Count;
            foreach (LanguageInfo info in _languageInfoList)
            {
                string checkBoxName = string.Concat(_CHECKBOX_NAME_START_STRING, info.Name);
                CheckBox chk = this.Controls[checkBoxName] as CheckBox;
                if (chk.Checked == true)
                {
                    string textBoxName = string.Concat(_TEXTBOX_NAME_START_STRING, info.Name);
                    TextBox txt = this.Controls[textBoxName] as TextBox;
                    string savePath = txt.Text.Trim();
                    logStringBuilder.AppendFormat("导出{0}语种对应的lang文件：", info.Name).AppendLine();
                    logStringBuilder.AppendFormat("导出路径：{0}", savePath).AppendLine();

                    string errorString = null;
                    if (ExportLangFileHelper.ExportLangFile(info.Name, savePath, out errorString) == true)
                    {
                        logStringBuilder.AppendLine("成功");
                        exportResult.Add(info.Name, true);
                    }
                    else
                    {
                        logStringBuilder.AppendLine("导出失败，原因为：");
                        logStringBuilder.AppendLine(errorString);
                        exportResult.Add(info.Name, false);
                        ++failCount;
                    }
                    logStringBuilder.AppendLine("----------------------------------------------");
                }
            }
            if (exportResult.Count < 1)
            {
                MessageBox.Show("至少需要勾选一个要导出lang文件的语种", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            logStringBuilder.AppendLine().AppendLine().AppendFormat("导出完毕，{0}个成功，{1}个失败", exportResult.Count - failCount, failCount);
            if (failCount == 0)
            {
                List<string> exportLanguageNames = new List<string>();
                foreach (string languageName in exportResult.Keys)
                    exportLanguageNames.Add(languageName);

                MessageBox.Show(string.Concat("导出勾选的以下语种的lang文件成功：\n", Utils.CombineString<string>(exportLanguageNames, ",")), "恭喜", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                // 在本工具所在路径下生成便于查错的log文件
                string fileName = string.Format("导出lang文件结果 {0:yyyy年MM月dd日 HH时mm分ss秒}.txt", DateTime.Now);
                string savePath = Utils.CombinePath(AppValues.PROGRAM_FOLDER_PATH, fileName);
                string errorString = null;
                string logString = logStringBuilder.ToString();
                if (Utils.SaveFile(savePath, logString, out errorString) == true)
                {
                    DialogResult dialogResult = MessageBox.Show(string.Format("导出lang文件失败，详细错误信息请查看下面的日志：\n{0}\n\n点击“确定”按钮后将自动打开此日志文件", savePath), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (dialogResult == DialogResult.OK)
                        System.Diagnostics.Process.Start(savePath);
                }
                else
                    MessageBox.Show(string.Format("导出lang文件失败，生成日志文件（{0}）失败，日志信息如下：\n\n{1}", savePath, logString), "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
