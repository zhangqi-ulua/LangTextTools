namespace LangTextTools
{
    partial class ResolveConflictWhenCommitForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblLocalFileRevision = new System.Windows.Forms.Label();
            this.txtLocalFileRevision = new System.Windows.Forms.TextBox();
            this.lblSvnFileRevision = new System.Windows.Forms.Label();
            this.txtSvnFileRevision = new System.Windows.Forms.TextBox();
            this.btnCommit = new System.Windows.Forms.Button();
            this.lblDiffInfo = new System.Windows.Forms.Label();
            this.dgvDiffDefaultLanguageInfo = new System.Windows.Forms.DataGridView();
            this.DiffInfoColumnNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiffInfoColumnKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiffInfoColumnLocalDefaultLanguage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiffInfoColumnSvnDefaultLanguage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiffInfoColumnLocalLineNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiffInfoColumnSvnLineNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiffInfoColumnIsChangedBySvnRevision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DiffInfoColumnResolveConflictWay = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lblDiffInfoUnifiedResolveConflictWay = new System.Windows.Forms.Label();
            this.cmbDiffInfoUnifiedResolveConflictWay = new System.Windows.Forms.ComboBox();
            this.lblChangedBySvnRevisionTips = new System.Windows.Forms.Label();
            this.lblCommitLogMessage = new System.Windows.Forms.Label();
            this.txtCommitLogMessage = new System.Windows.Forms.TextBox();
            this.chkDiffInfoIgnoreSvnRevisionChange = new System.Windows.Forms.CheckBox();
            this.lblLocalAddKeyInfo = new System.Windows.Forms.Label();
            this.chkLocalAddKeyInfoIgnoreSvnRevisionChange = new System.Windows.Forms.CheckBox();
            this.lblLocalAddKeyInfoUnifiedResolveConflictWay = new System.Windows.Forms.Label();
            this.cmbLocalAddKeyInfoUnifiedResolveConflictWay = new System.Windows.Forms.ComboBox();
            this.dgvLocalAddKeyInfo = new System.Windows.Forms.DataGridView();
            this.LocalAddKeyColumnNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalAddKeyColumnKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalAddKeyColumnLocalDefaultLanguage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalAddKeyColumnLocalLineNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalAddKeyColumnIsChangedBySvnRevision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LocalAddKeyColumnResolveConflictWay = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.lblSvnAddKeyInfo = new System.Windows.Forms.Label();
            this.dgvSvnAddKeyInfo = new System.Windows.Forms.DataGridView();
            this.SvnAddKeyColumnNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SvnAddKeyColumnKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SvnAddKeyColumnSvnDefaultLanguage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SvnAddKeyColumnSvnLineNum = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SvnAddKeyColumnIsChangedBySvnRevision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.SvnAddKeyColumnResolveConflictWay = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.chkSvnAddKeyInfoIgnoreSvnRevisionChange = new System.Windows.Forms.CheckBox();
            this.lblSvnAddKeyInfoUnifiedResolveConflictWay = new System.Windows.Forms.Label();
            this.cmbSvnAddKeyInfoUnifiedResolveConflictWay = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiffDefaultLanguageInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocalAddKeyInfo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSvnAddKeyInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // lblLocalFileRevision
            // 
            this.lblLocalFileRevision.AutoSize = true;
            this.lblLocalFileRevision.Location = new System.Drawing.Point(25, 21);
            this.lblLocalFileRevision.Name = "lblLocalFileRevision";
            this.lblLocalFileRevision.Size = new System.Drawing.Size(89, 12);
            this.lblLocalFileRevision.TabIndex = 0;
            this.lblLocalFileRevision.Text = "本地表版本号：";
            // 
            // txtLocalFileRevision
            // 
            this.txtLocalFileRevision.Location = new System.Drawing.Point(120, 18);
            this.txtLocalFileRevision.Name = "txtLocalFileRevision";
            this.txtLocalFileRevision.ReadOnly = true;
            this.txtLocalFileRevision.Size = new System.Drawing.Size(100, 21);
            this.txtLocalFileRevision.TabIndex = 1;
            // 
            // lblSvnFileRevision
            // 
            this.lblSvnFileRevision.AutoSize = true;
            this.lblSvnFileRevision.Location = new System.Drawing.Point(25, 47);
            this.lblSvnFileRevision.Name = "lblSvnFileRevision";
            this.lblSvnFileRevision.Size = new System.Drawing.Size(83, 12);
            this.lblSvnFileRevision.TabIndex = 2;
            this.lblSvnFileRevision.Text = "SVN中版本号：";
            // 
            // txtSvnFileRevision
            // 
            this.txtSvnFileRevision.Location = new System.Drawing.Point(120, 44);
            this.txtSvnFileRevision.Name = "txtSvnFileRevision";
            this.txtSvnFileRevision.ReadOnly = true;
            this.txtSvnFileRevision.Size = new System.Drawing.Size(100, 21);
            this.txtSvnFileRevision.TabIndex = 3;
            // 
            // btnCommit
            // 
            this.btnCommit.Location = new System.Drawing.Point(595, 21);
            this.btnCommit.Name = "btnCommit";
            this.btnCommit.Size = new System.Drawing.Size(75, 38);
            this.btnCommit.TabIndex = 4;
            this.btnCommit.Text = "提交";
            this.btnCommit.UseVisualStyleBackColor = true;
            this.btnCommit.Click += new System.EventHandler(this.btnCommit_Click);
            // 
            // lblDiffInfo
            // 
            this.lblDiffInfo.AutoSize = true;
            this.lblDiffInfo.Location = new System.Drawing.Point(25, 98);
            this.lblDiffInfo.Name = "lblDiffInfo";
            this.lblDiffInfo.Size = new System.Drawing.Size(329, 12);
            this.lblDiffInfo.TabIndex = 5;
            this.lblDiffInfo.Text = "以下为本地表与SVN中相同Key对应的主语言译文不同的信息：";
            // 
            // dgvDiffDefaultLanguageInfo
            // 
            this.dgvDiffDefaultLanguageInfo.AllowUserToAddRows = false;
            this.dgvDiffDefaultLanguageInfo.AllowUserToDeleteRows = false;
            this.dgvDiffDefaultLanguageInfo.AllowUserToOrderColumns = true;
            this.dgvDiffDefaultLanguageInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDiffDefaultLanguageInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dgvDiffDefaultLanguageInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDiffDefaultLanguageInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DiffInfoColumnNum,
            this.DiffInfoColumnKey,
            this.DiffInfoColumnLocalDefaultLanguage,
            this.DiffInfoColumnSvnDefaultLanguage,
            this.DiffInfoColumnLocalLineNum,
            this.DiffInfoColumnSvnLineNum,
            this.DiffInfoColumnIsChangedBySvnRevision,
            this.DiffInfoColumnResolveConflictWay});
            this.dgvDiffDefaultLanguageInfo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvDiffDefaultLanguageInfo.Location = new System.Drawing.Point(27, 128);
            this.dgvDiffDefaultLanguageInfo.Name = "dgvDiffDefaultLanguageInfo";
            this.dgvDiffDefaultLanguageInfo.RowTemplate.Height = 23;
            this.dgvDiffDefaultLanguageInfo.Size = new System.Drawing.Size(1164, 177);
            this.dgvDiffDefaultLanguageInfo.TabIndex = 6;
            // 
            // DiffInfoColumnNum
            // 
            this.DiffInfoColumnNum.FillWeight = 60F;
            this.DiffInfoColumnNum.HeaderText = "编号";
            this.DiffInfoColumnNum.Name = "DiffInfoColumnNum";
            this.DiffInfoColumnNum.ReadOnly = true;
            // 
            // DiffInfoColumnKey
            // 
            this.DiffInfoColumnKey.FillWeight = 200F;
            this.DiffInfoColumnKey.HeaderText = "Key";
            this.DiffInfoColumnKey.Name = "DiffInfoColumnKey";
            this.DiffInfoColumnKey.ReadOnly = true;
            // 
            // DiffInfoColumnLocalDefaultLanguage
            // 
            this.DiffInfoColumnLocalDefaultLanguage.FillWeight = 300F;
            this.DiffInfoColumnLocalDefaultLanguage.HeaderText = "本地表主语言译文";
            this.DiffInfoColumnLocalDefaultLanguage.Name = "DiffInfoColumnLocalDefaultLanguage";
            this.DiffInfoColumnLocalDefaultLanguage.ReadOnly = true;
            // 
            // DiffInfoColumnSvnDefaultLanguage
            // 
            this.DiffInfoColumnSvnDefaultLanguage.FillWeight = 300F;
            this.DiffInfoColumnSvnDefaultLanguage.HeaderText = "SVN表主语言译文";
            this.DiffInfoColumnSvnDefaultLanguage.Name = "DiffInfoColumnSvnDefaultLanguage";
            this.DiffInfoColumnSvnDefaultLanguage.ReadOnly = true;
            // 
            // DiffInfoColumnLocalLineNum
            // 
            this.DiffInfoColumnLocalLineNum.HeaderText = "本地表行号";
            this.DiffInfoColumnLocalLineNum.Name = "DiffInfoColumnLocalLineNum";
            this.DiffInfoColumnLocalLineNum.ReadOnly = true;
            // 
            // DiffInfoColumnSvnLineNum
            // 
            this.DiffInfoColumnSvnLineNum.HeaderText = "SVN表行号";
            this.DiffInfoColumnSvnLineNum.Name = "DiffInfoColumnSvnLineNum";
            this.DiffInfoColumnSvnLineNum.ReadOnly = true;
            // 
            // DiffInfoColumnIsChangedBySvnRevision
            // 
            this.DiffInfoColumnIsChangedBySvnRevision.HeaderText = "版本变动";
            this.DiffInfoColumnIsChangedBySvnRevision.Name = "DiffInfoColumnIsChangedBySvnRevision";
            this.DiffInfoColumnIsChangedBySvnRevision.ReadOnly = true;
            // 
            // DiffInfoColumnResolveConflictWay
            // 
            this.DiffInfoColumnResolveConflictWay.HeaderText = "处理方式";
            this.DiffInfoColumnResolveConflictWay.Name = "DiffInfoColumnResolveConflictWay";
            this.DiffInfoColumnResolveConflictWay.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.DiffInfoColumnResolveConflictWay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // lblDiffInfoUnifiedResolveConflictWay
            // 
            this.lblDiffInfoUnifiedResolveConflictWay.AutoSize = true;
            this.lblDiffInfoUnifiedResolveConflictWay.Location = new System.Drawing.Point(967, 98);
            this.lblDiffInfoUnifiedResolveConflictWay.Name = "lblDiffInfoUnifiedResolveConflictWay";
            this.lblDiffInfoUnifiedResolveConflictWay.Size = new System.Drawing.Size(125, 12);
            this.lblDiffInfoUnifiedResolveConflictWay.TabIndex = 7;
            this.lblDiffInfoUnifiedResolveConflictWay.Text = "统一使用此处理方式：";
            // 
            // cmbDiffInfoUnifiedResolveConflictWay
            // 
            this.cmbDiffInfoUnifiedResolveConflictWay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbDiffInfoUnifiedResolveConflictWay.FormattingEnabled = true;
            this.cmbDiffInfoUnifiedResolveConflictWay.Location = new System.Drawing.Point(1098, 95);
            this.cmbDiffInfoUnifiedResolveConflictWay.Name = "cmbDiffInfoUnifiedResolveConflictWay";
            this.cmbDiffInfoUnifiedResolveConflictWay.Size = new System.Drawing.Size(93, 20);
            this.cmbDiffInfoUnifiedResolveConflictWay.TabIndex = 8;
            // 
            // lblChangedBySvnRevisionTips
            // 
            this.lblChangedBySvnRevisionTips.AutoSize = true;
            this.lblChangedBySvnRevisionTips.Location = new System.Drawing.Point(736, 18);
            this.lblChangedBySvnRevisionTips.Name = "lblChangedBySvnRevisionTips";
            this.lblChangedBySvnRevisionTips.Size = new System.Drawing.Size(455, 36);
            this.lblChangedBySvnRevisionTips.TabIndex = 9;
            this.lblChangedBySvnRevisionTips.Text = "下列表格中“版本变动”列标明该变动是自己的修改还是SVN中两版本母表本身的变动\r\n若为“是”表示SVN中两版本母表本身存在变动，不是自己进行的修改\r\n本工具将情况" +
    "下的处理方式，默认选择为“使用SVN表”即不进行提交操作";
            // 
            // lblCommitLogMessage
            // 
            this.lblCommitLogMessage.AutoSize = true;
            this.lblCommitLogMessage.Location = new System.Drawing.Point(251, 21);
            this.lblCommitLogMessage.Name = "lblCommitLogMessage";
            this.lblCommitLogMessage.Size = new System.Drawing.Size(113, 12);
            this.lblCommitLogMessage.TabIndex = 10;
            this.lblCommitLogMessage.Text = "提交时的说明内容：";
            // 
            // txtCommitLogMessage
            // 
            this.txtCommitLogMessage.Location = new System.Drawing.Point(253, 44);
            this.txtCommitLogMessage.Name = "txtCommitLogMessage";
            this.txtCommitLogMessage.Size = new System.Drawing.Size(312, 21);
            this.txtCommitLogMessage.TabIndex = 11;
            // 
            // chkDiffInfoIgnoreSvnRevisionChange
            // 
            this.chkDiffInfoIgnoreSvnRevisionChange.AutoSize = true;
            this.chkDiffInfoIgnoreSvnRevisionChange.Checked = true;
            this.chkDiffInfoIgnoreSvnRevisionChange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkDiffInfoIgnoreSvnRevisionChange.Location = new System.Drawing.Point(656, 97);
            this.chkDiffInfoIgnoreSvnRevisionChange.Name = "chkDiffInfoIgnoreSvnRevisionChange";
            this.chkDiffInfoIgnoreSvnRevisionChange.Size = new System.Drawing.Size(276, 16);
            this.chkDiffInfoIgnoreSvnRevisionChange.TabIndex = 12;
            this.chkDiffInfoIgnoreSvnRevisionChange.Text = "忽略对“版本变动”为“是”的项进行批量处理";
            this.chkDiffInfoIgnoreSvnRevisionChange.UseVisualStyleBackColor = true;
            // 
            // lblLocalAddKeyInfo
            // 
            this.lblLocalAddKeyInfo.AutoSize = true;
            this.lblLocalAddKeyInfo.Location = new System.Drawing.Point(25, 330);
            this.lblLocalAddKeyInfo.Name = "lblLocalAddKeyInfo";
            this.lblLocalAddKeyInfo.Size = new System.Drawing.Size(281, 12);
            this.lblLocalAddKeyInfo.TabIndex = 13;
            this.lblLocalAddKeyInfo.Text = "以下为本地表中存在但最新SVN表中没有的Key信息：";
            // 
            // chkLocalAddKeyInfoIgnoreSvnRevisionChange
            // 
            this.chkLocalAddKeyInfoIgnoreSvnRevisionChange.AutoSize = true;
            this.chkLocalAddKeyInfoIgnoreSvnRevisionChange.Checked = true;
            this.chkLocalAddKeyInfoIgnoreSvnRevisionChange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLocalAddKeyInfoIgnoreSvnRevisionChange.Location = new System.Drawing.Point(656, 329);
            this.chkLocalAddKeyInfoIgnoreSvnRevisionChange.Name = "chkLocalAddKeyInfoIgnoreSvnRevisionChange";
            this.chkLocalAddKeyInfoIgnoreSvnRevisionChange.Size = new System.Drawing.Size(276, 16);
            this.chkLocalAddKeyInfoIgnoreSvnRevisionChange.TabIndex = 14;
            this.chkLocalAddKeyInfoIgnoreSvnRevisionChange.Text = "忽略对“版本变动”为“是”的项进行批量处理";
            this.chkLocalAddKeyInfoIgnoreSvnRevisionChange.UseVisualStyleBackColor = true;
            // 
            // lblLocalAddKeyInfoUnifiedResolveConflictWay
            // 
            this.lblLocalAddKeyInfoUnifiedResolveConflictWay.AutoSize = true;
            this.lblLocalAddKeyInfoUnifiedResolveConflictWay.Location = new System.Drawing.Point(967, 330);
            this.lblLocalAddKeyInfoUnifiedResolveConflictWay.Name = "lblLocalAddKeyInfoUnifiedResolveConflictWay";
            this.lblLocalAddKeyInfoUnifiedResolveConflictWay.Size = new System.Drawing.Size(125, 12);
            this.lblLocalAddKeyInfoUnifiedResolveConflictWay.TabIndex = 15;
            this.lblLocalAddKeyInfoUnifiedResolveConflictWay.Text = "统一使用此处理方式：";
            // 
            // cmbLocalAddKeyInfoUnifiedResolveConflictWay
            // 
            this.cmbLocalAddKeyInfoUnifiedResolveConflictWay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLocalAddKeyInfoUnifiedResolveConflictWay.FormattingEnabled = true;
            this.cmbLocalAddKeyInfoUnifiedResolveConflictWay.Location = new System.Drawing.Point(1098, 327);
            this.cmbLocalAddKeyInfoUnifiedResolveConflictWay.Name = "cmbLocalAddKeyInfoUnifiedResolveConflictWay";
            this.cmbLocalAddKeyInfoUnifiedResolveConflictWay.Size = new System.Drawing.Size(93, 20);
            this.cmbLocalAddKeyInfoUnifiedResolveConflictWay.TabIndex = 16;
            // 
            // dgvLocalAddKeyInfo
            // 
            this.dgvLocalAddKeyInfo.AllowUserToAddRows = false;
            this.dgvLocalAddKeyInfo.AllowUserToDeleteRows = false;
            this.dgvLocalAddKeyInfo.AllowUserToOrderColumns = true;
            this.dgvLocalAddKeyInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLocalAddKeyInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dgvLocalAddKeyInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLocalAddKeyInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.LocalAddKeyColumnNum,
            this.LocalAddKeyColumnKey,
            this.LocalAddKeyColumnLocalDefaultLanguage,
            this.LocalAddKeyColumnLocalLineNum,
            this.LocalAddKeyColumnIsChangedBySvnRevision,
            this.LocalAddKeyColumnResolveConflictWay});
            this.dgvLocalAddKeyInfo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvLocalAddKeyInfo.Location = new System.Drawing.Point(27, 360);
            this.dgvLocalAddKeyInfo.Name = "dgvLocalAddKeyInfo";
            this.dgvLocalAddKeyInfo.RowTemplate.Height = 23;
            this.dgvLocalAddKeyInfo.Size = new System.Drawing.Size(1164, 177);
            this.dgvLocalAddKeyInfo.TabIndex = 17;
            // 
            // LocalAddKeyColumnNum
            // 
            this.LocalAddKeyColumnNum.FillWeight = 60F;
            this.LocalAddKeyColumnNum.HeaderText = "编号";
            this.LocalAddKeyColumnNum.Name = "LocalAddKeyColumnNum";
            this.LocalAddKeyColumnNum.ReadOnly = true;
            // 
            // LocalAddKeyColumnKey
            // 
            this.LocalAddKeyColumnKey.FillWeight = 200F;
            this.LocalAddKeyColumnKey.HeaderText = "Key";
            this.LocalAddKeyColumnKey.Name = "LocalAddKeyColumnKey";
            this.LocalAddKeyColumnKey.ReadOnly = true;
            // 
            // LocalAddKeyColumnLocalDefaultLanguage
            // 
            this.LocalAddKeyColumnLocalDefaultLanguage.FillWeight = 300F;
            this.LocalAddKeyColumnLocalDefaultLanguage.HeaderText = "本地表主语言译文";
            this.LocalAddKeyColumnLocalDefaultLanguage.Name = "LocalAddKeyColumnLocalDefaultLanguage";
            this.LocalAddKeyColumnLocalDefaultLanguage.ReadOnly = true;
            // 
            // LocalAddKeyColumnLocalLineNum
            // 
            this.LocalAddKeyColumnLocalLineNum.HeaderText = "本地表行号";
            this.LocalAddKeyColumnLocalLineNum.Name = "LocalAddKeyColumnLocalLineNum";
            this.LocalAddKeyColumnLocalLineNum.ReadOnly = true;
            // 
            // LocalAddKeyColumnIsChangedBySvnRevision
            // 
            this.LocalAddKeyColumnIsChangedBySvnRevision.HeaderText = "版本变动";
            this.LocalAddKeyColumnIsChangedBySvnRevision.Name = "LocalAddKeyColumnIsChangedBySvnRevision";
            this.LocalAddKeyColumnIsChangedBySvnRevision.ReadOnly = true;
            // 
            // LocalAddKeyColumnResolveConflictWay
            // 
            this.LocalAddKeyColumnResolveConflictWay.HeaderText = "处理方式";
            this.LocalAddKeyColumnResolveConflictWay.Name = "LocalAddKeyColumnResolveConflictWay";
            this.LocalAddKeyColumnResolveConflictWay.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // lblSvnAddKeyInfo
            // 
            this.lblSvnAddKeyInfo.AutoSize = true;
            this.lblSvnAddKeyInfo.Location = new System.Drawing.Point(25, 562);
            this.lblSvnAddKeyInfo.Name = "lblSvnAddKeyInfo";
            this.lblSvnAddKeyInfo.Size = new System.Drawing.Size(281, 12);
            this.lblSvnAddKeyInfo.TabIndex = 18;
            this.lblSvnAddKeyInfo.Text = "以下为本地表中没有但最新SVN表中存在的Key信息：";
            // 
            // dgvSvnAddKeyInfo
            // 
            this.dgvSvnAddKeyInfo.AllowUserToAddRows = false;
            this.dgvSvnAddKeyInfo.AllowUserToDeleteRows = false;
            this.dgvSvnAddKeyInfo.AllowUserToOrderColumns = true;
            this.dgvSvnAddKeyInfo.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvSvnAddKeyInfo.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders;
            this.dgvSvnAddKeyInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSvnAddKeyInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.SvnAddKeyColumnNum,
            this.SvnAddKeyColumnKey,
            this.SvnAddKeyColumnSvnDefaultLanguage,
            this.SvnAddKeyColumnSvnLineNum,
            this.SvnAddKeyColumnIsChangedBySvnRevision,
            this.SvnAddKeyColumnResolveConflictWay});
            this.dgvSvnAddKeyInfo.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvSvnAddKeyInfo.Location = new System.Drawing.Point(27, 592);
            this.dgvSvnAddKeyInfo.Name = "dgvSvnAddKeyInfo";
            this.dgvSvnAddKeyInfo.RowTemplate.Height = 23;
            this.dgvSvnAddKeyInfo.Size = new System.Drawing.Size(1164, 177);
            this.dgvSvnAddKeyInfo.TabIndex = 19;
            // 
            // SvnAddKeyColumnNum
            // 
            this.SvnAddKeyColumnNum.FillWeight = 60F;
            this.SvnAddKeyColumnNum.HeaderText = "编号";
            this.SvnAddKeyColumnNum.Name = "SvnAddKeyColumnNum";
            this.SvnAddKeyColumnNum.ReadOnly = true;
            // 
            // SvnAddKeyColumnKey
            // 
            this.SvnAddKeyColumnKey.FillWeight = 200F;
            this.SvnAddKeyColumnKey.HeaderText = "Key";
            this.SvnAddKeyColumnKey.Name = "SvnAddKeyColumnKey";
            this.SvnAddKeyColumnKey.ReadOnly = true;
            // 
            // SvnAddKeyColumnSvnDefaultLanguage
            // 
            this.SvnAddKeyColumnSvnDefaultLanguage.FillWeight = 300F;
            this.SvnAddKeyColumnSvnDefaultLanguage.HeaderText = "SVN表主语言译文";
            this.SvnAddKeyColumnSvnDefaultLanguage.Name = "SvnAddKeyColumnSvnDefaultLanguage";
            this.SvnAddKeyColumnSvnDefaultLanguage.ReadOnly = true;
            // 
            // SvnAddKeyColumnSvnLineNum
            // 
            this.SvnAddKeyColumnSvnLineNum.HeaderText = "SVN表行号";
            this.SvnAddKeyColumnSvnLineNum.Name = "SvnAddKeyColumnSvnLineNum";
            this.SvnAddKeyColumnSvnLineNum.ReadOnly = true;
            // 
            // SvnAddKeyColumnIsChangedBySvnRevision
            // 
            this.SvnAddKeyColumnIsChangedBySvnRevision.HeaderText = "版本变动";
            this.SvnAddKeyColumnIsChangedBySvnRevision.Name = "SvnAddKeyColumnIsChangedBySvnRevision";
            this.SvnAddKeyColumnIsChangedBySvnRevision.ReadOnly = true;
            // 
            // SvnAddKeyColumnResolveConflictWay
            // 
            this.SvnAddKeyColumnResolveConflictWay.HeaderText = "处理方式";
            this.SvnAddKeyColumnResolveConflictWay.Name = "SvnAddKeyColumnResolveConflictWay";
            // 
            // chkSvnAddKeyInfoIgnoreSvnRevisionChange
            // 
            this.chkSvnAddKeyInfoIgnoreSvnRevisionChange.AutoSize = true;
            this.chkSvnAddKeyInfoIgnoreSvnRevisionChange.Checked = true;
            this.chkSvnAddKeyInfoIgnoreSvnRevisionChange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSvnAddKeyInfoIgnoreSvnRevisionChange.Location = new System.Drawing.Point(656, 561);
            this.chkSvnAddKeyInfoIgnoreSvnRevisionChange.Name = "chkSvnAddKeyInfoIgnoreSvnRevisionChange";
            this.chkSvnAddKeyInfoIgnoreSvnRevisionChange.Size = new System.Drawing.Size(276, 16);
            this.chkSvnAddKeyInfoIgnoreSvnRevisionChange.TabIndex = 20;
            this.chkSvnAddKeyInfoIgnoreSvnRevisionChange.Text = "忽略对“版本变动”为“是”的项进行批量处理";
            this.chkSvnAddKeyInfoIgnoreSvnRevisionChange.UseVisualStyleBackColor = true;
            // 
            // lblSvnAddKeyInfoUnifiedResolveConflictWay
            // 
            this.lblSvnAddKeyInfoUnifiedResolveConflictWay.AutoSize = true;
            this.lblSvnAddKeyInfoUnifiedResolveConflictWay.Location = new System.Drawing.Point(967, 562);
            this.lblSvnAddKeyInfoUnifiedResolveConflictWay.Name = "lblSvnAddKeyInfoUnifiedResolveConflictWay";
            this.lblSvnAddKeyInfoUnifiedResolveConflictWay.Size = new System.Drawing.Size(125, 12);
            this.lblSvnAddKeyInfoUnifiedResolveConflictWay.TabIndex = 21;
            this.lblSvnAddKeyInfoUnifiedResolveConflictWay.Text = "统一使用此处理方式：";
            // 
            // cmbSvnAddKeyInfoUnifiedResolveConflictWay
            // 
            this.cmbSvnAddKeyInfoUnifiedResolveConflictWay.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSvnAddKeyInfoUnifiedResolveConflictWay.FormattingEnabled = true;
            this.cmbSvnAddKeyInfoUnifiedResolveConflictWay.Location = new System.Drawing.Point(1098, 559);
            this.cmbSvnAddKeyInfoUnifiedResolveConflictWay.Name = "cmbSvnAddKeyInfoUnifiedResolveConflictWay";
            this.cmbSvnAddKeyInfoUnifiedResolveConflictWay.Size = new System.Drawing.Size(93, 20);
            this.cmbSvnAddKeyInfoUnifiedResolveConflictWay.TabIndex = 22;
            // 
            // ResolveConflictWhenCommitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1219, 794);
            this.Controls.Add(this.cmbSvnAddKeyInfoUnifiedResolveConflictWay);
            this.Controls.Add(this.lblSvnAddKeyInfoUnifiedResolveConflictWay);
            this.Controls.Add(this.chkSvnAddKeyInfoIgnoreSvnRevisionChange);
            this.Controls.Add(this.dgvSvnAddKeyInfo);
            this.Controls.Add(this.lblSvnAddKeyInfo);
            this.Controls.Add(this.dgvLocalAddKeyInfo);
            this.Controls.Add(this.cmbLocalAddKeyInfoUnifiedResolveConflictWay);
            this.Controls.Add(this.lblLocalAddKeyInfoUnifiedResolveConflictWay);
            this.Controls.Add(this.chkLocalAddKeyInfoIgnoreSvnRevisionChange);
            this.Controls.Add(this.lblLocalAddKeyInfo);
            this.Controls.Add(this.chkDiffInfoIgnoreSvnRevisionChange);
            this.Controls.Add(this.txtCommitLogMessage);
            this.Controls.Add(this.lblCommitLogMessage);
            this.Controls.Add(this.lblChangedBySvnRevisionTips);
            this.Controls.Add(this.cmbDiffInfoUnifiedResolveConflictWay);
            this.Controls.Add(this.lblDiffInfoUnifiedResolveConflictWay);
            this.Controls.Add(this.dgvDiffDefaultLanguageInfo);
            this.Controls.Add(this.lblDiffInfo);
            this.Controls.Add(this.btnCommit);
            this.Controls.Add(this.txtSvnFileRevision);
            this.Controls.Add(this.lblSvnFileRevision);
            this.Controls.Add(this.txtLocalFileRevision);
            this.Controls.Add(this.lblLocalFileRevision);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ResolveConflictWhenCommitForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "请选择要合并到SVN中母表的改动";
            ((System.ComponentModel.ISupportInitialize)(this.dgvDiffDefaultLanguageInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLocalAddKeyInfo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSvnAddKeyInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblLocalFileRevision;
        private System.Windows.Forms.TextBox txtLocalFileRevision;
        private System.Windows.Forms.Label lblSvnFileRevision;
        private System.Windows.Forms.TextBox txtSvnFileRevision;
        private System.Windows.Forms.Button btnCommit;
        private System.Windows.Forms.Label lblDiffInfo;
        private System.Windows.Forms.DataGridView dgvDiffDefaultLanguageInfo;
        private System.Windows.Forms.Label lblDiffInfoUnifiedResolveConflictWay;
        private System.Windows.Forms.ComboBox cmbDiffInfoUnifiedResolveConflictWay;
        private System.Windows.Forms.Label lblChangedBySvnRevisionTips;
        private System.Windows.Forms.Label lblCommitLogMessage;
        private System.Windows.Forms.TextBox txtCommitLogMessage;
        private System.Windows.Forms.CheckBox chkDiffInfoIgnoreSvnRevisionChange;
        private System.Windows.Forms.Label lblLocalAddKeyInfo;
        private System.Windows.Forms.CheckBox chkLocalAddKeyInfoIgnoreSvnRevisionChange;
        private System.Windows.Forms.Label lblLocalAddKeyInfoUnifiedResolveConflictWay;
        private System.Windows.Forms.ComboBox cmbLocalAddKeyInfoUnifiedResolveConflictWay;
        private System.Windows.Forms.DataGridView dgvLocalAddKeyInfo;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiffInfoColumnNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiffInfoColumnKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiffInfoColumnLocalDefaultLanguage;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiffInfoColumnSvnDefaultLanguage;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiffInfoColumnLocalLineNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiffInfoColumnSvnLineNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn DiffInfoColumnIsChangedBySvnRevision;
        private System.Windows.Forms.DataGridViewComboBoxColumn DiffInfoColumnResolveConflictWay;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalAddKeyColumnNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalAddKeyColumnKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalAddKeyColumnLocalDefaultLanguage;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalAddKeyColumnLocalLineNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn LocalAddKeyColumnIsChangedBySvnRevision;
        private System.Windows.Forms.DataGridViewComboBoxColumn LocalAddKeyColumnResolveConflictWay;
        private System.Windows.Forms.Label lblSvnAddKeyInfo;
        private System.Windows.Forms.DataGridView dgvSvnAddKeyInfo;
        private System.Windows.Forms.CheckBox chkSvnAddKeyInfoIgnoreSvnRevisionChange;
        private System.Windows.Forms.Label lblSvnAddKeyInfoUnifiedResolveConflictWay;
        private System.Windows.Forms.ComboBox cmbSvnAddKeyInfoUnifiedResolveConflictWay;
        private System.Windows.Forms.DataGridViewTextBoxColumn SvnAddKeyColumnNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn SvnAddKeyColumnKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn SvnAddKeyColumnSvnDefaultLanguage;
        private System.Windows.Forms.DataGridViewTextBoxColumn SvnAddKeyColumnSvnLineNum;
        private System.Windows.Forms.DataGridViewTextBoxColumn SvnAddKeyColumnIsChangedBySvnRevision;
        private System.Windows.Forms.DataGridViewComboBoxColumn SvnAddKeyColumnResolveConflictWay;
    }
}