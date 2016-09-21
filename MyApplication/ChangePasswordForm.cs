using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MyApplication
{
    public partial class ChangePasswordForm : Infrastructure.BaseForm
    {
        public ChangePasswordForm()
        {
            InitializeComponent();
        }

        private void ChangePasswordForm_Load(object sender, EventArgs e)
        {
            Models.DatabaseContext oDataBaseContext = null;

            try
            {
                oDataBaseContext =
                    new Models.DatabaseContext();

                Models.User oUser =
                    oDataBaseContext.Users
                    .Where(current => current.Id == Infrastructure.Utility.AuthenticatedUser.Id)
                    .FirstOrDefault();

                if (oUser == null)
                {
                    Application.Exit();
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
            finally
            {
                if (oDataBaseContext != null)
                {
                    oDataBaseContext.Dispose();
                    oDataBaseContext = null;
                }
            }
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            Models.DatabaseContext oDataBaseContext = null;

            try
            {
                oDataBaseContext =
                    new Models.DatabaseContext();

                Models.User oUser =
                    oDataBaseContext.Users
                    .Where(current => current.Id == Infrastructure.Utility.AuthenticatedUser.Id)
                    .FirstOrDefault();

                if (oUser == null)
                {
                    Application.Exit();
                }

                    if (oUser.Password != OldPasswordTextBox.Text)
                    {
                        MessageBox.Show("you have not entered your old password correctly !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                    }

                        if (NewPasswordTextBox.Text != ConfirmNewPasswordTextBox.Text)
                        {
                            MessageBox.Show("new password and confirm new password box values must be the same !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    return;
                        }

                            if (oUser.Password == NewPasswordTextBox.Text || oUser.Password == ConfirmNewPasswordTextBox.Text)
                            {
                                MessageBox.Show("You cannot use your old password as new password !");

                    return;
                            }

                            // **************************************************
                            if ((string.IsNullOrWhiteSpace(NewPasswordTextBox.Text)) ||
                                (string.IsNullOrWhiteSpace(ConfirmNewPasswordTextBox.Text)))
                            {
                                MessageBox.Show("New Password is Required!");

                    return;
                            }

                                string strErrorMessages = string.Empty;

                                if (NewPasswordTextBox.Text.Length < 8 || ConfirmNewPasswordTextBox.Text.Length < 8)
                                {
                                    if (strErrorMessages != string.Empty)
                                    {
                                        strErrorMessages +=
                                            System.Environment.NewLine;
                                    }

                                    strErrorMessages +=
                                        "Password length should be at least 8 characters!";
                                }

                                // اگر خطایی وجود داشت
                                if (strErrorMessages != string.Empty)
                                {
                                    System.Windows.Forms.MessageBox.Show(strErrorMessages);

                                    return;
                                }

                                oUser.Password = NewPasswordTextBox.Text;

                                oDataBaseContext.SaveChanges();

                                Infrastructure.Utility.AuthenticatedUser = oUser;

                                MessageBox.Show("Your Password was changed successfully !");
                            }
                                    
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }

            finally
            {
                if (oDataBaseContext != null)
                {
                    oDataBaseContext.Dispose();
                    oDataBaseContext = null;
                }
            }
        }

        private void ResetButton_Click(object sender, EventArgs e)
        {
            OldPasswordTextBox.Text = string.Empty;
            NewPasswordTextBox.Text = string.Empty;
            ConfirmNewPasswordTextBox.Text = string.Empty;
        }
    }
}
