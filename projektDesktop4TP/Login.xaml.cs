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
    /// Logika interakcji dla klasy Login.xaml
    /// </summary>
    public partial class Login : Window
    {

        private const string DatabaseFileName = "LoginDatabase.db";
        private const string ConnectionString = "Data Source=" + DatabaseFileName;


        public Login()
        {
            InitializeComponent();

            InitializeDatabase();

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

        

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = passwordb.Password;

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
                            string userType = reader["UserType"].ToString();
                            MessageBox.Show($"Login successful! User type: {userType}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            int y = int.Parse(reader["Id"].ToString());
                            FirstPage f = new FirstPage(y);
                            f.Show();
                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("Invalid username or password.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }

                    }
                }
            }
        }
    }
}
