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
    /// Logika interakcji dla klasy AddJobWindow.xaml
    /// </summary>
    public partial class AddJobWindow : Window
    {
        
        private const string DatabaseFileName = "JobAdsDatabase.db";
        public const string ConnectionString = "Data Source=" + DatabaseFileName;
        public ObservableCollection<JobAdd> JobAds { get; set; } = new ObservableCollection<JobAdd>();
        public int person_id = 0;

        public AddJobWindow(int i)
        {
            InitializeComponent();
            person_id= i;
        }

        public void LoadJobAds()
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

        public void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            string jobName = JobNameTextBox.Text;
            string agreementType = AgreementTypeTextBox.Text;
            string workType = WorkTypeTextBox.Text;
            decimal payment = decimal.Parse(PaymentTextBox.Text);
            string workDays = WorkDaysTextBox.Text;
            string workHours = WorkHoursTextBox.Text;
            string responsibilities = ResponsibilitiesTextBox.Text;
            string requirements = RequirementsTextBox.Text;
            string benefits = BenefitsTextBox.Text;
            string additionalInfo = AdditionalInfoTextBox.Text;

            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "INSERT INTO JobAds (JobName, AgreementType, WorkType, Payment, WorkDays, WorkHours, Responsibilities, Requirements, Benefits, AdditionalInfo) VALUES (@JobName, @AgreementType, @WorkType, @Payment, @WorkDays, @WorkHours, @Responsibilities, @Requirements, @Benefits, @AdditionalInfo)";
                    command.Parameters.AddWithValue("@JobName", jobName);
                    command.Parameters.AddWithValue("@AgreementType", agreementType);
                    command.Parameters.AddWithValue("@WorkType", workType);
                    command.Parameters.AddWithValue("@Payment", payment);
                    command.Parameters.AddWithValue("@WorkDays", workDays);
                    command.Parameters.AddWithValue("@WorkHours", workHours);
                    command.Parameters.AddWithValue("@Responsibilities", responsibilities);
                    command.Parameters.AddWithValue("@Requirements", requirements);
                    command.Parameters.AddWithValue("@Benefits", benefits);
                    command.Parameters.AddWithValue("@AdditionalInfo", additionalInfo);

                    command.ExecuteNonQuery();
                }
            }

            LoadJobAds();
            MessageBox.Show("Job ad added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            FirstPage f = new FirstPage(person_id);
            f.Show();
            this.Close();
        }
    }
}
