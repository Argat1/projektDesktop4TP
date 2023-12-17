using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO.Pipes;
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
    /// Logika interakcji dla klasy FirstPage.xaml
    /// </summary>
    public partial class FirstPage : Window
    {
        private const string DatabaseFileName = "LoginDatabase.db";
        private const string ConnectionString = "Data Source=" + DatabaseFileName;
        int person_id = 0;
        public FirstPage(int i)
        {
            InitializeComponent();

            person_id = i;

            

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM Users WHERE Id = @id";
                    command.Parameters.AddWithValue("@id", person_id);


                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string y = reader["Username"].ToString();
                            UsernameTextBlockContent.Text = y;

                            
                        }
                    }
                }
            }


        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow m = new MainWindow();
            m.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UserPage p = new UserPage(person_id);
            p.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            ChangeUserInfo c = new ChangeUserInfo(person_id);
            c.Show();
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            JobbWindow j = new JobbWindow(person_id);
            j.Show();
            this.Close();
        }
    }
}
