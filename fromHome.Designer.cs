namespace OperationXI
{
    partial class frmHome
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
            this.bgwGenerateTeam = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.cboTeamA = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboTeamB = new System.Windows.Forms.ComboBox();
            this.tabControlPlayer = new System.Windows.Forms.TabControl();
            this.tabWK = new System.Windows.Forms.TabPage();
            this.dgvWK = new System.Windows.Forms.DataGridView();
            this.tabBAT = new System.Windows.Forms.TabPage();
            this.dgvBAT = new System.Windows.Forms.DataGridView();
            this.tabALL = new System.Windows.Forms.TabPage();
            this.dgvALL = new System.Windows.Forms.DataGridView();
            this.tabBOWL = new System.Windows.Forms.TabPage();
            this.dgvBOWL = new System.Windows.Forms.DataGridView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tsProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.tslblMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip2 = new System.Windows.Forms.MenuStrip();
            this.tsmiTeam = new System.Windows.Forms.ToolStripMenuItem();
            this.btnGenerateTeam = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.cboTeamTyoe = new System.Windows.Forms.ComboBox();
            this.dgvcWKSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvcWKTeamCode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcWKPlayerName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcWKCredits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcWKCaptain = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dgvcBATSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column2 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn5 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column4 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.Column3 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.tabControlPlayer.SuspendLayout();
            this.tabWK.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWK)).BeginInit();
            this.tabBAT.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBAT)).BeginInit();
            this.tabALL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvALL)).BeginInit();
            this.tabBOWL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBOWL)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.menuStrip2.SuspendLayout();
            this.SuspendLayout();
            // 
            // bgwGenerateTeam
            // 
            this.bgwGenerateTeam.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwGenerateTeam_DoWork);
            this.bgwGenerateTeam.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwGenerateTeam_RunWorkerCompleted);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 43);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(58, 16);
            this.label1.TabIndex = 0;
            this.label1.Text = "Team A";
            // 
            // cboTeamA
            // 
            this.cboTeamA.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTeamA.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTeamA.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTeamA.FormattingEnabled = true;
            this.cboTeamA.Location = new System.Drawing.Point(103, 39);
            this.cboTeamA.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboTeamA.Name = "cboTeamA";
            this.cboTeamA.Size = new System.Drawing.Size(432, 24);
            this.cboTeamA.TabIndex = 1;
            this.cboTeamA.SelectedIndexChanged += new System.EventHandler(this.cboTeamA_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(57, 16);
            this.label2.TabIndex = 0;
            this.label2.Text = "Team B";
            // 
            // cboTeamB
            // 
            this.cboTeamB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTeamB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTeamB.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTeamB.FormattingEnabled = true;
            this.cboTeamB.Location = new System.Drawing.Point(103, 75);
            this.cboTeamB.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboTeamB.Name = "cboTeamB";
            this.cboTeamB.Size = new System.Drawing.Size(432, 24);
            this.cboTeamB.TabIndex = 1;
            this.cboTeamB.SelectedIndexChanged += new System.EventHandler(this.cboTeamB_SelectedIndexChanged);
            // 
            // tabControlPlayer
            // 
            this.tabControlPlayer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlPlayer.Controls.Add(this.tabWK);
            this.tabControlPlayer.Controls.Add(this.tabBAT);
            this.tabControlPlayer.Controls.Add(this.tabALL);
            this.tabControlPlayer.Controls.Add(this.tabBOWL);
            this.tabControlPlayer.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tabControlPlayer.Location = new System.Drawing.Point(15, 139);
            this.tabControlPlayer.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabControlPlayer.Name = "tabControlPlayer";
            this.tabControlPlayer.SelectedIndex = 0;
            this.tabControlPlayer.Size = new System.Drawing.Size(521, 408);
            this.tabControlPlayer.TabIndex = 2;
            // 
            // tabWK
            // 
            this.tabWK.Controls.Add(this.dgvWK);
            this.tabWK.Location = new System.Drawing.Point(4, 25);
            this.tabWK.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabWK.Name = "tabWK";
            this.tabWK.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabWK.Size = new System.Drawing.Size(513, 379);
            this.tabWK.TabIndex = 0;
            this.tabWK.Text = "WK";
            this.tabWK.UseVisualStyleBackColor = true;
            // 
            // dgvWK
            // 
            this.dgvWK.AllowUserToAddRows = false;
            this.dgvWK.AllowUserToDeleteRows = false;
            this.dgvWK.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWK.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcWKSelected,
            this.dgvcWKTeamCode,
            this.dgvcWKPlayerName,
            this.dgvcWKCredits,
            this.dgvcWKCaptain,
            this.Column1});
            this.dgvWK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWK.Location = new System.Drawing.Point(3, 4);
            this.dgvWK.Name = "dgvWK";
            this.dgvWK.Size = new System.Drawing.Size(507, 371);
            this.dgvWK.TabIndex = 0;
            this.dgvWK.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWK_CellContentClick);
            this.dgvWK.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvWK_DataError);
            // 
            // tabBAT
            // 
            this.tabBAT.Controls.Add(this.dgvBAT);
            this.tabBAT.Location = new System.Drawing.Point(4, 25);
            this.tabBAT.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabBAT.Name = "tabBAT";
            this.tabBAT.Padding = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.tabBAT.Size = new System.Drawing.Size(513, 379);
            this.tabBAT.TabIndex = 1;
            this.tabBAT.Text = "BAT";
            this.tabBAT.UseVisualStyleBackColor = true;
            // 
            // dgvBAT
            // 
            this.dgvBAT.AllowUserToAddRows = false;
            this.dgvBAT.AllowUserToDeleteRows = false;
            this.dgvBAT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBAT.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcBATSelected,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewCheckBoxColumn2,
            this.Column2});
            this.dgvBAT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBAT.Location = new System.Drawing.Point(3, 4);
            this.dgvBAT.Name = "dgvBAT";
            this.dgvBAT.Size = new System.Drawing.Size(507, 371);
            this.dgvBAT.TabIndex = 1;
            this.dgvBAT.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBAT_CellContentClick);
            // 
            // tabALL
            // 
            this.tabALL.Controls.Add(this.dgvALL);
            this.tabALL.Location = new System.Drawing.Point(4, 25);
            this.tabALL.Name = "tabALL";
            this.tabALL.Padding = new System.Windows.Forms.Padding(3);
            this.tabALL.Size = new System.Drawing.Size(513, 379);
            this.tabALL.TabIndex = 2;
            this.tabALL.Text = "ALL";
            this.tabALL.UseVisualStyleBackColor = true;
            // 
            // dgvALL
            // 
            this.dgvALL.AllowUserToAddRows = false;
            this.dgvALL.AllowUserToDeleteRows = false;
            this.dgvALL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvALL.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewCheckBoxColumn3,
            this.Column3});
            this.dgvALL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvALL.Location = new System.Drawing.Point(3, 3);
            this.dgvALL.Name = "dgvALL";
            this.dgvALL.Size = new System.Drawing.Size(507, 373);
            this.dgvALL.TabIndex = 1;
            this.dgvALL.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvALL_CellContentClick);
            // 
            // tabBOWL
            // 
            this.tabBOWL.Controls.Add(this.dgvBOWL);
            this.tabBOWL.Location = new System.Drawing.Point(4, 25);
            this.tabBOWL.Name = "tabBOWL";
            this.tabBOWL.Padding = new System.Windows.Forms.Padding(3);
            this.tabBOWL.Size = new System.Drawing.Size(513, 379);
            this.tabBOWL.TabIndex = 3;
            this.tabBOWL.Text = "BOWL";
            this.tabBOWL.UseVisualStyleBackColor = true;
            // 
            // dgvBOWL
            // 
            this.dgvBOWL.AllowUserToAddRows = false;
            this.dgvBOWL.AllowUserToDeleteRows = false;
            this.dgvBOWL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBOWL.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewCheckBoxColumn4,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn9,
            this.dataGridViewCheckBoxColumn5,
            this.Column4});
            this.dgvBOWL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBOWL.Location = new System.Drawing.Point(3, 3);
            this.dgvBOWL.Name = "dgvBOWL";
            this.dgvBOWL.Size = new System.Drawing.Size(507, 373);
            this.dgvBOWL.TabIndex = 2;
            this.dgvBOWL.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBOWL_CellContentClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsProgress,
            this.tslblMessage});
            this.statusStrip1.Location = new System.Drawing.Point(0, 595);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(549, 22);
            this.statusStrip1.TabIndex = 4;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tsProgress
            // 
            this.tsProgress.Name = "tsProgress";
            this.tsProgress.Size = new System.Drawing.Size(114, 20);
            this.tsProgress.Visible = false;
            // 
            // tslblMessage
            // 
            this.tslblMessage.Name = "tslblMessage";
            this.tslblMessage.Size = new System.Drawing.Size(0, 17);
            this.tslblMessage.Visible = false;
            // 
            // menuStrip2
            // 
            this.menuStrip2.BackColor = System.Drawing.Color.BurlyWood;
            this.menuStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiTeam});
            this.menuStrip2.Location = new System.Drawing.Point(0, 0);
            this.menuStrip2.Name = "menuStrip2";
            this.menuStrip2.Padding = new System.Windows.Forms.Padding(7, 2, 0, 2);
            this.menuStrip2.Size = new System.Drawing.Size(549, 24);
            this.menuStrip2.TabIndex = 6;
            this.menuStrip2.Text = "menuStrip2";
            // 
            // tsmiTeam
            // 
            this.tsmiTeam.Name = "tsmiTeam";
            this.tsmiTeam.Size = new System.Drawing.Size(49, 20);
            this.tsmiTeam.Text = "&Team";
            this.tsmiTeam.Click += new System.EventHandler(this.tsmiTeam_Click);
            // 
            // btnGenerateTeam
            // 
            this.btnGenerateTeam.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnGenerateTeam.Location = new System.Drawing.Point(376, 555);
            this.btnGenerateTeam.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.btnGenerateTeam.Name = "btnGenerateTeam";
            this.btnGenerateTeam.Size = new System.Drawing.Size(159, 28);
            this.btnGenerateTeam.TabIndex = 3;
            this.btnGenerateTeam.Text = "Genereate Teams";
            this.btnGenerateTeam.UseVisualStyleBackColor = true;
            this.btnGenerateTeam.Click += new System.EventHandler(this.btnGenerateTeam_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 111);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 16);
            this.label3.TabIndex = 0;
            this.label3.Text = "Team Type";
            // 
            // cboTeamTyoe
            // 
            this.cboTeamTyoe.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTeamTyoe.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTeamTyoe.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cboTeamTyoe.FormattingEnabled = true;
            this.cboTeamTyoe.Location = new System.Drawing.Point(103, 107);
            this.cboTeamTyoe.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.cboTeamTyoe.Name = "cboTeamTyoe";
            this.cboTeamTyoe.Size = new System.Drawing.Size(432, 24);
            this.cboTeamTyoe.TabIndex = 1;
            // 
            // dgvcWKSelected
            // 
            this.dgvcWKSelected.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvcWKSelected.HeaderText = "";
            this.dgvcWKSelected.Name = "dgvcWKSelected";
            this.dgvcWKSelected.Width = 5;
            // 
            // dgvcWKTeamCode
            // 
            this.dgvcWKTeamCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvcWKTeamCode.DataPropertyName = "TeamCode";
            this.dgvcWKTeamCode.HeaderText = "Team";
            this.dgvcWKTeamCode.Name = "dgvcWKTeamCode";
            this.dgvcWKTeamCode.ReadOnly = true;
            this.dgvcWKTeamCode.Width = 69;
            // 
            // dgvcWKPlayerName
            // 
            this.dgvcWKPlayerName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcWKPlayerName.DataPropertyName = "PlayerName";
            this.dgvcWKPlayerName.HeaderText = "Name";
            this.dgvcWKPlayerName.Name = "dgvcWKPlayerName";
            this.dgvcWKPlayerName.ReadOnly = true;
            // 
            // dgvcWKCredits
            // 
            this.dgvcWKCredits.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvcWKCredits.DataPropertyName = "Credits";
            this.dgvcWKCredits.HeaderText = "Credits";
            this.dgvcWKCredits.Name = "dgvcWKCredits";
            this.dgvcWKCredits.ReadOnly = true;
            this.dgvcWKCredits.Width = 79;
            // 
            // dgvcWKCaptain
            // 
            this.dgvcWKCaptain.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvcWKCaptain.HeaderText = "Must";
            this.dgvcWKCaptain.Name = "dgvcWKCaptain";
            this.dgvcWKCaptain.ReadOnly = true;
            this.dgvcWKCaptain.Width = 46;
            // 
            // Column1
            // 
            this.Column1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column1.HeaderText = "Cap";
            this.Column1.Name = "Column1";
            this.Column1.Width = 39;
            // 
            // dgvcBATSelected
            // 
            this.dgvcBATSelected.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvcBATSelected.HeaderText = "";
            this.dgvcBATSelected.Name = "dgvcBATSelected";
            this.dgvcBATSelected.Width = 5;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn1.DataPropertyName = "TeamCode";
            this.dataGridViewTextBoxColumn1.HeaderText = "Team";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 69;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn2.DataPropertyName = "PlayerName";
            this.dataGridViewTextBoxColumn2.HeaderText = "Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn3.DataPropertyName = "Credits";
            this.dataGridViewTextBoxColumn3.HeaderText = "Credits";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            this.dataGridViewTextBoxColumn3.Width = 79;
            // 
            // dataGridViewCheckBoxColumn2
            // 
            this.dataGridViewCheckBoxColumn2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewCheckBoxColumn2.HeaderText = "Must";
            this.dataGridViewCheckBoxColumn2.Name = "dataGridViewCheckBoxColumn2";
            this.dataGridViewCheckBoxColumn2.ReadOnly = true;
            this.dataGridViewCheckBoxColumn2.Width = 46;
            // 
            // Column2
            // 
            this.Column2.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column2.HeaderText = "Cap";
            this.Column2.Name = "Column2";
            this.Column2.Width = 39;
            // 
            // dataGridViewCheckBoxColumn4
            // 
            this.dataGridViewCheckBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewCheckBoxColumn4.HeaderText = "";
            this.dataGridViewCheckBoxColumn4.Name = "dataGridViewCheckBoxColumn4";
            this.dataGridViewCheckBoxColumn4.Width = 5;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn7.DataPropertyName = "TeamCode";
            this.dataGridViewTextBoxColumn7.HeaderText = "Team";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            this.dataGridViewTextBoxColumn7.Width = 69;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn8.DataPropertyName = "PlayerName";
            this.dataGridViewTextBoxColumn8.HeaderText = "Name";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn9
            // 
            this.dataGridViewTextBoxColumn9.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn9.DataPropertyName = "Credits";
            this.dataGridViewTextBoxColumn9.HeaderText = "Credits";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.ReadOnly = true;
            this.dataGridViewTextBoxColumn9.Width = 79;
            // 
            // dataGridViewCheckBoxColumn5
            // 
            this.dataGridViewCheckBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewCheckBoxColumn5.HeaderText = "Must";
            this.dataGridViewCheckBoxColumn5.Name = "dataGridViewCheckBoxColumn5";
            this.dataGridViewCheckBoxColumn5.ReadOnly = true;
            this.dataGridViewCheckBoxColumn5.Width = 46;
            // 
            // Column4
            // 
            this.Column4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column4.HeaderText = "Cap";
            this.Column4.Name = "Column4";
            this.Column4.Width = 39;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewCheckBoxColumn1.HeaderText = "";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.Width = 5;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn4.DataPropertyName = "TeamCode";
            this.dataGridViewTextBoxColumn4.HeaderText = "Team";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 69;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxColumn5.DataPropertyName = "PlayerName";
            this.dataGridViewTextBoxColumn5.HeaderText = "Name";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewTextBoxColumn6.DataPropertyName = "Credits";
            this.dataGridViewTextBoxColumn6.HeaderText = "Credits";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            this.dataGridViewTextBoxColumn6.Width = 79;
            // 
            // dataGridViewCheckBoxColumn3
            // 
            this.dataGridViewCheckBoxColumn3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dataGridViewCheckBoxColumn3.HeaderText = "Must";
            this.dataGridViewCheckBoxColumn3.Name = "dataGridViewCheckBoxColumn3";
            this.dataGridViewCheckBoxColumn3.ReadOnly = true;
            this.dataGridViewCheckBoxColumn3.Width = 46;
            // 
            // Column3
            // 
            this.Column3.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.Column3.HeaderText = "Cap";
            this.Column3.Name = "Column3";
            this.Column3.Width = 39;
            // 
            // frmHome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(549, 617);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip2);
            this.Controls.Add(this.btnGenerateTeam);
            this.Controls.Add(this.tabControlPlayer);
            this.Controls.Add(this.cboTeamTyoe);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboTeamB);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboTeamA);
            this.Controls.Add(this.label1);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmHome";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "OperationXI";
            this.Load += new System.EventHandler(this.frmHome_Load);
            this.tabControlPlayer.ResumeLayout(false);
            this.tabWK.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWK)).EndInit();
            this.tabBAT.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBAT)).EndInit();
            this.tabALL.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvALL)).EndInit();
            this.tabBOWL.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBOWL)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.menuStrip2.ResumeLayout(false);
            this.menuStrip2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bgwGenerateTeam;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboTeamA;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboTeamB;
        private System.Windows.Forms.TabControl tabControlPlayer;
        private System.Windows.Forms.TabPage tabWK;
        private System.Windows.Forms.TabPage tabBAT;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar tsProgress;
        private System.Windows.Forms.MenuStrip menuStrip2;
        private System.Windows.Forms.ToolStripMenuItem tsmiTeam;
        private System.Windows.Forms.ToolStripStatusLabel tslblMessage;
        private System.Windows.Forms.DataGridView dgvWK;
        private System.Windows.Forms.DataGridView dgvBAT;
        private System.Windows.Forms.TabPage tabALL;
        private System.Windows.Forms.DataGridView dgvALL;
        private System.Windows.Forms.TabPage tabBOWL;
        private System.Windows.Forms.DataGridView dgvBOWL;
        private System.Windows.Forms.Button btnGenerateTeam;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboTeamTyoe;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvcWKSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcWKTeamCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcWKPlayerName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcWKCredits;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvcWKCaptain;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column1;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dgvcBATSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column2;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column3;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn5;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Column4;
    }
}

