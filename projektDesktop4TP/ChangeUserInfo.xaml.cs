using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace projektDesktop4TP
{
    /// <summary>
    /// Logika interakcji dla klasy ChangeUserInfo.xaml
    /// </summary>
    public partial class ChangeUserInfo : Window
    {
        private const string DatabaseFileName = "UserInfoDatabase.db";
        private const string ConnectionString = "Data Source=" + DatabaseFileName;
        public int person_id= 0;
        public ChangeUserInfo(int id)
        {
            InitializeComponent();
            InitializeDatabase();
            person_id= id;
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CREATE TABLE IF NOT EXISTS UserInfo (Id INTEGER PRIMARY KEY AUTOINCREMENT, Name TEXT, Surname TEXT, Birthday DATE, Email TEXT, PhoneNumber TEXT, Place TEXT, LoginId INTEGER)";
                    command.ExecuteNonQuery();
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string surname = SurnameTextBox.Text;
            DateTime? birthday = BirthdayDatePicker.SelectedDate;
            string email = EmailTextBox.Text;
            string phoneNumber = PhoneNumberTextBox.Text;
            string place = PlaceTextBox.Text;
            int id = person_id;

            

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(surname) || !birthday.HasValue || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(phoneNumber) || string.IsNullOrWhiteSpace(place))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id FROM UserInfo WHERE LoginId = @ID";
                    command.Parameters.AddWithValue("@ID", id);

                    var existingUserId = command.ExecuteScalar();

                    if (existingUserId != null)
                    {
                        // User exists, update the information
                        using (var updateCommand = connection.CreateCommand())
                        {
                            updateCommand.CommandText = "UPDATE UserInfo SET Name = @Name, Surname = @Surname, Birthday = @Birthday, PhoneNumber = @PhoneNumber, Place = @Place, Email = @Email WHERE LoginId = @ID";
                            updateCommand.Parameters.AddWithValue("@Name", name);
                            updateCommand.Parameters.AddWithValue("@Surname", surname);
                            updateCommand.Parameters.AddWithValue("@Birthday", birthday.Value);
                            updateCommand.Parameters.AddWithValue("@Email", email);
                            updateCommand.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                            updateCommand.Parameters.AddWithValue("@Place", place);
                            updateCommand.Parameters.AddWithValue("@ID", id);

                            updateCommand.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        // User does not exist, insert a new record
                        using (var insertCommand = connection.CreateCommand())
                        {
                            insertCommand.CommandText = "INSERT INTO UserInfo (Name, Surname, Birthday, Email, PhoneNumber, Place, LoginId) VALUES (@Name, @Surname, @Birthday, @Email, @PhoneNumber, @Place, @ID)";
                            insertCommand.Parameters.AddWithValue("@Name", name);
                            insertCommand.Parameters.AddWithValue("@Surname", surname);
                            insertCommand.Parameters.AddWithValue("@Birthday", birthday.Value);
                            insertCommand.Parameters.AddWithValue("@Email", email);
                            insertCommand.Parameters.AddWithValue("@PhoneNumber", phoneNumber);
                            insertCommand.Parameters.AddWithValue("@Place", place);
                            insertCommand.Parameters.AddWithValue("@ID", id);

                            insertCommand.ExecuteNonQuery();
                        }
                    }
                }
            }

            MessageBox.Show("User information saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            FirstPage p = new FirstPage(person_id);
            p.Show();
            this.Close();
        }
    }
}

