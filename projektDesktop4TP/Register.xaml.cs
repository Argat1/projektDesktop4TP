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
    /// Logika interakcji dla klasy Register.xaml
    /// </summary>
    public partial class Register : Window
    {
        private const string DatabaseFileName = "LoginDatabase.db";
        private const string ConnectionString = "Data Source=" + DatabaseFileName;
        private int type = 0;

        public Register(int i)
        {
            InitializeComponent();

            InitializeDatabase();
            type= i;
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CREATE TABLE IF NOT EXISTS Users (Id INTEGER PRIMARY KEY AUTOINCREMENT, Username TEXT, Password TEXT, UserType TEXT)";
                    command.ExecuteNonQuery();
                }
            }
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = passwordb.Password;
            string userType = "";
            if (type == 0)
            {
                userType = "employee";
            }
            else if (type == 1)
            {
                userType = "employer";
            }


            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(userType))
            {
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO Users (Username, Password, UserType) VALUES (@Username, @Password, @UserType)";
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);
                    command.Parameters.AddWithValue("@UserType", userType);

                    command.ExecuteNonQuery();
                }
            }

            MessageBox.Show("Registration successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Users WHERE Username = @Username AND Password = @Password";
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@Password", password);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            int y = int.Parse(reader["Id"].ToString());
                            FirstPage f = new FirstPage(y);
                            f.Show();
                            this.Close();
                        }
                    }
                }
            }
                   
        }
    }
}
