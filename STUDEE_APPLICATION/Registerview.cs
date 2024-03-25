using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySqlConnector;


namespace STUDEE_APPLICATION
{
    public partial class Registerview : Form
    {
        MySqlConnection connection = new MySqlConnection("server=localhost;database=studeeapp;port=3306;username=root;password=");
        public Registerview()
        {
            InitializeComponent();
        }

        private void toLogin_Click(object sender, EventArgs e)
        {
            this.Hide();
            Login frm4 = new Login();
            frm4.ShowDialog();

        }

        private void btnRegister_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(fullText.Text) || string.IsNullOrEmpty(userEmail.Text) || string.IsNullOrEmpty(passText.Text))
                {
                    MessageBox.Show("Please fill out all information!", "Error");
                    return;
                }

                connection.Open();

                MySqlCommand cmd1 = new MySqlCommand("SELECT * FROM userdetails WHERE Username = @UserName", connection);

                cmd1.Parameters.AddWithValue("@UserName", userEmail.Text);

                bool userExists = false;

                using (var dr1 = cmd1.ExecuteReader())
                    if (userExists = dr1.HasRows) MessageBox.Show("Username not available!");

                if (!userExists)
                {
                    string iquery = "INSERT INTO userdetails (`ID`, `FirstName`, `Username`,`Occupation`, `Password`,`ProfileImage`) VALUES (NULL, @FullName, @UserName,@Occupation, @Password, @ProfileImage)";
                    MySqlCommand commandDatabase = new MySqlCommand(iquery, connection);
                    commandDatabase.Parameters.AddWithValue("@FullName", fullText.Text);
                    commandDatabase.Parameters.AddWithValue("@Occupation", UserOccupation.Text);
                    commandDatabase.Parameters.AddWithValue("@UserName", userEmail.Text);
                    commandDatabase.Parameters.AddWithValue("@Password", passText.Text);

                    // Convert the image to a byte array and insert it into the database
                    if (btnUploard.Image != null)
                    {
                        byte[] imageData = ImageToByteArray(btnUploard.Image);
                        commandDatabase.Parameters.AddWithValue("@ProfileImage", imageData);
                    }
                    else
                    {
                        commandDatabase.Parameters.AddWithValue("@ProfileImage", DBNull.Value); // If no image is selected, insert null into the database
                    }

                    commandDatabase.CommandTimeout = 60;

                    commandDatabase.ExecuteNonQuery();

                    MessageBox.Show("Account Successfully Created!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred: " + ex.Message, "Error");
            }
            finally
            {
                connection.Close();
            }

        }

        private void btnUploard_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif";
            openFileDialog.Title = "Select an Image File";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                // Display the selected image in the PictureBox
                btnUploard.Image = new Bitmap(openFileDialog.FileName);
            }

        }

        // Method to convert Image to byte array
        private byte[] ImageToByteArray(Image image)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(image, typeof(byte[]));
        }
    }
}
