using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ScheduleApp
{
    public partial class AppointmentForm : Form
    {
        private List<Customer> customers = new List<Customer>();

        public AppointmentForm()
        {
            InitializeComponent();
        }

        private void AppointmentForm_Load(object sender, EventArgs e)
        {
            lblUser.Text = "Logged in as: " + Session.UserName;
            lblTimeZone.Text = "Times are shown in your time zone (" + TimeZoneInfo.Local.Id + ").";
            LoadCustomers();
            LoadGrid();
            ResetFields();
        }

        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            FormFillSubmit();
        }

        private void LoadCustomers()
        {
            try
            {
                customers = CustomerRepo.GetAll();
                cboCustomer.DataSource = customers;
                cboCustomer.DisplayMember = "Name";
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Could not load the customers.\n\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadGrid()
        {
            try
            {
                grid.DataSource = null;
                grid.DataSource = AppointmentRepo.GetAll();

                UI.SetHeader(grid, "CustomerName", "Customer");
                UI.SetHeader(grid, "UserName", "User");
                UI.SetHeader(grid, "StartLocal", "Start");
                UI.SetHeader(grid, "EndLocal", "End");
                if (grid.Columns["StartLocal"] != null) grid.Columns["StartLocal"].DefaultCellStyle.Format = "g";
                if (grid.Columns["EndLocal"] != null) grid.Columns["EndLocal"].DefaultCellStyle.Format = "g";
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Could not load the appointments.\n\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Statement for grid is empty
        private Appointment Selected()
        {
            if (grid.SelectedRows.Count == 0) return null;
            return grid.SelectedRows[0].DataBoundItem as Appointment;
        }

        private void FormFillSubmit()
        {
            Appointment a = Selected();
            if (a == null) return;

            cboCustomer.SelectedItem = customers.FirstOrDefault(c => c.CustomerId == a.CustomerId);
            txtTitle.Text = a.Title;
            cboType.SelectedItem = a.Type;
            txtDescription.Text = a.Description;
            txtLocation.Text = a.Location;
            txtContact.Text = a.Contact;
            txtUrl.Text = a.Url;
            dtpDate.Value = a.StartLocal.Date;
            dtpStart.Value = a.StartLocal;
            dtpEnd.Value = a.EndLocal;
        }

        private void ResetFields()
        {
            grid.ClearSelection();
            txtTitle.Clear();
            txtDescription.Clear();
            txtLocation.Clear();
            txtContact.Clear();
            txtUrl.Clear();
            cboType.SelectedIndex = -1;
            if (cboCustomer.Items.Count > 0) cboCustomer.SelectedIndex = 0;
            dtpDate.Value = DateTime.Today;
            dtpStart.Value = DateTime.Today.AddHours(9);
            dtpEnd.Value = DateTime.Today.AddHours(10);
        }

        // Checks rules vs fields and returns appointment.
        private Appointment ReadAndValidate(int appointmentId, int userId)
        {
            Customer customer = cboCustomer.SelectedItem as Customer;
            if (customer == null) throw new ValidationException("Please choose a customer.");

            string title = txtTitle.Text.Trim();
            string description = txtDescription.Text.Trim();
            string location = txtLocation.Text.Trim();
            string contact = txtContact.Text.Trim();
            string url = txtUrl.Text.Trim();

            txtTitle.Text = title;
            txtDescription.Text = description;
            txtLocation.Text = location;
            txtContact.Text = contact;
            txtUrl.Text = url;

            if (title.Length == 0) throw new ValidationException("A title is required.");
            if (cboType.SelectedItem == null) throw new ValidationException("Please choose an appointment type.");

            // URL validation
            if (url.Length > 0)
            {
                Uri parsed;
                if (!Uri.TryCreate(url, UriKind.Absolute, out parsed) ||
                    (parsed.Scheme != Uri.UriSchemeHttp && parsed.Scheme != Uri.UriSchemeHttps))
                {
                    throw new ValidationException("The URL must be a web address that starts with http:// or https://.");
                }
            }

            DateTime day = dtpDate.Value.Date;
            DateTime startLocal = day + dtpStart.Value.TimeOfDay;
            DateTime endLocal = day + dtpEnd.Value.TimeOfDay;
            if (endLocal <= startLocal) throw new ValidationException("The end time must be after the start time.");

            DateTime startUtc = TimeZoneHelperFunction.ToUtc(startLocal);
            DateTime endUtc = TimeZoneHelperFunction.ToUtc(endLocal);

            // Business Hours Rule.
            if (!TimeZoneHelperFunction.IsWithinBusinessHours(startUtc, endUtc))
                throw new ValidationException("Appointments must be between 9:00 AM and 5:00 PM Eastern Time, Monday through Friday.");

            // Validates overlapping appointment
            if (AppointmentRepo.HasAppointmentOverlap(startUtc, endUtc, customer.CustomerId, userId, appointmentId))
                throw new ValidationException("This appointment overlaps with another appointment for the same user or customer.");

            return new Appointment
            {
                AppointmentId = appointmentId,
                CustomerId = customer.CustomerId,
                UserId = userId,
                Title = title,
                Description = description,
                Location = location,
                Contact = contact,
                Url = url,
                Type = cboType.SelectedItem.ToString(),
                StartUtc = startUtc,
                EndUtc = endUtc
            };
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Appointment a = ReadAndValidate(0, Session.UserId);
                AppointmentRepo.Add(a, Session.UserName);
                LoadGrid();
                ResetFields();
            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message, "Invalid Appointment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("That time does not exist in your time zone (it falls in a daylight saving time change). Please pick another time.",
                    "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("The database could not add the appointment.\n\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something unexpected went wrong while adding the appointment.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Appointment selected = Selected();
                if (selected == null) throw new ValidationException("Select an appointment in the table first.");

                Appointment a = ReadAndValidate(selected.AppointmentId, selected.UserId);
                AppointmentRepo.Update(a, Session.UserName);
                LoadGrid();
            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message, "Invalid Appointment", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (ArgumentException)
            {
                MessageBox.Show("That time does not exist in your time zone (it falls in a daylight saving time change). Please pick another time.",
                    "Invalid Time", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("The database could not update the appointment.\n\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something unexpected went wrong while updating the appointment.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Appointment selected = Selected();
                if (selected == null) throw new ValidationException("Select an appointment in the table first.");

                DialogResult answer = MessageBox.Show("Delete the appointment \"" + selected.Title + "\"?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer != DialogResult.Yes) return;

                AppointmentRepo.Delete(selected.AppointmentId);
                LoadGrid();
                ResetFields();
            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message, "Nothing Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("The database could not delete the appointment.\n\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something unexpected went wrong while deleting the appointment.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ResetFields();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
