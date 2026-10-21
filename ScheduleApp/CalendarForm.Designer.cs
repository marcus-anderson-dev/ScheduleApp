namespace ScheduleApp
{
    partial class CalendarForm
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
            this.calendar = new System.Windows.Forms.MonthCalendar();
            this.rdoDay = new System.Windows.Forms.RadioButton();
            this.rdoMonth = new System.Windows.Forms.RadioButton();
            this.lblBold = new System.Windows.Forms.Label();
            this.lblInfo = new System.Windows.Forms.Label();
            this.grid = new System.Windows.Forms.DataGridView();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblUser = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // calendar
            // 
            this.calendar.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.calendar.Location = new System.Drawing.Point(10, 10);
            this.calendar.MaxSelectionCount = 1;
            this.calendar.Name = "calendar";
            this.calendar.TabIndex = 0;
            this.calendar.DateChanged += new System.Windows.Forms.DateRangeEventHandler(this.calendar_DateChanged);
            // 
            // rdoDay
            // 
            this.rdoDay.AutoSize = true;
            this.rdoDay.Checked = true;
            this.rdoDay.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoDay.Location = new System.Drawing.Point(10, 213);
            this.rdoDay.Name = "rdoDay";
            this.rdoDay.Size = new System.Drawing.Size(138, 21);
            this.rdoDay.TabIndex = 1;
            this.rdoDay.TabStop = true;
            this.rdoDay.Text = "Show selected day";
            this.rdoDay.UseVisualStyleBackColor = true;
            this.rdoDay.CheckedChanged += new System.EventHandler(this.rdoDay_CheckedChanged);
            // 
            // rdoMonth
            // 
            this.rdoMonth.AutoSize = true;
            this.rdoMonth.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rdoMonth.Location = new System.Drawing.Point(10, 244);
            this.rdoMonth.Name = "rdoMonth";
            this.rdoMonth.Size = new System.Drawing.Size(144, 21);
            this.rdoMonth.TabIndex = 2;
            this.rdoMonth.Text = "Show whole month";
            this.rdoMonth.UseVisualStyleBackColor = true;
            // 
            // lblBold
            // 
            this.lblBold.AutoSize = true;
            this.lblBold.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBold.Location = new System.Drawing.Point(12, 181);
            this.lblBold.Name = "lblBold";
            this.lblBold.Size = new System.Drawing.Size(190, 17);
            this.lblBold.TabIndex = 3;
            this.lblBold.Text = "Bold dates have appointments.";
            // 
            // lblInfo
            // 
            this.lblInfo.AutoSize = true;
            this.lblInfo.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblInfo.Location = new System.Drawing.Point(281, 10);
            this.lblInfo.Name = "lblInfo";
            this.lblInfo.Size = new System.Drawing.Size(83, 15);
            this.lblInfo.TabIndex = 4;
            this.lblInfo.Text = "Appointments";
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Location = new System.Drawing.Point(284, 46);
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RowHeadersVisible = false;
            this.grid.RowHeadersWidth = 51;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.Size = new System.Drawing.Size(630, 335);
            this.grid.TabIndex = 5;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(814, 398);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 30);
            this.btnClose.TabIndex = 6;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.Location = new System.Drawing.Point(17, -1);
            this.lblUser.Name = "lblUser";
            this.lblUser.Size = new System.Drawing.Size(91, 19);
            this.lblUser.TabIndex = 0;
            this.lblUser.Text = "Logged in as:";
            this.lblUser.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.lblUser);
            this.panel1.Location = new System.Drawing.Point(-18, 461);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(964, 28);
            this.panel1.TabIndex = 8;
            // 
            // CalendarForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(942, 485);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.calendar);
            this.Controls.Add(this.rdoDay);
            this.Controls.Add(this.rdoMonth);
            this.Controls.Add(this.lblBold);
            this.Controls.Add(this.lblInfo);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CalendarForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Calendar";
            this.Load += new System.EventHandler(this.CalendarForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MonthCalendar calendar;
        private System.Windows.Forms.RadioButton rdoDay;
        private System.Windows.Forms.RadioButton rdoMonth;
        private System.Windows.Forms.Label lblBold;
        private System.Windows.Forms.Label lblInfo;
        private System.Windows.Forms.DataGridView grid;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Panel panel1;
    }
}
