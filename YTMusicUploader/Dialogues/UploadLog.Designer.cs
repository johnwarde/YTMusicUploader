﻿namespace YTMusicUploader.Dialogues
{
    partial class UploadLog
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UploadLog));
            this.pnlUploads = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.pbIssueLogIcon = new System.Windows.Forms.PictureBox();
            this.dgvUploads = new JBToolkit.WinForms.FastScrollDataGridView();
            this.pnlUploads.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbIssueLogIcon)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUploads)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlUploads
            // 
            this.pnlUploads.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlUploads.Controls.Add(this.dgvUploads);
            this.pnlUploads.Location = new System.Drawing.Point(26, 76);
            this.pnlUploads.Name = "pnlUploads";
            this.pnlUploads.Size = new System.Drawing.Size(988, 701);
            this.pnlUploads.TabIndex = 1;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(73, 34);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(115, 13);
            this.lblTitle.TabIndex = 3;
            this.lblTitle.Text = "Upload Log. Loading...";
            // 
            // pbIssueLogIcon
            // 
            this.pbIssueLogIcon.Image = global::YTMusicUploader.Properties.Resources.happy;
            this.pbIssueLogIcon.Location = new System.Drawing.Point(23, 20);
            this.pbIssueLogIcon.Name = "pbIssueLogIcon";
            this.pbIssueLogIcon.Size = new System.Drawing.Size(40, 40);
            this.pbIssueLogIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbIssueLogIcon.TabIndex = 2;
            this.pbIssueLogIcon.TabStop = false;
            // 
            // dgvUploads
            // 
            this.dgvUploads.AllowUserToAddRows = false;
            this.dgvUploads.AllowUserToDeleteRows = false;
            this.dgvUploads.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.dgvUploads.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvUploads.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUploads.BackgroundColor = System.Drawing.Color.White;
            this.dgvUploads.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvUploads.DisableScroll = false;
            this.dgvUploads.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvUploads.Location = new System.Drawing.Point(0, 0);
            this.dgvUploads.Name = "dgvUploads";
            this.dgvUploads.ReadOnly = true;
            this.dgvUploads.RowHeadersVisible = false;
            this.dgvUploads.Size = new System.Drawing.Size(988, 701);
            this.dgvUploads.TabIndex = 0;
            // 
            // UploadLog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1037, 800);
            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.pbIssueLogIcon);
            this.Controls.Add(this.pnlUploads);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(920, 800);
            this.Name = "UploadLog";
            this.Style = MetroFramework.MetroColorStyle.Green;
            this.Load += new System.EventHandler(this.IssueLog_Load);
            this.pnlUploads.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbIssueLogIcon)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvUploads)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel pnlUploads;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.PictureBox pbIssueLogIcon;
        private JBToolkit.WinForms.FastScrollDataGridView dgvUploads;
    }
}