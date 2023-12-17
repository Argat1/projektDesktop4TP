using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Logika interakcji dla klasy JobbWindow.xaml
    /// </summary>
    public partial class JobbWindow : Window
    {
        private const string DatabaseFileName = "JobAdsDatabase.db";
        private const string ConnectionString = "Data Source=" + DatabaseFileName;
        private const string DatabaseFileName2 = "LoginDatabase.db";
        private const string ConnectionString2 = "Data Source=" + DatabaseFileName2;
        public int person_id=0;
        public ObservableCollection<JobAdd> JobAds { get; set; } = new ObservableCollection<JobAdd>();

        public JobbWindow(int i)
        {
            InitializeComponent();
            InitializeDatabase();
            LoadJobAds();
            DataContext = this;
            person_id = i;

            using (var connection = new SqliteConnection(ConnectionString2))
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
                            string y = reader["UserType"].ToString();
                            if (y == "employer")
                            {
                                Employer_only.Visibility= Visibility.Visible;
                            }


                        }
                    }
                }
            }
        }



        private void JobAdsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            

            if (e.AddedItems.Count > 0)
            {
                var selectedJobAd = e.AddedItems[0] as JobAdd;
                

                if (selectedJobAd != null)
                {
                    ShowJobAdDetails(selectedJobAd);
                }
            }
        }

        private void ShowJobAdDetails(JobAdd jobAd)
        {
            var jobDetailsWindow = new JobDetailsWindow(jobAd);
            jobDetailsWindow.ShowDialog();
        }

        private void InitializeDatabase()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "CREATE TABLE IF NOT EXISTS JobAds (Id INTEGER PRIMARY KEY AUTOINCREMENT, JobName TEXT, AgreementType TEXT, WorkType TEXT, Payment REAL, WorkDays TEXT, WorkHours TEXT, Responsibilities TEXT, Requirements TEXT, Benefits TEXT, AdditionalInfo TEXT)";
                    command.ExecuteNonQuery();
                }
            }
        }

        private void LoadJobAds()
        {
            JobAds.Clear();

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT * FROM JobAds";

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var jobAd = new JobAdd
                            {
                                Id = reader.GetInt32(0),
                                JobName = reader.GetString(1),
                                AgreementType = reader.GetString(2),
                                WorkType = reader.GetString(3),
                                Payment = reader.GetDecimal(4),
                                WorkDays = reader.GetString(5),
                                WorkHours = reader.GetString(6),
                                Responsibilities = reader.GetString(7),
                                Requirements = reader.GetString(8),
                                Benefits = reader.GetString(9),
                                AdditionalInfo = reader.GetString(10),
                            };

                            JobAds.Add(jobAd);
                        }
                    }
                }
            }
        }

        private void OpenAddJobAdWindow(object sender, RoutedEventArgs e)
        {
            var addJobAdWindow = new AddJobWindow(person_id);
            addJobAdWindow.Show();
            this.Close();
        }
    }
}
