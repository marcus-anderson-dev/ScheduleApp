using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ScheduleApp
{
    public partial class CustomerForm : Form
    {
        public CustomerForm()
        {
            InitializeComponent();
        }

        private void CustomerForm_Load(object sender, EventArgs e)
        {
            LoadGrid();
            lblUser.Text = "Logged in as: " + Session.UserName;
        }

        private void Grid_SelectionChanged(object sender, EventArgs e)
        {
            FormFillSubmit();
        }

        private void LoadGrid()
        {
            try
            {
                grid.DataSource = null;
                grid.DataSource = CustomerRepo.GetAll();
                UI.SetHeader(grid, "Address2", "Address 2");
                UI.SetHeader(grid, "PostalCode", "Postal Code");
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Could not load the customers.\n\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // The customer whose row is highlighted in the table (null if none).
        private Customer Selected()
        {
            if (grid.SelectedRows.Count == 0) return null;
            return grid.SelectedRows[0].DataBoundItem as Customer;
        }

        private void FormFillSubmit()
        {
            Customer c = Selected();
            if (c == null) return;
            txtName.Text = c.Name;
            txtAddress.Text = c.Address;
            txtAddress2.Text = c.Address2;
            txtCity.Text = c.City;
            txtCountry.Text = c.Country;
            txtPostal.Text = c.PostalCode;
            txtPhone.Text = c.Phone;
            chkActive.Checked = c.Active;
        }

        private void ClearFields()
        {
            grid.ClearSelection();
            txtName.Clear();
            txtAddress.Clear();
            txtAddress2.Clear();
            txtCity.Clear();
            txtCountry.Clear();
            txtPostal.Clear();
            txtPhone.Clear();
            chkActive.Checked = true;
            txtName.Focus();
        }

        // Trims every field, makes sure the required ones are not empty, and checks the phone format.
        private Customer ReadAndValidate()
        {
            Customer c = new Customer
            {
                Name = txtName.Text.Trim(),
                Address = txtAddress.Text.Trim(),
                Address2 = txtAddress2.Text.Trim(),
                City = txtCity.Text.Trim(),
                Country = txtCountry.Text.Trim(),
                PostalCode = txtPostal.Text.Trim(),
                Phone = txtPhone.Text.Trim(),
                Active = chkActive.Checked
            };

            // Shows Trimmed text to user
            txtName.Text = c.Name;
            txtAddress.Text = c.Address;
            txtAddress2.Text = c.Address2;
            txtCity.Text = c.City;
            txtCountry.Text = c.Country;
            txtPostal.Text = c.PostalCode;
            txtPhone.Text = c.Phone;

            if (c.Name.Length == 0) throw new ValidationException("Customer name is required.");
            if (c.Address.Length == 0) throw new ValidationException("Address is required.");
            if (c.City.Length == 0) throw new ValidationException("City is required.");
            if (c.Country.Length == 0) throw new ValidationException("Country is required.");
            if (c.PostalCode.Length == 0) throw new ValidationException("Postal code is required.");
            if (c.Phone.Length == 0) throw new ValidationException("Phone number is required.");

            // Only allows digits and dashes
            if (!Regex.IsMatch(c.Phone, @"^[0-9\-]+$"))
                throw new ValidationException("The phone number can only contain digits and dashes (for example 555-123-4567).");

            return c;
        }

        private void BtnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                Customer c = ReadAndValidate();
                if (CustomerRepo.IsDuplicateCustomer(c))
                    throw new ValidationException("Customer already exisits");

                CustomerRepo.Add(c, Session.UserName);
                LoadGrid();
                ClearFields();
            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message, "Customer not valid", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("The database unable to add customer.\n\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something unexpected happened while adding customer.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                Customer selected = Selected();
                if (selected == null) throw new ValidationException("Select a customer in the table first.");

                Customer c = ReadAndValidate();
                c.CustomerId = selected.CustomerId;
                c.AddressId = selected.AddressId;

                if (CustomerRepo.IsDuplicateCustomer(c))
                    throw new ValidationException("Another customer already has this name, address and phone number.");

                CustomerRepo.Update(c, Session.UserName);
                LoadGrid();
            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message, "Invalid Customer", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("The database could not update the customer.\n\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something unexpected went wrong while updating the customer.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Customer selected = Selected();
                if (selected == null) throw new ValidationException("Oops, forgot to select a customer.");

                DialogResult answer = MessageBox.Show(
                    "Remove " + selected.Name + " and all of their appointments?", "Confirm Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (answer != DialogResult.Yes) return;

                CustomerRepo.Delete(selected);
                LoadGrid();
                ClearFields();
            }
            catch (ValidationException ex)
            {
                MessageBox.Show(ex.Message, "Nothing Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("The database could not delete the customer.\n\n" + ex.Message, "Database Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Something unexpected went wrong while deleting the customer.\n\n" + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void BtnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
