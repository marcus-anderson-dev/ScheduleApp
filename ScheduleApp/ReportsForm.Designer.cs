namespace ScheduleApp
{
    partial class ReportsForm
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
            this.btnTypes = new System.Windows.Forms.Button();
            this.btnSchedules = new System.Windows.Forms.Button();
            this.btnCustomers = new System.Windows.Forms.Button();
            this.output = new System.Windows.Forms.TextBox();
            this.btnClose = new System.Windows.Forms.Button();
            this.lblUser = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnTypes
            // 
            this.btnTypes.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnTypes.Location = new System.Drawing.Point(10, 10);
            this.btnTypes.Name = "btnTypes";
            this.btnTypes.Size = new System.Drawing.Size(230, 34);
            this.btnTypes.TabIndex = 0;
            this.btnTypes.Text = "Appointment Types by Month";
            this.btnTypes.UseVisualStyleBackColor = true;
            this.btnTypes.Click += new System.EventHandler(this.btnTypes_Click);
            // 
            // btnSchedules
            // 
            this.btnSchedules.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnSchedules.Location = new System.Drawing.Point(250, 10);
            this.btnSchedules.Name = "btnSchedules";
            this.btnSchedules.Size = new System.Drawing.Size(230, 34);
            this.btnSchedules.TabIndex = 1;
            this.btnSchedules.Text = "Schedule for Each User";
            this.btnSchedules.UseVisualStyleBackColor = true;
            this.btnSchedules.Click += new System.EventHandler(this.btnSchedules_Click);
            // 
            // btnCustomers
            // 
            this.btnCustomers.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnCustomers.Location = new System.Drawing.Point(490, 10);
            this.btnCustomers.Name = "btnCustomers";
            this.btnCustomers.Size = new System.Drawing.Size(230, 34);
            this.btnCustomers.TabIndex = 2;
            this.btnCustomers.Text = "Appointments per Customer";
            this.btnCustomers.UseVisualStyleBackColor = true;
            this.btnCustomers.Click += new System.EventHandler(this.btnCustomers_Click);
            // 
            // output
            // 
            this.output.Font = new System.Drawing.Font("Consolas", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.output.Location = new System.Drawing.Point(10, 59);
            this.output.Multiline = true;
            this.output.Name = "output";
            this.output.ReadOnly = true;
            this.output.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.output.Size = new System.Drawing.Size(740, 431);
            this.output.TabIndex = 3;
            this.output.WordWrap = false;
            // 
            // btnClose
            // 
            this.btnClose.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClose.Location = new System.Drawing.Point(650, 505);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(100, 30);
            this.btnClose.TabIndex = 4;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            this.btnClose.Click += new System.EventHandler(this.BtnClose_Click);
            // 
            // lblUser
            // 
            this.lblUser.AutoSize = true;
            this.lblUser.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.lblUser.Font = new System.Drawing.Font("Segoe UI Semibold", 10.2F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUser.Location = new System.Drawing.Point(9, -1);
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
            this.panel1.Location = new System.Drawing.Point(-11, 551);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(773, 25);
            this.panel1.TabIndex = 8;
            // 
            // ReportsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(760, 578);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.btnTypes);
            this.Controls.Add(this.btnSchedules);
            this.Controls.Add(this.btnCustomers);
            this.Controls.Add(this.output);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ReportsForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Reports";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnTypes;
        private System.Windows.Forms.Button btnSchedules;
        private System.Windows.Forms.Button btnCustomers;
        private System.Windows.Forms.TextBox output;
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblUser;
        private System.Windows.Forms.Panel panel1;
    }
}
