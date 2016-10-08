namespace LangTextTools
{
    partial class MainForm
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.lblExcelPath = new System.Windows.Forms.Label();
            this.txtExcelPath = new System.Windows.Forms.TextBox();
            this.btnChooseExcelPath = new System.Windows.Forms.Button();
            this.grpExportLangFile = new System.Windows.Forms.GroupBox();
            this.btnChooseExportUnifiedDirPath = new System.Windows.Forms.Button();
            this.txtExportUnifiedDirPath = new System.Windows.Forms.TextBox();
            this.lblExportUnifiedDirPath = new System.Windows.Forms.Label();
            this.txtLangFileExtension = new System.Windows.Forms.TextBox();
            this.lblLangFileExtension = new System.Windows.Forms.Label();
            this.txtKeyValueSplitChar = new System.Windows.Forms.TextBox();
            this.lblKeyValueSplitChar = new System.Windows.Forms.Label();
            this.btnExportLangFile = new System.Windows.Forms.Button();
            this.rdoExportDifferentDir = new System.Windows.Forms.RadioButton();
            this.rdoExportUnifiedDir = new System.Windows.Forms.RadioButton();
            this.grpCompare = new System.Windows.Forms.GroupBox();
            this.btnOpenOldExcelFile = new System.Windows.Forms.Button();
            this.grpExportComparedExcelFile = new System.Windows.Forms.GroupBox();
            this.btnGenarateComparedExcelFile = new System.Windows.Forms.Button();
            this.txtComparedExcelPath = new System.Windows.Forms.TextBox();
            this.lblComparedExcelPath = new System.Windows.Forms.Label();
            this.txtFillNullCellText = new System.Windows.Forms.TextBox();
            this.lblFillNullCellText = new System.Windows.Forms.Label();
            this.btnChangeColorForChange = new System.Windows.Forms.Button();
            this.lblColorForChange = new System.Windows.Forms.Label();
            this.lblShowColorForChange = new System.Windows.Forms.Label();
            this.btnChangeColorForAdd = new System.Windows.Forms.Button();
            this.lblColorForAdd = new System.Windows.Forms.Label();
            this.lblShowColorForAdd = new System.Windows.Forms.Label();
            this.grpExportNeedTranslateExcelFile = new System.Windows.Forms.GroupBox();
            this.btnGenerateNeedTranslateExcelFile = new System.Windows.Forms.Button();
            this.lblNeedTranslateExcelPath = new System.Windows.Forms.Label();
            this.txtNeedTranslateExcelPath = new System.Windows.Forms.TextBox();
            this.btnChooseOldExcelPath = new System.Windows.Forms.Button();
            this.txtOldExcelPath = new System.Windows.Forms.TextBox();
            this.lblOldExcelPath = new System.Windows.Forms.Label();
            this.lblCommentLineStartChar = new System.Windows.Forms.Label();
            this.txtCommentLineStartChar = new System.Windows.Forms.TextBox();
            this.grpMerger = new System.Windows.Forms.GroupBox();
            this.btnGenerateMergedExcelPath = new System.Windows.Forms.Button();
            this.txtMergedExcelPath = new System.Windows.Forms.TextBox();
            this.lblMergedExcelPath = new System.Windows.Forms.Label();
            this.btnMergeTranslatedExcelFile = new System.Windows.Forms.Button();
            this.btnChooseTranslatedExcelPath = new System.Windows.Forms.Button();
            this.txtTranslatedExcelPath = new System.Windows.Forms.TextBox();
            this.lblTranslatedExcelPath = new System.Windows.Forms.Label();
            this.btnOpenExcelFile = new System.Windows.Forms.Button();
            this.btnShowExcelFileTools = new System.Windows.Forms.Button();
            this.lblLocalExcelFilePath = new System.Windows.Forms.Label();
            this.txtLocalExcelFilePath = new System.Windows.Forms.TextBox();
            this.btnChooseLocalExcelPath = new System.Windows.Forms.Button();
            this.btnCheckLocalExcelFilePath = new System.Windows.Forms.Button();
            this.grpOperateLocalExcelFile = new System.Windows.Forms.GroupBox();
            this.btnCommit = new System.Windows.Forms.Button();
            this.btnRevertAndUpdateLocalExcelFile = new System.Windows.Forms.Button();
            this.btnGetLocalExcelFileState = new System.Windows.Forms.Button();
            this.btnOpenLocalExcelFile = new System.Windows.Forms.Button();
            this.grpExportLangFile.SuspendLayout();
            this.grpCompare.SuspendLayout();
            this.grpExportComparedExcelFile.SuspendLayout();
            this.grpExportNeedTranslateExcelFile.SuspendLayout();
            this.grpMerger.SuspendLayout();
            this.grpOperateLocalExcelFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblExcelPath
            // 
            this.lblExcelPath.AutoSize = true;
            this.lblExcelPath.Location = new System.Drawing.Point(24, 21);
            this.lblExcelPath.Name = "lblExcelPath";
            this.lblExcelPath.Size = new System.Drawing.Size(95, 12);
            this.lblExcelPath.TabIndex = 0;
            this.lblExcelPath.Text = "Excel母表路径：";
            // 
            // txtExcelPath
            // 
            this.txtExcelPath.Location = new System.Drawing.Point(125, 18);
            this.txtExcelPath.Name = "txtExcelPath";
            this.txtExcelPath.Size = new System.Drawing.Size(262, 21);
            this.txtExcelPath.TabIndex = 0;
            // 
            // btnChooseExcelPath
            // 
            this.btnChooseExcelPath.Location = new System.Drawing.Point(393, 16);
            this.btnChooseExcelPath.Name = "btnChooseExcelPath";
            this.btnChooseExcelPath.Size = new System.Drawing.Size(54, 23);
            this.btnChooseExcelPath.TabIndex = 1;
            this.btnChooseExcelPath.Text = "选择";
            this.btnChooseExcelPath.UseVisualStyleBackColor = true;
            this.btnChooseExcelPath.Click += new System.EventHandler(this.btnChooseExcelPath_Click);
            // 
            // grpExportLangFile
            // 
            this.grpExportLangFile.Controls.Add(this.btnChooseExportUnifiedDirPath);
            this.grpExportLangFile.Controls.Add(this.txtExportUnifiedDirPath);
            this.grpExportLangFile.Controls.Add(this.lblExportUnifiedDirPath);
            this.grpExportLangFile.Controls.Add(this.txtLangFileExtension);
            this.grpExportLangFile.Controls.Add(this.lblLangFileExtension);
            this.grpExportLangFile.Controls.Add(this.txtKeyValueSplitChar);
            this.grpExportLangFile.Controls.Add(this.lblKeyValueSplitChar);
            this.grpExportLangFile.Controls.Add(this.btnExportLangFile);
            this.grpExportLangFile.Controls.Add(this.rdoExportDifferentDir);
            this.grpExportLangFile.Controls.Add(this.rdoExportUnifiedDir);
            this.grpExportLangFile.Location = new System.Drawing.Point(26, 86);
            this.grpExportLangFile.Name = "grpExportLangFile";
            this.grpExportLangFile.Size = new System.Drawing.Size(500, 130);
            this.grpExportLangFile.TabIndex = 3;
            this.grpExportLangFile.TabStop = false;
            this.grpExportLangFile.Text = "导出lang文件";
            // 
            // btnChooseExportUnifiedDirPath
            // 
            this.btnChooseExportUnifiedDirPath.Location = new System.Drawing.Point(307, 71);
            this.btnChooseExportUnifiedDirPath.Name = "btnChooseExportUnifiedDirPath";
            this.btnChooseExportUnifiedDirPath.Size = new System.Drawing.Size(54, 23);
            this.btnChooseExportUnifiedDirPath.TabIndex = 11;
            this.btnChooseExportUnifiedDirPath.Text = "选择";
            this.btnChooseExportUnifiedDirPath.UseVisualStyleBackColor = true;
            this.btnChooseExportUnifiedDirPath.Click += new System.EventHandler(this.btnChooseExportUnifiedDirPath_Click);
            // 
            // txtExportUnifiedDirPath
            // 
            this.txtExportUnifiedDirPath.Location = new System.Drawing.Point(128, 73);
            this.txtExportUnifiedDirPath.Name = "txtExportUnifiedDirPath";
            this.txtExportUnifiedDirPath.Size = new System.Drawing.Size(173, 21);
            this.txtExportUnifiedDirPath.TabIndex = 8;
            // 
            // lblExportUnifiedDirPath
            // 
            this.lblExportUnifiedDirPath.AutoSize = true;
            this.lblExportUnifiedDirPath.Location = new System.Drawing.Point(57, 76);
            this.lblExportUnifiedDirPath.Name = "lblExportUnifiedDirPath";
            this.lblExportUnifiedDirPath.Size = new System.Drawing.Size(65, 12);
            this.lblExportUnifiedDirPath.TabIndex = 7;
            this.lblExportUnifiedDirPath.Text = "导出路径：";
            // 
            // txtLangFileExtension
            // 
            this.txtLangFileExtension.Location = new System.Drawing.Point(307, 21);
            this.txtLangFileExtension.Name = "txtLangFileExtension";
            this.txtLangFileExtension.Size = new System.Drawing.Size(31, 21);
            this.txtLangFileExtension.TabIndex = 6;
            this.txtLangFileExtension.Text = "txt";
            // 
            // lblLangFileExtension
            // 
            this.lblLangFileExtension.AutoSize = true;
            this.lblLangFileExtension.Location = new System.Drawing.Point(248, 24);
            this.lblLangFileExtension.Name = "lblLangFileExtension";
            this.lblLangFileExtension.Size = new System.Drawing.Size(53, 12);
            this.lblLangFileExtension.TabIndex = 5;
            this.lblLangFileExtension.Text = "扩展名：";
            // 
            // txtKeyValueSplitChar
            // 
            this.txtKeyValueSplitChar.Location = new System.Drawing.Point(159, 21);
            this.txtKeyValueSplitChar.MaxLength = 1;
            this.txtKeyValueSplitChar.Name = "txtKeyValueSplitChar";
            this.txtKeyValueSplitChar.Size = new System.Drawing.Size(52, 21);
            this.txtKeyValueSplitChar.TabIndex = 4;
            // 
            // lblKeyValueSplitChar
            // 
            this.lblKeyValueSplitChar.AutoSize = true;
            this.lblKeyValueSplitChar.Location = new System.Drawing.Point(16, 24);
            this.lblKeyValueSplitChar.Name = "lblKeyValueSplitChar";
            this.lblKeyValueSplitChar.Size = new System.Drawing.Size(137, 12);
            this.lblKeyValueSplitChar.TabIndex = 3;
            this.lblKeyValueSplitChar.Text = "Key、Value的分隔字符：";
            // 
            // btnExportLangFile
            // 
            this.btnExportLangFile.Location = new System.Drawing.Point(403, 21);
            this.btnExportLangFile.Name = "btnExportLangFile";
            this.btnExportLangFile.Size = new System.Drawing.Size(78, 46);
            this.btnExportLangFile.TabIndex = 6;
            this.btnExportLangFile.Text = "导出";
            this.btnExportLangFile.UseVisualStyleBackColor = true;
            this.btnExportLangFile.Click += new System.EventHandler(this.btnExportLangFile_Click);
            // 
            // rdoExportDifferentDir
            // 
            this.rdoExportDifferentDir.AutoSize = true;
            this.rdoExportDifferentDir.Location = new System.Drawing.Point(18, 101);
            this.rdoExportDifferentDir.Name = "rdoExportDifferentDir";
            this.rdoExportDifferentDir.Size = new System.Drawing.Size(323, 16);
            this.rdoExportDifferentDir.TabIndex = 2;
            this.rdoExportDifferentDir.Text = "导出到不同目录下，人为指定各语种lang文件的导出路径";
            this.rdoExportDifferentDir.UseVisualStyleBackColor = true;
            // 
            // rdoExportUnifiedDir
            // 
            this.rdoExportUnifiedDir.AutoSize = true;
            this.rdoExportUnifiedDir.Checked = true;
            this.rdoExportUnifiedDir.Location = new System.Drawing.Point(18, 52);
            this.rdoExportUnifiedDir.Name = "rdoExportUnifiedDir";
            this.rdoExportUnifiedDir.Size = new System.Drawing.Size(263, 16);
            this.rdoExportUnifiedDir.TabIndex = 1;
            this.rdoExportUnifiedDir.TabStop = true;
            this.rdoExportUnifiedDir.Text = "导出到统一目录下，以各语种名称作为文件名";
            this.rdoExportUnifiedDir.UseVisualStyleBackColor = true;
            this.rdoExportUnifiedDir.CheckedChanged += new System.EventHandler(this.rdoExportUnifiedDir_CheckedChanged);
            // 
            // grpCompare
            // 
            this.grpCompare.Controls.Add(this.btnOpenOldExcelFile);
            this.grpCompare.Controls.Add(this.grpExportComparedExcelFile);
            this.grpCompare.Controls.Add(this.grpExportNeedTranslateExcelFile);
            this.grpCompare.Controls.Add(this.btnChooseOldExcelPath);
            this.grpCompare.Controls.Add(this.txtOldExcelPath);
            this.grpCompare.Controls.Add(this.lblOldExcelPath);
            this.grpCompare.Location = new System.Drawing.Point(26, 232);
            this.grpCompare.Name = "grpCompare";
            this.grpCompare.Size = new System.Drawing.Size(500, 298);
            this.grpCompare.TabIndex = 4;
            this.grpCompare.TabStop = false;
            this.grpCompare.Text = "对比功能（与旧版母表对比，标记新增key、主语言翻译变动等）";
            // 
            // btnOpenOldExcelFile
            // 
            this.btnOpenOldExcelFile.Location = new System.Drawing.Point(427, 21);
            this.btnOpenOldExcelFile.Name = "btnOpenOldExcelFile";
            this.btnOpenOldExcelFile.Size = new System.Drawing.Size(54, 23);
            this.btnOpenOldExcelFile.TabIndex = 2;
            this.btnOpenOldExcelFile.Text = "打开";
            this.btnOpenOldExcelFile.UseVisualStyleBackColor = true;
            this.btnOpenOldExcelFile.Click += new System.EventHandler(this.btnOpenOldExcelFile_Click);
            // 
            // grpExportComparedExcelFile
            // 
            this.grpExportComparedExcelFile.Controls.Add(this.btnGenarateComparedExcelFile);
            this.grpExportComparedExcelFile.Controls.Add(this.txtComparedExcelPath);
            this.grpExportComparedExcelFile.Controls.Add(this.lblComparedExcelPath);
            this.grpExportComparedExcelFile.Controls.Add(this.txtFillNullCellText);
            this.grpExportComparedExcelFile.Controls.Add(this.lblFillNullCellText);
            this.grpExportComparedExcelFile.Controls.Add(this.btnChangeColorForChange);
            this.grpExportComparedExcelFile.Controls.Add(this.lblColorForChange);
            this.grpExportComparedExcelFile.Controls.Add(this.lblShowColorForChange);
            this.grpExportComparedExcelFile.Controls.Add(this.btnChangeColorForAdd);
            this.grpExportComparedExcelFile.Controls.Add(this.lblColorForAdd);
            this.grpExportComparedExcelFile.Controls.Add(this.lblShowColorForAdd);
            this.grpExportComparedExcelFile.Location = new System.Drawing.Point(11, 132);
            this.grpExportComparedExcelFile.Name = "grpExportComparedExcelFile";
            this.grpExportComparedExcelFile.Size = new System.Drawing.Size(480, 154);
            this.grpExportComparedExcelFile.TabIndex = 7;
            this.grpExportComparedExcelFile.TabStop = false;
            this.grpExportComparedExcelFile.Text = "复制最新母表，并用不同颜色标识新增Key、主语言编译变动等所在行";
            // 
            // btnGenarateComparedExcelFile
            // 
            this.btnGenarateComparedExcelFile.Location = new System.Drawing.Point(356, 116);
            this.btnGenarateComparedExcelFile.Name = "btnGenarateComparedExcelFile";
            this.btnGenarateComparedExcelFile.Size = new System.Drawing.Size(112, 23);
            this.btnGenarateComparedExcelFile.TabIndex = 5;
            this.btnGenarateComparedExcelFile.Text = "选择并生成";
            this.btnGenarateComparedExcelFile.UseVisualStyleBackColor = true;
            this.btnGenarateComparedExcelFile.Click += new System.EventHandler(this.btnGenarateComparedExcelFile_Click);
            // 
            // txtComparedExcelPath
            // 
            this.txtComparedExcelPath.Location = new System.Drawing.Point(88, 118);
            this.txtComparedExcelPath.Name = "txtComparedExcelPath";
            this.txtComparedExcelPath.Size = new System.Drawing.Size(262, 21);
            this.txtComparedExcelPath.TabIndex = 3;
            // 
            // lblComparedExcelPath
            // 
            this.lblComparedExcelPath.AutoSize = true;
            this.lblComparedExcelPath.Location = new System.Drawing.Point(17, 121);
            this.lblComparedExcelPath.Name = "lblComparedExcelPath";
            this.lblComparedExcelPath.Size = new System.Drawing.Size(65, 12);
            this.lblComparedExcelPath.TabIndex = 11;
            this.lblComparedExcelPath.Text = "保存路径：";
            // 
            // txtFillNullCellText
            // 
            this.txtFillNullCellText.Location = new System.Drawing.Point(348, 88);
            this.txtFillNullCellText.Name = "txtFillNullCellText";
            this.txtFillNullCellText.Size = new System.Drawing.Size(120, 21);
            this.txtFillNullCellText.TabIndex = 2;
            // 
            // lblFillNullCellText
            // 
            this.lblFillNullCellText.AutoSize = true;
            this.lblFillNullCellText.Location = new System.Drawing.Point(17, 91);
            this.lblFillNullCellText.Name = "lblFillNullCellText";
            this.lblFillNullCellText.Size = new System.Drawing.Size(329, 12);
            this.lblFillNullCellText.TabIndex = 9;
            this.lblFillNullCellText.Text = "以此字符串填充未翻译的语种单元格（无需此功能请留空）：";
            // 
            // btnChangeColorForChange
            // 
            this.btnChangeColorForChange.Location = new System.Drawing.Point(406, 56);
            this.btnChangeColorForChange.Name = "btnChangeColorForChange";
            this.btnChangeColorForChange.Size = new System.Drawing.Size(62, 23);
            this.btnChangeColorForChange.TabIndex = 1;
            this.btnChangeColorForChange.Text = "更改颜色";
            this.btnChangeColorForChange.UseVisualStyleBackColor = true;
            this.btnChangeColorForChange.Click += new System.EventHandler(this.btnChangeColorForChange_Click);
            // 
            // lblColorForChange
            // 
            this.lblColorForChange.AutoSize = true;
            this.lblColorForChange.Location = new System.Drawing.Point(36, 61);
            this.lblColorForChange.Name = "lblColorForChange";
            this.lblColorForChange.Size = new System.Drawing.Size(365, 12);
            this.lblColorForChange.TabIndex = 7;
            this.lblColorForChange.Text = "新表中主语言翻译变动的Key行（某Key的翻译在新表中与旧表不同）";
            // 
            // lblShowColorForChange
            // 
            this.lblShowColorForChange.AutoSize = true;
            this.lblShowColorForChange.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.lblShowColorForChange.Location = new System.Drawing.Point(17, 61);
            this.lblShowColorForChange.Name = "lblShowColorForChange";
            this.lblShowColorForChange.Size = new System.Drawing.Size(11, 12);
            this.lblShowColorForChange.TabIndex = 6;
            this.lblShowColorForChange.Text = " ";
            // 
            // btnChangeColorForAdd
            // 
            this.btnChangeColorForAdd.Location = new System.Drawing.Point(406, 26);
            this.btnChangeColorForAdd.Name = "btnChangeColorForAdd";
            this.btnChangeColorForAdd.Size = new System.Drawing.Size(62, 23);
            this.btnChangeColorForAdd.TabIndex = 0;
            this.btnChangeColorForAdd.Text = "更改颜色";
            this.btnChangeColorForAdd.UseVisualStyleBackColor = true;
            this.btnChangeColorForAdd.Click += new System.EventHandler(this.btnChangeColorForAdd_Click);
            // 
            // lblColorForAdd
            // 
            this.lblColorForAdd.AutoSize = true;
            this.lblColorForAdd.Location = new System.Drawing.Point(36, 31);
            this.lblColorForAdd.Name = "lblColorForAdd";
            this.lblColorForAdd.Size = new System.Drawing.Size(281, 12);
            this.lblColorForAdd.TabIndex = 4;
            this.lblColorForAdd.Text = "新表中新增的Key行（旧版母表无，新版新增的Key）";
            // 
            // lblShowColorForAdd
            // 
            this.lblShowColorForAdd.AutoSize = true;
            this.lblShowColorForAdd.BackColor = System.Drawing.Color.Lime;
            this.lblShowColorForAdd.Location = new System.Drawing.Point(17, 31);
            this.lblShowColorForAdd.Name = "lblShowColorForAdd";
            this.lblShowColorForAdd.Size = new System.Drawing.Size(11, 12);
            this.lblShowColorForAdd.TabIndex = 3;
            this.lblShowColorForAdd.Text = " ";
            // 
            // grpExportNeedTranslateExcelFile
            // 
            this.grpExportNeedTranslateExcelFile.Controls.Add(this.btnGenerateNeedTranslateExcelFile);
            this.grpExportNeedTranslateExcelFile.Controls.Add(this.lblNeedTranslateExcelPath);
            this.grpExportNeedTranslateExcelFile.Controls.Add(this.txtNeedTranslateExcelPath);
            this.grpExportNeedTranslateExcelFile.Location = new System.Drawing.Point(11, 54);
            this.grpExportNeedTranslateExcelFile.Name = "grpExportNeedTranslateExcelFile";
            this.grpExportNeedTranslateExcelFile.Size = new System.Drawing.Size(480, 61);
            this.grpExportNeedTranslateExcelFile.TabIndex = 6;
            this.grpExportNeedTranslateExcelFile.TabStop = false;
            this.grpExportNeedTranslateExcelFile.Text = "仅将新增Key、翻译变动Key所在行信息导出至新建Excel文件";
            // 
            // btnGenerateNeedTranslateExcelFile
            // 
            this.btnGenerateNeedTranslateExcelFile.Location = new System.Drawing.Point(356, 26);
            this.btnGenerateNeedTranslateExcelFile.Name = "btnGenerateNeedTranslateExcelFile";
            this.btnGenerateNeedTranslateExcelFile.Size = new System.Drawing.Size(114, 23);
            this.btnGenerateNeedTranslateExcelFile.TabIndex = 2;
            this.btnGenerateNeedTranslateExcelFile.Text = "选择并生成";
            this.btnGenerateNeedTranslateExcelFile.UseVisualStyleBackColor = true;
            this.btnGenerateNeedTranslateExcelFile.Click += new System.EventHandler(this.btnGenerateNeedTranslateExcelFile_Click);
            // 
            // lblNeedTranslateExcelPath
            // 
            this.lblNeedTranslateExcelPath.AutoSize = true;
            this.lblNeedTranslateExcelPath.Location = new System.Drawing.Point(17, 31);
            this.lblNeedTranslateExcelPath.Name = "lblNeedTranslateExcelPath";
            this.lblNeedTranslateExcelPath.Size = new System.Drawing.Size(65, 12);
            this.lblNeedTranslateExcelPath.TabIndex = 3;
            this.lblNeedTranslateExcelPath.Text = "保存路径：";
            // 
            // txtNeedTranslateExcelPath
            // 
            this.txtNeedTranslateExcelPath.Location = new System.Drawing.Point(88, 28);
            this.txtNeedTranslateExcelPath.Name = "txtNeedTranslateExcelPath";
            this.txtNeedTranslateExcelPath.Size = new System.Drawing.Size(262, 21);
            this.txtNeedTranslateExcelPath.TabIndex = 0;
            // 
            // btnChooseOldExcelPath
            // 
            this.btnChooseOldExcelPath.Location = new System.Drawing.Point(367, 21);
            this.btnChooseOldExcelPath.Name = "btnChooseOldExcelPath";
            this.btnChooseOldExcelPath.Size = new System.Drawing.Size(54, 23);
            this.btnChooseOldExcelPath.TabIndex = 1;
            this.btnChooseOldExcelPath.Text = "选择";
            this.btnChooseOldExcelPath.UseVisualStyleBackColor = true;
            this.btnChooseOldExcelPath.Click += new System.EventHandler(this.btnChooseOldExcelPath_Click);
            // 
            // txtOldExcelPath
            // 
            this.txtOldExcelPath.Location = new System.Drawing.Point(111, 23);
            this.txtOldExcelPath.Name = "txtOldExcelPath";
            this.txtOldExcelPath.Size = new System.Drawing.Size(250, 21);
            this.txtOldExcelPath.TabIndex = 0;
            // 
            // lblOldExcelPath
            // 
            this.lblOldExcelPath.AutoSize = true;
            this.lblOldExcelPath.Location = new System.Drawing.Point(16, 26);
            this.lblOldExcelPath.Name = "lblOldExcelPath";
            this.lblOldExcelPath.Size = new System.Drawing.Size(89, 12);
            this.lblOldExcelPath.TabIndex = 0;
            this.lblOldExcelPath.Text = "旧版母表路径：";
            // 
            // lblCommentLineStartChar
            // 
            this.lblCommentLineStartChar.AutoSize = true;
            this.lblCommentLineStartChar.Location = new System.Drawing.Point(24, 53);
            this.lblCommentLineStartChar.Name = "lblCommentLineStartChar";
            this.lblCommentLineStartChar.Size = new System.Drawing.Size(485, 12);
            this.lblCommentLineStartChar.TabIndex = 5;
            this.lblCommentLineStartChar.Text = "注释行开头字符（在Excel的key列，若所填值以该字符开头则认为该行为注释行并忽略）：";
            // 
            // txtCommentLineStartChar
            // 
            this.txtCommentLineStartChar.Location = new System.Drawing.Point(505, 50);
            this.txtCommentLineStartChar.MaxLength = 1;
            this.txtCommentLineStartChar.Name = "txtCommentLineStartChar";
            this.txtCommentLineStartChar.Size = new System.Drawing.Size(21, 21);
            this.txtCommentLineStartChar.TabIndex = 3;
            this.txtCommentLineStartChar.Text = "#";
            // 
            // grpMerger
            // 
            this.grpMerger.Controls.Add(this.btnGenerateMergedExcelPath);
            this.grpMerger.Controls.Add(this.txtMergedExcelPath);
            this.grpMerger.Controls.Add(this.lblMergedExcelPath);
            this.grpMerger.Controls.Add(this.btnMergeTranslatedExcelFile);
            this.grpMerger.Controls.Add(this.btnChooseTranslatedExcelPath);
            this.grpMerger.Controls.Add(this.txtTranslatedExcelPath);
            this.grpMerger.Controls.Add(this.lblTranslatedExcelPath);
            this.grpMerger.Location = new System.Drawing.Point(26, 547);
            this.grpMerger.Name = "grpMerger";
            this.grpMerger.Size = new System.Drawing.Size(500, 103);
            this.grpMerger.TabIndex = 7;
            this.grpMerger.TabStop = false;
            this.grpMerger.Text = "合并功能（复制最新母表，并将翻译完的Excel表中的外语与其合并）";
            // 
            // btnGenerateMergedExcelPath
            // 
            this.btnGenerateMergedExcelPath.Location = new System.Drawing.Point(307, 61);
            this.btnGenerateMergedExcelPath.Margin = new System.Windows.Forms.Padding(2);
            this.btnGenerateMergedExcelPath.Name = "btnGenerateMergedExcelPath";
            this.btnGenerateMergedExcelPath.Size = new System.Drawing.Size(54, 23);
            this.btnGenerateMergedExcelPath.TabIndex = 5;
            this.btnGenerateMergedExcelPath.Text = "选择";
            this.btnGenerateMergedExcelPath.UseVisualStyleBackColor = true;
            this.btnGenerateMergedExcelPath.Click += new System.EventHandler(this.btnGenerateMergedExcelPath_Click);
            // 
            // txtMergedExcelPath
            // 
            this.txtMergedExcelPath.Location = new System.Drawing.Point(111, 64);
            this.txtMergedExcelPath.Margin = new System.Windows.Forms.Padding(2);
            this.txtMergedExcelPath.Name = "txtMergedExcelPath";
            this.txtMergedExcelPath.Size = new System.Drawing.Size(191, 21);
            this.txtMergedExcelPath.TabIndex = 4;
            // 
            // lblMergedExcelPath
            // 
            this.lblMergedExcelPath.AutoSize = true;
            this.lblMergedExcelPath.Location = new System.Drawing.Point(16, 61);
            this.lblMergedExcelPath.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMergedExcelPath.Name = "lblMergedExcelPath";
            this.lblMergedExcelPath.Size = new System.Drawing.Size(89, 24);
            this.lblMergedExcelPath.TabIndex = 3;
            this.lblMergedExcelPath.Text = "合并后的Excel\r\n文件保存路径：";
            // 
            // btnMergeTranslatedExcelFile
            // 
            this.btnMergeTranslatedExcelFile.Location = new System.Drawing.Point(399, 21);
            this.btnMergeTranslatedExcelFile.Name = "btnMergeTranslatedExcelFile";
            this.btnMergeTranslatedExcelFile.Size = new System.Drawing.Size(78, 46);
            this.btnMergeTranslatedExcelFile.TabIndex = 2;
            this.btnMergeTranslatedExcelFile.Text = "合并";
            this.btnMergeTranslatedExcelFile.UseVisualStyleBackColor = true;
            this.btnMergeTranslatedExcelFile.Click += new System.EventHandler(this.btnMergeTranslatedExcelFile_Click);
            // 
            // btnChooseTranslatedExcelPath
            // 
            this.btnChooseTranslatedExcelPath.Location = new System.Drawing.Point(307, 22);
            this.btnChooseTranslatedExcelPath.Name = "btnChooseTranslatedExcelPath";
            this.btnChooseTranslatedExcelPath.Size = new System.Drawing.Size(54, 23);
            this.btnChooseTranslatedExcelPath.TabIndex = 1;
            this.btnChooseTranslatedExcelPath.Text = "选择";
            this.btnChooseTranslatedExcelPath.UseVisualStyleBackColor = true;
            this.btnChooseTranslatedExcelPath.Click += new System.EventHandler(this.btnChooseTranslatedExcelPath_Click);
            // 
            // txtTranslatedExcelPath
            // 
            this.txtTranslatedExcelPath.Location = new System.Drawing.Point(111, 24);
            this.txtTranslatedExcelPath.Name = "txtTranslatedExcelPath";
            this.txtTranslatedExcelPath.Size = new System.Drawing.Size(191, 21);
            this.txtTranslatedExcelPath.TabIndex = 0;
            // 
            // lblTranslatedExcelPath
            // 
            this.lblTranslatedExcelPath.AutoSize = true;
            this.lblTranslatedExcelPath.Location = new System.Drawing.Point(16, 27);
            this.lblTranslatedExcelPath.Name = "lblTranslatedExcelPath";
            this.lblTranslatedExcelPath.Size = new System.Drawing.Size(89, 12);
            this.lblTranslatedExcelPath.TabIndex = 0;
            this.lblTranslatedExcelPath.Text = "翻译完的表格：";
            // 
            // btnOpenExcelFile
            // 
            this.btnOpenExcelFile.Location = new System.Drawing.Point(451, 16);
            this.btnOpenExcelFile.Name = "btnOpenExcelFile";
            this.btnOpenExcelFile.Size = new System.Drawing.Size(54, 23);
            this.btnOpenExcelFile.TabIndex = 2;
            this.btnOpenExcelFile.Text = "打开";
            this.btnOpenExcelFile.UseVisualStyleBackColor = true;
            this.btnOpenExcelFile.Click += new System.EventHandler(this.btnOpenExcelFile_Click);
            // 
            // btnShowExcelFileTools
            // 
            this.btnShowExcelFileTools.Location = new System.Drawing.Point(543, 225);
            this.btnShowExcelFileTools.Name = "btnShowExcelFileTools";
            this.btnShowExcelFileTools.Size = new System.Drawing.Size(28, 196);
            this.btnShowExcelFileTools.TabIndex = 8;
            this.btnShowExcelFileTools.Text = "→\r\n\r\n显\r\n\r\n示\r\n\r\n母\r\n\r\n表\r\n\r\n工\r\n\r\n具";
            this.btnShowExcelFileTools.UseVisualStyleBackColor = true;
            this.btnShowExcelFileTools.Click += new System.EventHandler(this.btnShowExcelFileTools_Click);
            // 
            // lblLocalExcelFilePath
            // 
            this.lblLocalExcelFilePath.AutoSize = true;
            this.lblLocalExcelFilePath.Location = new System.Drawing.Point(604, 21);
            this.lblLocalExcelFilePath.Name = "lblLocalExcelFilePath";
            this.lblLocalExcelFilePath.Size = new System.Drawing.Size(263, 12);
            this.lblLocalExcelFilePath.TabIndex = 9;
            this.lblLocalExcelFilePath.Text = "SVN中母表文件对应本机中的Working Copy路径：";
            // 
            // txtLocalExcelFilePath
            // 
            this.txtLocalExcelFilePath.Location = new System.Drawing.Point(606, 50);
            this.txtLocalExcelFilePath.Name = "txtLocalExcelFilePath";
            this.txtLocalExcelFilePath.Size = new System.Drawing.Size(439, 21);
            this.txtLocalExcelFilePath.TabIndex = 10;
            // 
            // btnChooseLocalExcelPath
            // 
            this.btnChooseLocalExcelPath.Location = new System.Drawing.Point(931, 16);
            this.btnChooseLocalExcelPath.Name = "btnChooseLocalExcelPath";
            this.btnChooseLocalExcelPath.Size = new System.Drawing.Size(54, 23);
            this.btnChooseLocalExcelPath.TabIndex = 11;
            this.btnChooseLocalExcelPath.Text = "选择";
            this.btnChooseLocalExcelPath.UseVisualStyleBackColor = true;
            this.btnChooseLocalExcelPath.Click += new System.EventHandler(this.btnChooseLocalExcelPath_Click);
            // 
            // btnCheckLocalExcelFilePath
            // 
            this.btnCheckLocalExcelFilePath.Location = new System.Drawing.Point(991, 16);
            this.btnCheckLocalExcelFilePath.Name = "btnCheckLocalExcelFilePath";
            this.btnCheckLocalExcelFilePath.Size = new System.Drawing.Size(54, 23);
            this.btnCheckLocalExcelFilePath.TabIndex = 12;
            this.btnCheckLocalExcelFilePath.Text = "检查";
            this.btnCheckLocalExcelFilePath.UseVisualStyleBackColor = true;
            this.btnCheckLocalExcelFilePath.Click += new System.EventHandler(this.btnCheckLocalExcelFilePath_Click);
            // 
            // grpOperateLocalExcelFile
            // 
            this.grpOperateLocalExcelFile.Controls.Add(this.btnCommit);
            this.grpOperateLocalExcelFile.Controls.Add(this.btnRevertAndUpdateLocalExcelFile);
            this.grpOperateLocalExcelFile.Controls.Add(this.btnGetLocalExcelFileState);
            this.grpOperateLocalExcelFile.Controls.Add(this.btnOpenLocalExcelFile);
            this.grpOperateLocalExcelFile.Location = new System.Drawing.Point(606, 86);
            this.grpOperateLocalExcelFile.Name = "grpOperateLocalExcelFile";
            this.grpOperateLocalExcelFile.Size = new System.Drawing.Size(454, 103);
            this.grpOperateLocalExcelFile.TabIndex = 13;
            this.grpOperateLocalExcelFile.TabStop = false;
            this.grpOperateLocalExcelFile.Text = "操作本机Working Copy中的母表文件";
            // 
            // btnCommit
            // 
            this.btnCommit.Location = new System.Drawing.Point(21, 61);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(418, 23);
            this.btnCommit.TabIndex = 17;
            this.btnCommit.Text = "提交至SVN（仅处理Key变动以及主语言译文变动，不处理外语语种列）";
            this.btnCommit.UseVisualStyleBackColor = true;
            this.btnCommit.Click += new System.EventHandler(this.btnCommit_Click);
            // 
            // btnRevertAndUpdateLocalExcelFile
            // 
            this.btnRevertAndUpdateLocalExcelFile.Location = new System.Drawing.Point(293, 24);
            this.btnRevertAndUpdateLocalExcelFile.Name = "btnRevertAndUpdateLocalExcelFile";
            this.btnRevertAndUpdateLocalExcelFile.Size = new System.Drawing.Size(146, 23);
            this.btnRevertAndUpdateLocalExcelFile.TabIndex = 2;
            this.btnRevertAndUpdateLocalExcelFile.Text = "Revert并Update本地表";
            this.btnRevertAndUpdateLocalExcelFile.UseVisualStyleBackColor = true;
            this.btnRevertAndUpdateLocalExcelFile.Click += new System.EventHandler(this.btnRevertAndUpdateLocalExcelFile_Click);
            // 
            // btnGetLocalExcelFileState
            // 
            this.btnGetLocalExcelFileState.Location = new System.Drawing.Point(148, 24);
            this.btnGetLocalExcelFileState.Name = "btnGetLocalExcelFileState";
            this.btnGetLocalExcelFileState.Size = new System.Drawing.Size(113, 23);
            this.btnGetLocalExcelFileState.TabIndex = 1;
            this.btnGetLocalExcelFileState.Text = "获取本地表状态";
            this.btnGetLocalExcelFileState.UseVisualStyleBackColor = true;
            this.btnGetLocalExcelFileState.Click += new System.EventHandler(this.btnGetLocalExcelFileState_Click);
            // 
            // btnOpenLocalExcelFile
            // 
            this.btnOpenLocalExcelFile.Location = new System.Drawing.Point(21, 24);
            this.btnOpenLocalExcelFile.Name = "btnOpenLocalExcelFile";
            this.btnOpenLocalExcelFile.Size = new System.Drawing.Size(93, 23);
            this.btnOpenLocalExcelFile.TabIndex = 0;
            this.btnOpenLocalExcelFile.Text = "打开本地表";
            this.btnOpenLocalExcelFile.UseVisualStyleBackColor = true;
            this.btnOpenLocalExcelFile.Click += new System.EventHandler(this.btnOpenLocalExcelFile_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1084, 669);
            this.Controls.Add(this.grpOperateLocalExcelFile);
            this.Controls.Add(this.btnCheckLocalExcelFilePath);
            this.Controls.Add(this.btnChooseLocalExcelPath);
            this.Controls.Add(this.txtLocalExcelFilePath);
            this.Controls.Add(this.lblLocalExcelFilePath);
            this.Controls.Add(this.btnShowExcelFileTools);
            this.Controls.Add(this.btnOpenExcelFile);
            this.Controls.Add(this.grpMerger);
            this.Controls.Add(this.txtCommentLineStartChar);
            this.Controls.Add(this.lblCommentLineStartChar);
            this.Controls.Add(this.grpCompare);
            this.Controls.Add(this.grpExportLangFile);
            this.Controls.Add(this.btnChooseExcelPath);
            this.Controls.Add(this.txtExcelPath);
            this.Controls.Add(this.lblExcelPath);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "国际化文本工具 1.5   by 张齐";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.grpExportLangFile.ResumeLayout(false);
            this.grpExportLangFile.PerformLayout();
            this.grpCompare.ResumeLayout(false);
            this.grpCompare.PerformLayout();
            this.grpExportComparedExcelFile.ResumeLayout(false);
            this.grpExportComparedExcelFile.PerformLayout();
            this.grpExportNeedTranslateExcelFile.ResumeLayout(false);
            this.grpExportNeedTranslateExcelFile.PerformLayout();
            this.grpMerger.ResumeLayout(false);
            this.grpMerger.PerformLayout();
            this.grpOperateLocalExcelFile.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblExcelPath;
        private System.Windows.Forms.TextBox txtExcelPath;
        private System.Windows.Forms.Button btnChooseExcelPath;
        private System.Windows.Forms.GroupBox grpExportLangFile;
        private System.Windows.Forms.Button btnExportLangFile;
        private System.Windows.Forms.RadioButton rdoExportDifferentDir;
        private System.Windows.Forms.RadioButton rdoExportUnifiedDir;
        private System.Windows.Forms.GroupBox grpCompare;
        private System.Windows.Forms.Button btnChooseOldExcelPath;
        private System.Windows.Forms.TextBox txtOldExcelPath;
        private System.Windows.Forms.Label lblOldExcelPath;
        private System.Windows.Forms.Label lblCommentLineStartChar;
        private System.Windows.Forms.TextBox txtCommentLineStartChar;
        private System.Windows.Forms.Label lblNeedTranslateExcelPath;
        private System.Windows.Forms.TextBox txtNeedTranslateExcelPath;
        private System.Windows.Forms.GroupBox grpExportNeedTranslateExcelFile;
        private System.Windows.Forms.Button btnGenerateNeedTranslateExcelFile;
        private System.Windows.Forms.GroupBox grpExportComparedExcelFile;
        private System.Windows.Forms.Button btnChangeColorForChange;
        private System.Windows.Forms.Label lblColorForChange;
        private System.Windows.Forms.Label lblShowColorForChange;
        private System.Windows.Forms.Button btnChangeColorForAdd;
        private System.Windows.Forms.Label lblColorForAdd;
        private System.Windows.Forms.Label lblShowColorForAdd;
        private System.Windows.Forms.Label lblComparedExcelPath;
        private System.Windows.Forms.TextBox txtFillNullCellText;
        private System.Windows.Forms.Label lblFillNullCellText;
        private System.Windows.Forms.TextBox txtKeyValueSplitChar;
        private System.Windows.Forms.Label lblKeyValueSplitChar;
        private System.Windows.Forms.Button btnGenarateComparedExcelFile;
        private System.Windows.Forms.TextBox txtComparedExcelPath;
        private System.Windows.Forms.GroupBox grpMerger;
        private System.Windows.Forms.Button btnOpenOldExcelFile;
        private System.Windows.Forms.Button btnOpenExcelFile;
        private System.Windows.Forms.Button btnMergeTranslatedExcelFile;
        private System.Windows.Forms.Button btnChooseTranslatedExcelPath;
        private System.Windows.Forms.TextBox txtTranslatedExcelPath;
        private System.Windows.Forms.Label lblTranslatedExcelPath;
        private System.Windows.Forms.TextBox txtLangFileExtension;
        private System.Windows.Forms.Label lblLangFileExtension;
        private System.Windows.Forms.Button btnChooseExportUnifiedDirPath;
        private System.Windows.Forms.TextBox txtExportUnifiedDirPath;
        private System.Windows.Forms.Label lblExportUnifiedDirPath;
        private System.Windows.Forms.Button btnGenerateMergedExcelPath;
        private System.Windows.Forms.TextBox txtMergedExcelPath;
        private System.Windows.Forms.Label lblMergedExcelPath;
        private System.Windows.Forms.Button btnShowExcelFileTools;
        private System.Windows.Forms.Label lblLocalExcelFilePath;
        private System.Windows.Forms.TextBox txtLocalExcelFilePath;
        private System.Windows.Forms.Button btnChooseLocalExcelPath;
        private System.Windows.Forms.Button btnCheckLocalExcelFilePath;
        private System.Windows.Forms.GroupBox grpOperateLocalExcelFile;
        private System.Windows.Forms.Button btnRevertAndUpdateLocalExcelFile;
        private System.Windows.Forms.Button btnGetLocalExcelFileState;
        private System.Windows.Forms.Button btnOpenLocalExcelFile;
        private System.Windows.Forms.Button btnCommit;
    }
}

