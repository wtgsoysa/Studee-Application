using System;
using System.Data;
using System.Windows.Forms;
//using MySql.Data.MySqlClient;
using MySqlConnector;

namespace STUDEE_APPLICATION
{
    public partial class Login : Form
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;database=studeeapp;port=3306;username=root;password=");
        MySqlCommand command;
        MySqlDataReader mdr;
        public Login()
        {
            InitializeComponent();
        }

        private void toReg_Click(object sender, EventArgs e)
        {
            this.Hide();
            Registerview frm3 = new Registerview();
            frm3.ShowDialog();

        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(userText.Text) || string.IsNullOrEmpty(passText.Text))
                {
                    MessageBox.Show("Please input Username and Password", "Error");
                    return;
                }

                connection.Open();
                string selectQuery = "SELECT * FROM userdetails WHERE Username = @Username AND Password = @Password";
                command = new MySqlCommand(selectQuery, connection);
                command.Parameters.AddWithValue("@Username", userText.Text);
                command.Parameters.AddWithValue("@Password", passText.Text);
                mdr = command.ExecuteReader();
                if (mdr.Read())
                {
                    MessageBox.Show("Login Successful!");
                    this.Hide();
                    string loggedInUsername = userText.Text; // Replace with the actual username obtained during login
                    LoginSuccess loginSuccessForm = new LoginSuccess(loggedInUsername);
                    loginSuccessForm.ShowDialog();

                }
                else
                {
                    MessageBox.Show("Incorrect Login Information! Try again.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error");
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }

        }
    }
}
