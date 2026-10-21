using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ScheduleApp
{
    public partial class ReportsForm : Form
    {
        private List<Appointment> Appointments = new List<Appointment>();

        public ReportsForm()
        {
            InitializeComponent();
            lblUser.Text = "Logged in as: " + Session.UserName;
        }

        private bool LoadData()
        {
            try
            {
                Appointments = AppointmentRepo.GetAll();
                return true;
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Unable to load appointments.\n\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private void btnTypes_Click(object sender, EventArgs e)
        {
            ShowTypesByMonth();
        }

        private void btnSchedules_Click(object sender, EventArgs e)
        {
            ShowUserSchedules();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            ShowCustomerTotals();
        }

        // Groups appointments by appointmentment type for each month.
        private void ShowTypesByMonth()
        {
            if (!LoadData()) return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("APPOINTMENT TYPES BY MONTH");
            sb.AppendLine("__________________________");

            //Uses Linq for sorting.
            var months = Appointments
                .GroupBy(a => new { a.StartLocal.Year, a.StartLocal.Month })
                .OrderBy(g => g.Key.Year)
                .ThenBy(g => g.Key.Month)
                .ToList();

            foreach (var month in months)
            {
                sb.AppendLine();
                sb.AppendLine(new DateTime(month.Key.Year, month.Key.Month, 1).ToString("MMMM yyyy"));
                foreach (var type in month.GroupBy(a => a.Type).OrderBy(t => t.Key))
                {
                    sb.AppendLine("   " + type.Key.PadRight(20) + type.Count());
                }
            }

            if (months.Count == 0) sb.AppendLine("No appointments found.");
            output.Text = sb.ToString();
        }

        // Each users schedule in order.
        private void ShowUserSchedules()
        {
            if (!LoadData()) return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Appointments By User");
            sb.AppendLine("____________________");

            var users = Appointments.GroupBy(a => a.UserName).OrderBy(g => g.Key).ToList();

            foreach (var user in users)
            {
                sb.AppendLine();
                sb.AppendLine("User: " + user.Key);
                foreach (Appointment a in user.OrderBy(x => x.StartUtc))
                {
                    sb.AppendLine("   " + a.StartLocal.ToString("yyyy-MM-dd h:mm tt") + " - " + a.EndLocal.ToString("h:mm tt") +
                                  "   " + a.CustomerName + " - " + a.Title + " (" + a.Type + ")");
                }
            }

            if (users.Count == 0) sb.AppendLine("No appointments found.");
            output.Text = sb.ToString();
        }

        // Groups by customer appointment + time each month.
        private void ShowCustomerTotals()
        {
            if (!LoadData()) return;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("APPOINTMENTS PER CUSTOMER");
            sb.AppendLine("_________________________");
            sb.AppendLine();

            var rows = Appointments
                .GroupBy(a => a.CustomerId)
                .Select(g => new
                {
                    Name = g.First().CustomerName,
                    Count = g.Count(),
                    Hours = g.Sum(a => (a.EndUtc - a.StartUtc).TotalHours)
                })
                .OrderByDescending(r => r.Count)
                .ThenBy(r => r.Name)
                .ToList();

            foreach (var row in rows)
            {
                sb.AppendLine(row.Name.PadRight(25) + row.Count.ToString().PadLeft(3) + " appointment(s)   " + row.Hours.ToString("0.0") + " hour(s)");
            }

            if (rows.Count == 0) sb.AppendLine("No appointments found.");
            output.Text = sb.ToString();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
