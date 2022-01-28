namespace People_Browser
{
    partial class frmMain
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
            this.mnuMain = new System.Windows.Forms.MenuStrip();
            this.mnuConnect = new System.Windows.Forms.ToolStripMenuItem();
            this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer = new System.Windows.Forms.SplitContainer();
            this.personsSplitContainer = new System.Windows.Forms.SplitContainer();
            this.dgvPersons = new System.Windows.Forms.DataGridView();
            this.colId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFamily = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOldFamily = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBirthDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStreet = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHouse = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colBirthCountry = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ctlCurrentPerson = new People.Browser.UI.ctlPerson();
            this.mainStatusStrip = new System.Windows.Forms.StatusStrip();
            this.pbPercentage = new System.Windows.Forms.ToolStripProgressBar();
            this.lblPercentage = new System.Windows.Forms.ToolStripStatusLabel();
            this.lblMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgvAudit = new System.Windows.Forms.DataGridView();
            this.colDateTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSeverity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModule = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMethod = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.mnuMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
            this.splitContainer.Panel1.SuspendLayout();
            this.splitContainer.Panel2.SuspendLayout();
            this.splitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.personsSplitContainer)).BeginInit();
            this.personsSplitContainer.Panel1.SuspendLayout();
            this.personsSplitContainer.Panel2.SuspendLayout();
            this.personsSplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersons)).BeginInit();
            this.mainStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAudit)).BeginInit();
            this.SuspendLayout();
            // 
            // mnuMain
            // 
            this.mnuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuConnect,
            this.mnuExit});
            this.mnuMain.Location = new System.Drawing.Point(0, 0);
            this.mnuMain.Name = "mnuMain";
            this.mnuMain.Size = new System.Drawing.Size(1212, 24);
            this.mnuMain.TabIndex = 2;
            this.mnuMain.Text = "menuStrip1";
            // 
            // mnuConnect
            // 
            this.mnuConnect.Name = "mnuConnect";
            this.mnuConnect.Size = new System.Drawing.Size(64, 20);
            this.mnuConnect.Text = "Connect";
            this.mnuConnect.Click += new System.EventHandler(this.mnuConnect_Click);
            // 
            // mnuExit
            // 
            this.mnuExit.Name = "mnuExit";
            this.mnuExit.Size = new System.Drawing.Size(38, 20);
            this.mnuExit.Text = "Exit";
            this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
            // 
            // splitContainer
            // 
            this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer.Location = new System.Drawing.Point(0, 24);
            this.splitContainer.Name = "splitContainer";
            this.splitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer.Panel1
            // 
            this.splitContainer.Panel1.Controls.Add(this.personsSplitContainer);
            this.splitContainer.Panel1.Controls.Add(this.mainStatusStrip);
            // 
            // splitContainer.Panel2
            // 
            this.splitContainer.Panel2.Controls.Add(this.dgvAudit);
            this.splitContainer.Size = new System.Drawing.Size(1212, 673);
            this.splitContainer.SplitterDistance = 420;
            this.splitContainer.TabIndex = 3;
            // 
            // personsSplitContainer
            // 
            this.personsSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.personsSplitContainer.Location = new System.Drawing.Point(0, 0);
            this.personsSplitContainer.Name = "personsSplitContainer";
            // 
            // personsSplitContainer.Panel1
            // 
            this.personsSplitContainer.Panel1.Controls.Add(this.dgvPersons);
            // 
            // personsSplitContainer.Panel2
            // 
            this.personsSplitContainer.Panel2.Controls.Add(this.ctlCurrentPerson);
            this.personsSplitContainer.Size = new System.Drawing.Size(1212, 398);
            this.personsSplitContainer.SplitterDistance = 888;
            this.personsSplitContainer.TabIndex = 3;
            // 
            // dgvPersons
            // 
            this.dgvPersons.AllowUserToAddRows = false;
            this.dgvPersons.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPersons.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colId,
            this.colFamily,
            this.colOldFamily,
            this.colName,
            this.colBirthDate,
            this.colCity,
            this.colStreet,
            this.colHouse,
            this.colBirthCountry});
            this.dgvPersons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvPersons.Location = new System.Drawing.Point(0, 0);
            this.dgvPersons.Name = "dgvPersons";
            this.dgvPersons.Size = new System.Drawing.Size(888, 398);
            this.dgvPersons.TabIndex = 0;
            this.dgvPersons.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvPersons_MouseDown);
            // 
            // colId
            // 
            this.colId.HeaderText = "מספר זהות";
            this.colId.Name = "colId";
            // 
            // colFamily
            // 
            this.colFamily.HeaderText = "שם משפחה";
            this.colFamily.Name = "colFamily";
            // 
            // colOldFamily
            // 
            this.colOldFamily.HeaderText = "שם משפחה ישן";
            this.colOldFamily.Name = "colOldFamily";
            // 
            // colName
            // 
            this.colName.HeaderText = "שם פרטי";
            this.colName.Name = "colName";
            // 
            // colBirthDate
            // 
            this.colBirthDate.HeaderText = "תאריך לידה";
            this.colBirthDate.Name = "colBirthDate";
            // 
            // colCity
            // 
            this.colCity.HeaderText = "ישוב";
            this.colCity.Name = "colCity";
            // 
            // colStreet
            // 
            this.colStreet.HeaderText = "רחוב";
            this.colStreet.Name = "colStreet";
            // 
            // colHouse
            // 
            this.colHouse.HeaderText = "מספר";
            this.colHouse.Name = "colHouse";
            // 
            // colBirthCountry
            // 
            this.colBirthCountry.HeaderText = "ארץ לידה";
            this.colBirthCountry.Name = "colBirthCountry";
            // 
            // ctlCurrentPerson
            // 
            this.ctlCurrentPerson.cities = null;
            this.ctlCurrentPerson.countries = null;
            this.ctlCurrentPerson.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlCurrentPerson.Location = new System.Drawing.Point(0, 0);
            this.ctlCurrentPerson.Name = "ctlCurrentPerson";
            this.ctlCurrentPerson.Size = new System.Drawing.Size(320, 398);
            this.ctlCurrentPerson.TabIndex = 0;
            // 
            // mainStatusStrip
            // 
            this.mainStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pbPercentage,
            this.lblPercentage,
            this.lblMessage});
            this.mainStatusStrip.Location = new System.Drawing.Point(0, 398);
            this.mainStatusStrip.Name = "mainStatusStrip";
            this.mainStatusStrip.Size = new System.Drawing.Size(1212, 22);
            this.mainStatusStrip.TabIndex = 2;
            this.mainStatusStrip.Text = "statusStrip1";
            // 
            // pbPercentage
            // 
            this.pbPercentage.Name = "pbPercentage";
            this.pbPercentage.Size = new System.Drawing.Size(100, 16);
            // 
            // lblPercentage
            // 
            this.lblPercentage.Name = "lblPercentage";
            this.lblPercentage.Size = new System.Drawing.Size(0, 17);
            // 
            // lblMessage
            // 
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(0, 17);
            // 
            // dgvAudit
            // 
            this.dgvAudit.AllowUserToAddRows = false;
            this.dgvAudit.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAudit.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colDateTime,
            this.colSeverity,
            this.colModule,
            this.colMethod,
            this.colLine,
            this.colMessage});
            this.dgvAudit.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAudit.Location = new System.Drawing.Point(0, 0);
            this.dgvAudit.Name = "dgvAudit";
            this.dgvAudit.Size = new System.Drawing.Size(1212, 249);
            this.dgvAudit.TabIndex = 1;
            // 
            // colDateTime
            // 
            this.colDateTime.HeaderText = "Date/Time";
            this.colDateTime.Name = "colDateTime";
            // 
            // colSeverity
            // 
            this.colSeverity.HeaderText = "Severity";
            this.colSeverity.Name = "colSeverity";
            // 
            // colModule
            // 
            this.colModule.HeaderText = "Module";
            this.colModule.Name = "colModule";
            // 
            // colMethod
            // 
            this.colMethod.HeaderText = "Method";
            this.colMethod.Name = "colMethod";
            // 
            // colLine
            // 
            this.colLine.HeaderText = "Line";
            this.colLine.Name = "colLine";
            // 
            // colMessage
            // 
            this.colMessage.HeaderText = "Message";
            this.colMessage.Name = "colMessage";
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1212, 697);
            this.Controls.Add(this.splitContainer);
            this.Controls.Add(this.mnuMain);
            this.MainMenuStrip = this.mnuMain;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.Text = "People Browswer";
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.mnuMain.ResumeLayout(false);
            this.mnuMain.PerformLayout();
            this.splitContainer.Panel1.ResumeLayout(false);
            this.splitContainer.Panel1.PerformLayout();
            this.splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
            this.splitContainer.ResumeLayout(false);
            this.personsSplitContainer.Panel1.ResumeLayout(false);
            this.personsSplitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.personsSplitContainer)).EndInit();
            this.personsSplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPersons)).EndInit();
            this.mainStatusStrip.ResumeLayout(false);
            this.mainStatusStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAudit)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip mnuMain;
        private System.Windows.Forms.ToolStripMenuItem mnuConnect;
        private System.Windows.Forms.ToolStripMenuItem mnuExit;
        private System.Windows.Forms.SplitContainer splitContainer;
        private System.Windows.Forms.DataGridView dgvAudit;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDateTime;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSeverity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModule;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMethod;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLine;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMessage;
        private System.Windows.Forms.StatusStrip mainStatusStrip;
        private System.Windows.Forms.ToolStripProgressBar pbPercentage;
        private System.Windows.Forms.ToolStripStatusLabel lblPercentage;
        private System.Windows.Forms.ToolStripStatusLabel lblMessage;
        private System.Windows.Forms.SplitContainer personsSplitContainer;
        private System.Windows.Forms.DataGridView dgvPersons;
        private System.Windows.Forms.DataGridViewTextBoxColumn colId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFamily;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOldFamily;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBirthDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStreet;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHouse;
        private System.Windows.Forms.DataGridViewTextBoxColumn colBirthCountry;
        private People.Browser.UI.ctlPerson ctlCurrentPerson;
    }
}

