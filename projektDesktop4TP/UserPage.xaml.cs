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
    /// Logika interakcji dla klasy UserPage.xaml
    /// </summary>
    public partial class UserPage : Window
    {
        private const string DatabaseFileName = "UserInfoDatabase.db";
        private const string ConnectionString = "Data Source=" + DatabaseFileName;
        int userid1= 0;

        public UserPage(int userId)
        {
            InitializeComponent();
            userid1= userId;
            LoadUserData(userid1);

        }

        private void LoadUserData(int userId1)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM UserInfo WHERE LoginId = @UserId";
                    command.Parameters.AddWithValue("@UserId", userId1);

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            NameTextBlock.Text = reader["Name"].ToString();
                            SurnameTextBlock.Text = reader["Surname"].ToString();
                            BirthdayTextBlock.Text = DateTime.Parse(reader["Birthday"].ToString()).ToShortDateString();
                            EmailTextBlock.Text = reader["Email"].ToString();
                            PhoneNumberTextBlock.Text = reader["PhoneNumber"].ToString();
                            PlaceTextBlock.Text = reader["Place"].ToString();
                        }
                    }
                }
            }
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            FirstPage f = new FirstPage(userid1);
            f.Show();
            this.Close();
        }
    }
}
