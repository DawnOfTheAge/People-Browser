﻿namespace People.Browser.UI
{
    partial class FrmSearch
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
            this.ctlSearchFilter = new People.Browser.UI.CtlPerson();
            this.SuspendLayout();
            // 
            // ctlSearchFilter
            // 
            this.ctlSearchFilter.cities = null;
            this.ctlSearchFilter.countries = null;
            this.ctlSearchFilter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ctlSearchFilter.Location = new System.Drawing.Point(0, 0);
            this.ctlSearchFilter.Name = "ctlSearchFilter";
            this.ctlSearchFilter.Size = new System.Drawing.Size(328, 501);
            this.ctlSearchFilter.TabIndex = 0;
            // 
            // frmSearch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 501);
            this.Controls.Add(this.ctlSearchFilter);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Name = "frmSearch";
            this.Text = "Search";
            this.Load += new System.EventHandler(this.FrmSearch_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private CtlPerson ctlSearchFilter;
    }
}