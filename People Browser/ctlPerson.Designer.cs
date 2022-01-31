namespace People.Browser.UI
{
    partial class ctlPerson
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.lblName = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblHouse = new System.Windows.Forms.Label();
            this.lblStreet = new System.Windows.Forms.Label();
            this.lblCity = new System.Windows.Forms.Label();
            this.lblBirthDate = new System.Windows.Forms.Label();
            this.lblFamily = new System.Windows.Forms.Label();
            this.lblOldFamily = new System.Windows.Forms.Label();
            this.txtOldFamily = new System.Windows.Forms.TextBox();
            this.txtFamily = new System.Windows.Forms.TextBox();
            this.gbName = new System.Windows.Forms.GroupBox();
            this.txtStreet = new System.Windows.Forms.TextBox();
            this.gbAddress = new System.Windows.Forms.GroupBox();
            this.nudHouse = new System.Windows.Forms.NumericUpDown();
            this.cboCity = new System.Windows.Forms.ComboBox();
            this.lblCountry = new System.Windows.Forms.Label();
            this.cboCountry = new System.Windows.Forms.ComboBox();
            this.btn = new System.Windows.Forms.Button();
            this.cboSex = new System.Windows.Forms.ComboBox();
            this.lblSex = new System.Windows.Forms.Label();
            this.cboYear = new System.Windows.Forms.ComboBox();
            this.cboMonth = new System.Windows.Forms.ComboBox();
            this.cboDay = new System.Windows.Forms.ComboBox();
            this.gbName.SuspendLayout();
            this.gbAddress.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHouse)).BeginInit();
            this.SuspendLayout();
            // 
            // lblName
            // 
            this.lblName.AutoSize = true;
            this.lblName.Location = new System.Drawing.Point(211, 31);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(51, 13);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "שם פרטי";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(14, 28);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(147, 20);
            this.txtName.TabIndex = 1;
            // 
            // lblHouse
            // 
            this.lblHouse.AutoSize = true;
            this.lblHouse.Location = new System.Drawing.Point(237, 78);
            this.lblHouse.Name = "lblHouse";
            this.lblHouse.Size = new System.Drawing.Size(26, 13);
            this.lblHouse.TabIndex = 5;
            this.lblHouse.Text = "בית";
            // 
            // lblStreet
            // 
            this.lblStreet.AutoSize = true;
            this.lblStreet.Location = new System.Drawing.Point(231, 55);
            this.lblStreet.Name = "lblStreet";
            this.lblStreet.Size = new System.Drawing.Size(32, 13);
            this.lblStreet.TabIndex = 6;
            this.lblStreet.Text = "רחוב";
            // 
            // lblCity
            // 
            this.lblCity.AutoSize = true;
            this.lblCity.Location = new System.Drawing.Point(230, 32);
            this.lblCity.Name = "lblCity";
            this.lblCity.Size = new System.Drawing.Size(33, 13);
            this.lblCity.TabIndex = 7;
            this.lblCity.Text = "ישוב";
            // 
            // lblBirthDate
            // 
            this.lblBirthDate.AutoSize = true;
            this.lblBirthDate.Location = new System.Drawing.Point(224, 266);
            this.lblBirthDate.Name = "lblBirthDate";
            this.lblBirthDate.Size = new System.Drawing.Size(69, 13);
            this.lblBirthDate.TabIndex = 8;
            this.lblBirthDate.Text = "תאריך לידה";
            // 
            // lblFamily
            // 
            this.lblFamily.AutoSize = true;
            this.lblFamily.Location = new System.Drawing.Point(199, 79);
            this.lblFamily.Name = "lblFamily";
            this.lblFamily.Size = new System.Drawing.Size(63, 13);
            this.lblFamily.TabIndex = 9;
            this.lblFamily.Text = "שם משפחה";
            // 
            // lblOldFamily
            // 
            this.lblOldFamily.AutoSize = true;
            this.lblOldFamily.Location = new System.Drawing.Point(177, 55);
            this.lblOldFamily.Name = "lblOldFamily";
            this.lblOldFamily.Size = new System.Drawing.Size(85, 13);
            this.lblOldFamily.TabIndex = 10;
            this.lblOldFamily.Text = "שם משפחה ישן";
            // 
            // txtOldFamily
            // 
            this.txtOldFamily.Location = new System.Drawing.Point(14, 52);
            this.txtOldFamily.Name = "txtOldFamily";
            this.txtOldFamily.Size = new System.Drawing.Size(147, 20);
            this.txtOldFamily.TabIndex = 11;
            // 
            // txtFamily
            // 
            this.txtFamily.Location = new System.Drawing.Point(14, 76);
            this.txtFamily.Name = "txtFamily";
            this.txtFamily.Size = new System.Drawing.Size(147, 20);
            this.txtFamily.TabIndex = 12;
            // 
            // gbName
            // 
            this.gbName.Controls.Add(this.txtFamily);
            this.gbName.Controls.Add(this.lblName);
            this.gbName.Controls.Add(this.txtOldFamily);
            this.gbName.Controls.Add(this.txtName);
            this.gbName.Controls.Add(this.lblOldFamily);
            this.gbName.Controls.Add(this.lblFamily);
            this.gbName.Location = new System.Drawing.Point(13, 13);
            this.gbName.Name = "gbName";
            this.gbName.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gbName.Size = new System.Drawing.Size(286, 114);
            this.gbName.TabIndex = 13;
            this.gbName.TabStop = false;
            this.gbName.Text = "שם";
            // 
            // txtStreet
            // 
            this.txtStreet.Location = new System.Drawing.Point(13, 52);
            this.txtStreet.Name = "txtStreet";
            this.txtStreet.Size = new System.Drawing.Size(211, 20);
            this.txtStreet.TabIndex = 14;
            // 
            // gbAddress
            // 
            this.gbAddress.Controls.Add(this.nudHouse);
            this.gbAddress.Controls.Add(this.cboCity);
            this.gbAddress.Controls.Add(this.txtStreet);
            this.gbAddress.Controls.Add(this.lblHouse);
            this.gbAddress.Controls.Add(this.lblStreet);
            this.gbAddress.Controls.Add(this.lblCity);
            this.gbAddress.Location = new System.Drawing.Point(13, 133);
            this.gbAddress.Name = "gbAddress";
            this.gbAddress.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.gbAddress.Size = new System.Drawing.Size(286, 114);
            this.gbAddress.TabIndex = 15;
            this.gbAddress.TabStop = false;
            this.gbAddress.Text = "כתובת";
            // 
            // nudHouse
            // 
            this.nudHouse.Location = new System.Drawing.Point(158, 76);
            this.nudHouse.Name = "nudHouse";
            this.nudHouse.Size = new System.Drawing.Size(67, 20);
            this.nudHouse.TabIndex = 16;
            // 
            // cboCity
            // 
            this.cboCity.FormattingEnabled = true;
            this.cboCity.Location = new System.Drawing.Point(13, 29);
            this.cboCity.Name = "cboCity";
            this.cboCity.Size = new System.Drawing.Size(211, 21);
            this.cboCity.Sorted = true;
            this.cboCity.TabIndex = 15;
            // 
            // lblCountry
            // 
            this.lblCountry.AutoSize = true;
            this.lblCountry.Location = new System.Drawing.Point(234, 295);
            this.lblCountry.Name = "lblCountry";
            this.lblCountry.Size = new System.Drawing.Size(59, 13);
            this.lblCountry.TabIndex = 16;
            this.lblCountry.Text = "ארץ מוצא";
            // 
            // cboCountry
            // 
            this.cboCountry.FormattingEnabled = true;
            this.cboCountry.Location = new System.Drawing.Point(21, 292);
            this.cboCountry.Name = "cboCountry";
            this.cboCountry.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboCountry.Size = new System.Drawing.Size(197, 21);
            this.cboCountry.Sorted = true;
            this.cboCountry.TabIndex = 17;
            // 
            // btn
            // 
            this.btn.Location = new System.Drawing.Point(206, 352);
            this.btn.Name = "btn";
            this.btn.Size = new System.Drawing.Size(105, 32);
            this.btn.TabIndex = 18;
            this.btn.UseVisualStyleBackColor = true;
            this.btn.Visible = false;
            this.btn.Click += new System.EventHandler(this.btn_Click);
            // 
            // cboSex
            // 
            this.cboSex.FormattingEnabled = true;
            this.cboSex.Items.AddRange(new object[] {
            "זכר",
            "נקבה"});
            this.cboSex.Location = new System.Drawing.Point(156, 319);
            this.cboSex.Name = "cboSex";
            this.cboSex.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboSex.Size = new System.Drawing.Size(62, 21);
            this.cboSex.Sorted = true;
            this.cboSex.TabIndex = 19;
            // 
            // lblSex
            // 
            this.lblSex.AutoSize = true;
            this.lblSex.Location = new System.Drawing.Point(269, 322);
            this.lblSex.Name = "lblSex";
            this.lblSex.Size = new System.Drawing.Size(24, 13);
            this.lblSex.TabIndex = 20;
            this.lblSex.Text = "מין";
            // 
            // cboYear
            // 
            this.cboYear.FormattingEnabled = true;
            this.cboYear.Items.AddRange(new object[] {
            "זכר",
            "נקבה"});
            this.cboYear.Location = new System.Drawing.Point(156, 263);
            this.cboYear.Name = "cboYear";
            this.cboYear.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboYear.Size = new System.Drawing.Size(62, 21);
            this.cboYear.Sorted = true;
            this.cboYear.TabIndex = 21;
            // 
            // cboMonth
            // 
            this.cboMonth.FormattingEnabled = true;
            this.cboMonth.Items.AddRange(new object[] {
            "זכר",
            "נקבה"});
            this.cboMonth.Location = new System.Drawing.Point(79, 263);
            this.cboMonth.Name = "cboMonth";
            this.cboMonth.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboMonth.Size = new System.Drawing.Size(39, 21);
            this.cboMonth.Sorted = true;
            this.cboMonth.TabIndex = 22;
            // 
            // cboDay
            // 
            this.cboDay.FormattingEnabled = true;
            this.cboDay.Items.AddRange(new object[] {
            "זכר",
            "נקבה"});
            this.cboDay.Location = new System.Drawing.Point(21, 263);
            this.cboDay.Name = "cboDay";
            this.cboDay.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.cboDay.Size = new System.Drawing.Size(39, 21);
            this.cboDay.Sorted = true;
            this.cboDay.TabIndex = 23;
            // 
            // ctlPerson
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cboDay);
            this.Controls.Add(this.cboMonth);
            this.Controls.Add(this.cboYear);
            this.Controls.Add(this.lblSex);
            this.Controls.Add(this.cboSex);
            this.Controls.Add(this.btn);
            this.Controls.Add(this.cboCountry);
            this.Controls.Add(this.lblCountry);
            this.Controls.Add(this.gbAddress);
            this.Controls.Add(this.gbName);
            this.Controls.Add(this.lblBirthDate);
            this.Name = "ctlPerson";
            this.Size = new System.Drawing.Size(314, 387);
            this.Load += new System.EventHandler(this.ctlPerson_Load);
            this.gbName.ResumeLayout(false);
            this.gbName.PerformLayout();
            this.gbAddress.ResumeLayout(false);
            this.gbAddress.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudHouse)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label lblHouse;
        private System.Windows.Forms.Label lblStreet;
        private System.Windows.Forms.Label lblCity;
        private System.Windows.Forms.Label lblBirthDate;
        private System.Windows.Forms.Label lblFamily;
        private System.Windows.Forms.Label lblOldFamily;
        private System.Windows.Forms.TextBox txtOldFamily;
        private System.Windows.Forms.TextBox txtFamily;
        private System.Windows.Forms.GroupBox gbName;
        private System.Windows.Forms.TextBox txtStreet;
        private System.Windows.Forms.GroupBox gbAddress;
        private System.Windows.Forms.NumericUpDown nudHouse;
        private System.Windows.Forms.ComboBox cboCity;
        private System.Windows.Forms.Label lblCountry;
        private System.Windows.Forms.ComboBox cboCountry;
        private System.Windows.Forms.Button btn;
        private System.Windows.Forms.ComboBox cboSex;
        private System.Windows.Forms.Label lblSex;
        private System.Windows.Forms.ComboBox cboYear;
        private System.Windows.Forms.ComboBox cboMonth;
        private System.Windows.Forms.ComboBox cboDay;
    }
}
