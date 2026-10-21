using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ScheduleApp
{
    public partial class CalendarForm : Form
    {
        private List<Appointment> all = new List<Appointment>();

        public CalendarForm()
        {
            InitializeComponent();
        }

        private void CalendarForm_Load(object sender, EventArgs e)
        {

            lblUser.Text = "Logged in as: " + Session.UserName;

            try
            {
                all = AppointmentRepo.GetAll();

                calendar.BoldedDates = all.Select(a => a.StartLocal.Date).Distinct().ToArray();
                calendar.UpdateBoldedDates();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Could not load the appointments.\n\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            ShowAppointments();
        }

        private void calendar_DateChanged(object sender, DateRangeEventArgs e)
        {
            ShowAppointments();
        }

        private void rdoDay_CheckedChanged(object sender, EventArgs e)
        {
            ShowAppointments();
        }

        private void ShowAppointments()
        {
            DateTime day = calendar.SelectionStart.Date;
            List<Appointment> shown;

            if (rdoDay.Checked)
            {
                shown = all.Where(a => a.StartLocal.Date == day).OrderBy(a => a.StartUtc).ToList();
                lblInfo.Text = "Appointments on " + day.ToLongDateString();
            }
            else
            {
                shown = all.Where(a => a.StartLocal.Year == day.Year && a.StartLocal.Month == day.Month).OrderBy(a => a.StartUtc).ToList();
                lblInfo.Text = "Appointments in " + day.ToString("MMMM yyyy");
            }

            grid.DataSource = null;
            grid.DataSource = shown;
            UI.SetHeader(grid, "CustomerName", "Customer");
            UI.SetHeader(grid, "UserName", "User");
            UI.SetHeader(grid, "StartLocal", "Start");
            UI.SetHeader(grid, "EndLocal", "End");
            if (grid.Columns["StartLocal"] != null) grid.Columns["StartLocal"].DefaultCellStyle.Format = "g";
            if (grid.Columns["EndLocal"] != null) grid.Columns["EndLocal"].DefaultCellStyle.Format = "g";
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
