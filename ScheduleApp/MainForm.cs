using System;
using System.Windows.Forms;

namespace ScheduleApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            lblUser.Text = "Logged in as: " + Session.UserName;
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            using (CustomerForm form = new CustomerForm()) form.ShowDialog(this);
        }

        private void btnAppointments_Click(object sender, EventArgs e)
        {
            using (AppointmentForm form = new AppointmentForm()) form.ShowDialog(this);
        }

        private void btnCalendar_Click(object sender, EventArgs e)
        {
            using (CalendarForm form = new CalendarForm()) form.ShowDialog(this);
        }

        private void btnReports_Click(object sender, EventArgs e)
        {
            using (ReportsForm form = new ReportsForm()) form.ShowDialog(this);
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Close();
        }

    }
}
