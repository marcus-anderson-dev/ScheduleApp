using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace ScheduleApp
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {

            string displayLanguage = CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;
            string regionalLanguage = CultureInfo.CurrentCulture.TwoLetterISOLanguageName;
            Lang.Spanish = displayLanguage == "es" || regionalLanguage == "es";

            ApplyLanguage();
        }


        private void ApplyLanguage()
        {
            Text = Lang.T("title");
            lblUser.Text = Lang.T("username");
            lblPass.Text = Lang.T("password");
            btnLogin.Text = Lang.T("login");
            btnExit.Text = Lang.T("exit");

            // Users Location: Computers Region + TimeZone.
            lblLocation.Text = Lang.T("location") + " " + RegionInfo.CurrentRegion.DisplayName +
                               " (" + TimeZoneInfo.Local.Id + ")";
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = txtUser.Text.Trim();
            string password = txtPass.Text;

            if (userName.Length == 0 || password.Length == 0)
            {
                MessageBox.Show(Lang.T("errEmpty"), Lang.T("error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int? userId = UserRepo.Login(userName, password);
                if (userId == null)
                {
                    MessageBox.Show(Lang.T("errMismatch"), Lang.T("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Session.UserId = userId.Value;
                Session.UserName = userName;

                WriteLoginHistory(userName);
                ShowUpcomingAppointmentAlert();

                MainForm main = new MainForm();
                main.FormClosed += (s, args) => Close();   
                Hide();
                main.Show();
            }
            catch (MySqlException ex)
            {
                MessageBox.Show(Lang.T("errDb") + "\n\n" + ex.Message, Lang.T("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(Lang.T("errUnexpected") + "\n\n" + ex.Message, Lang.T("error"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Adds Line to Login_history.txt | File located in bin\Debug folder.
        private void WriteLoginHistory(string userName)
        {
            try
            {
                string path = Path.Combine(Application.StartupPath, "Login_History.txt");
                string line = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - " + userName + Environment.NewLine;
                File.AppendAllText(path, line);
            }
            catch (IOException ex)
            {
                MessageBox.Show(Lang.T("errLog") + "\n\n" + ex.Message, Lang.T("error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (UnauthorizedAccessException ex)
            {
                MessageBox.Show(Lang.T("errLog") + "\n\n" + ex.Message, Lang.T("error"), MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // One alert that uses appointment list for every apointment in the next 15 mins.
        private void ShowUpcomingAppointmentAlert()
        {
            List<Appointment> soon = AppointmentRepo.GetUpcoming(Session.UserId, 15);
            if (soon.Count == 0) return;

            StringBuilder message = new StringBuilder(Lang.T("alertBody"));
            foreach (Appointment a in soon)
            {
                message.AppendLine();
                message.Append(a.StartLocal.ToString("t") + " - " + a.Title + " (" + a.CustomerName + ")");
            }
            MessageBox.Show(message.ToString(), Lang.T("reminder"), MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
