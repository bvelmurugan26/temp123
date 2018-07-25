namespace OperationXI
{
    partial class frmTeam
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
            this.cboTeam = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControlPlayer = new System.Windows.Forms.TabControl();
            this.tabWK = new System.Windows.Forms.TabPage();
            this.dgvWK = new System.Windows.Forms.DataGridView();
            this.dgvcWKPlayerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcWKName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcWKCredits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabBAT = new System.Windows.Forms.TabPage();
            this.dgvBAT = new System.Windows.Forms.DataGridView();
            this.dgvcBATPlayerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcBATName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcBATCredits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabALL = new System.Windows.Forms.TabPage();
            this.dgvALL = new System.Windows.Forms.DataGridView();
            this.dgvcALLPlayerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcALLName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcALLCredits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tabBOWL = new System.Windows.Forms.TabPage();
            this.dgvBOWL = new System.Windows.Forms.DataGridView();
            this.dgvcBOWLPlayerID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcBOWLName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvcBOWLCredits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tabControlPlayer.SuspendLayout();
            this.tabWK.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvWK)).BeginInit();
            this.tabBAT.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBAT)).BeginInit();
            this.tabALL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvALL)).BeginInit();
            this.tabBOWL.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBOWL)).BeginInit();
            this.SuspendLayout();
            // 
            // cboTeam
            // 
            this.cboTeam.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboTeam.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTeam.FormattingEnabled = true;
            this.cboTeam.Location = new System.Drawing.Point(61, 13);
            this.cboTeam.Name = "cboTeam";
            this.cboTeam.Size = new System.Drawing.Size(390, 22);
            this.cboTeam.TabIndex = 3;
            this.cboTeam.SelectedIndexChanged += new System.EventHandler(this.cboTeam_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 14);
            this.label1.TabIndex = 2;
            this.label1.Text = "Team";
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
            this.tabControlPlayer.Location = new System.Drawing.Point(9, 46);
            this.tabControlPlayer.Name = "tabControlPlayer";
            this.tabControlPlayer.SelectedIndex = 0;
            this.tabControlPlayer.Size = new System.Drawing.Size(442, 433);
            this.tabControlPlayer.TabIndex = 6;
            // 
            // tabWK
            // 
            this.tabWK.Controls.Add(this.dgvWK);
            this.tabWK.Location = new System.Drawing.Point(4, 23);
            this.tabWK.Name = "tabWK";
            this.tabWK.Padding = new System.Windows.Forms.Padding(3);
            this.tabWK.Size = new System.Drawing.Size(434, 406);
            this.tabWK.TabIndex = 0;
            this.tabWK.Text = "WK";
            this.tabWK.UseVisualStyleBackColor = true;
            // 
            // dgvWK
            // 
            this.dgvWK.AllowUserToDeleteRows = false;
            this.dgvWK.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvWK.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcWKPlayerID,
            this.dgvcWKName,
            this.dgvcWKCredits});
            this.dgvWK.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvWK.Location = new System.Drawing.Point(3, 3);
            this.dgvWK.Name = "dgvWK";
            this.dgvWK.Size = new System.Drawing.Size(428, 400);
            this.dgvWK.TabIndex = 0;
            this.dgvWK.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvWK_CellEndEdit);
            this.dgvWK.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvWK_EditingControlShowing);
            // 
            // dgvcWKPlayerID
            // 
            this.dgvcWKPlayerID.DataPropertyName = "PlayerID";
            this.dgvcWKPlayerID.HeaderText = "PlayerID";
            this.dgvcWKPlayerID.Name = "dgvcWKPlayerID";
            this.dgvcWKPlayerID.Visible = false;
            // 
            // dgvcWKName
            // 
            this.dgvcWKName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcWKName.DataPropertyName = "PlayerName";
            this.dgvcWKName.HeaderText = "Name";
            this.dgvcWKName.Name = "dgvcWKName";
            // 
            // dgvcWKCredits
            // 
            this.dgvcWKCredits.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvcWKCredits.DataPropertyName = "Credits";
            this.dgvcWKCredits.HeaderText = "Credits";
            this.dgvcWKCredits.Name = "dgvcWKCredits";
            this.dgvcWKCredits.Width = 77;
            // 
            // tabBAT
            // 
            this.tabBAT.Controls.Add(this.dgvBAT);
            this.tabBAT.Location = new System.Drawing.Point(4, 22);
            this.tabBAT.Name = "tabBAT";
            this.tabBAT.Padding = new System.Windows.Forms.Padding(3);
            this.tabBAT.Size = new System.Drawing.Size(434, 407);
            this.tabBAT.TabIndex = 1;
            this.tabBAT.Text = "BAT";
            this.tabBAT.UseVisualStyleBackColor = true;
            // 
            // dgvBAT
            // 
            this.dgvBAT.AllowUserToDeleteRows = false;
            this.dgvBAT.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBAT.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcBATPlayerID,
            this.dgvcBATName,
            this.dgvcBATCredits});
            this.dgvBAT.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBAT.Location = new System.Drawing.Point(3, 3);
            this.dgvBAT.Name = "dgvBAT";
            this.dgvBAT.Size = new System.Drawing.Size(428, 401);
            this.dgvBAT.TabIndex = 1;
            this.dgvBAT.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBAT_CellEndEdit);
            this.dgvBAT.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvBAT_EditingControlShowing);
            // 
            // dgvcBATPlayerID
            // 
            this.dgvcBATPlayerID.DataPropertyName = "PlayerID";
            this.dgvcBATPlayerID.HeaderText = "PlayerID";
            this.dgvcBATPlayerID.Name = "dgvcBATPlayerID";
            this.dgvcBATPlayerID.ReadOnly = true;
            this.dgvcBATPlayerID.Visible = false;
            // 
            // dgvcBATName
            // 
            this.dgvcBATName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcBATName.DataPropertyName = "PlayerName";
            this.dgvcBATName.HeaderText = "Name";
            this.dgvcBATName.Name = "dgvcBATName";
            // 
            // dgvcBATCredits
            // 
            this.dgvcBATCredits.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvcBATCredits.DataPropertyName = "Credits";
            this.dgvcBATCredits.HeaderText = "Credits";
            this.dgvcBATCredits.Name = "dgvcBATCredits";
            this.dgvcBATCredits.Width = 77;
            // 
            // tabALL
            // 
            this.tabALL.Controls.Add(this.dgvALL);
            this.tabALL.Location = new System.Drawing.Point(4, 22);
            this.tabALL.Name = "tabALL";
            this.tabALL.Size = new System.Drawing.Size(434, 407);
            this.tabALL.TabIndex = 2;
            this.tabALL.Text = "ALL";
            this.tabALL.UseVisualStyleBackColor = true;
            // 
            // dgvALL
            // 
            this.dgvALL.AllowUserToDeleteRows = false;
            this.dgvALL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvALL.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcALLPlayerID,
            this.dgvcALLName,
            this.dgvcALLCredits});
            this.dgvALL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvALL.Location = new System.Drawing.Point(0, 0);
            this.dgvALL.Name = "dgvALL";
            this.dgvALL.Size = new System.Drawing.Size(434, 407);
            this.dgvALL.TabIndex = 2;
            this.dgvALL.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvALL_CellEndEdit);
            this.dgvALL.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvALL_EditingControlShowing);
            // 
            // dgvcALLPlayerID
            // 
            this.dgvcALLPlayerID.DataPropertyName = "PlayerID";
            this.dgvcALLPlayerID.HeaderText = "PlayerID";
            this.dgvcALLPlayerID.Name = "dgvcALLPlayerID";
            this.dgvcALLPlayerID.Visible = false;
            // 
            // dgvcALLName
            // 
            this.dgvcALLName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcALLName.DataPropertyName = "PlayerName";
            this.dgvcALLName.HeaderText = "Name";
            this.dgvcALLName.Name = "dgvcALLName";
            // 
            // dgvcALLCredits
            // 
            this.dgvcALLCredits.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvcALLCredits.DataPropertyName = "Credits";
            this.dgvcALLCredits.HeaderText = "Credits";
            this.dgvcALLCredits.Name = "dgvcALLCredits";
            this.dgvcALLCredits.Width = 77;
            // 
            // tabBOWL
            // 
            this.tabBOWL.Controls.Add(this.dgvBOWL);
            this.tabBOWL.Location = new System.Drawing.Point(4, 22);
            this.tabBOWL.Name = "tabBOWL";
            this.tabBOWL.Size = new System.Drawing.Size(434, 407);
            this.tabBOWL.TabIndex = 3;
            this.tabBOWL.Text = "BOWL";
            this.tabBOWL.UseVisualStyleBackColor = true;
            // 
            // dgvBOWL
            // 
            this.dgvBOWL.AllowUserToDeleteRows = false;
            this.dgvBOWL.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBOWL.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvcBOWLPlayerID,
            this.dgvcBOWLName,
            this.dgvcBOWLCredits});
            this.dgvBOWL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvBOWL.Location = new System.Drawing.Point(0, 0);
            this.dgvBOWL.Name = "dgvBOWL";
            this.dgvBOWL.Size = new System.Drawing.Size(434, 407);
            this.dgvBOWL.TabIndex = 3;
            this.dgvBOWL.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBOWL_CellEndEdit);
            this.dgvBOWL.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvBOWL_EditingControlShowing);
            // 
            // dgvcBOWLPlayerID
            // 
            this.dgvcBOWLPlayerID.DataPropertyName = "PlayerID";
            this.dgvcBOWLPlayerID.HeaderText = "PlayerID";
            this.dgvcBOWLPlayerID.Name = "dgvcBOWLPlayerID";
            this.dgvcBOWLPlayerID.Visible = false;
            // 
            // dgvcBOWLName
            // 
            this.dgvcBOWLName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvcBOWLName.DataPropertyName = "PlayerName";
            this.dgvcBOWLName.HeaderText = "Name";
            this.dgvcBOWLName.Name = "dgvcBOWLName";
            // 
            // dgvcBOWLCredits
            // 
            this.dgvcBOWLCredits.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.dgvcBOWLCredits.DataPropertyName = "Credits";
            this.dgvcBOWLCredits.HeaderText = "Credits";
            this.dgvcBOWLCredits.Name = "dgvcBOWLCredits";
            this.dgvcBOWLCredits.Width = 77;
            // 
            // statusStrip1
            // 
            this.statusStrip1.Location = new System.Drawing.Point(0, 491);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            this.statusStrip1.Size = new System.Drawing.Size(463, 22);
            this.statusStrip1.TabIndex = 7;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // frmTeam
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(463, 513);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControlPlayer);
            this.Controls.Add(this.cboTeam);
            this.Controls.Add(this.label1);
            this.Font = new System.Drawing.Font("Verdana", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "frmTeam";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Team";
            this.Load += new System.EventHandler(this.frmTeam_Load);
            this.tabControlPlayer.ResumeLayout(false);
            this.tabWK.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvWK)).EndInit();
            this.tabBAT.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBAT)).EndInit();
            this.tabALL.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvALL)).EndInit();
            this.tabBOWL.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBOWL)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboTeam;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tabControlPlayer;
        private System.Windows.Forms.TabPage tabWK;
        private System.Windows.Forms.TabPage tabBAT;
        private System.Windows.Forms.TabPage tabALL;
        private System.Windows.Forms.TabPage tabBOWL;
        private System.Windows.Forms.DataGridView dgvWK;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.DataGridView dgvBAT;
        private System.Windows.Forms.DataGridView dgvALL;
        private System.Windows.Forms.DataGridView dgvBOWL;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcBATPlayerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcBATName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcBATCredits;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcALLPlayerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcALLName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcALLCredits;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcBOWLPlayerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcBOWLName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcBOWLCredits;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcWKPlayerID;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcWKName;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvcWKCredits;
    }
}