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
    /// Logika interakcji dla klasy JobDetailsWindow.xaml
    /// </summary>
    public partial class JobDetailsWindow : Window
    {
        public JobDetailsWindow(JobAdd jobAd)
        {
            InitializeComponent();
            DataContext = jobAd;
            
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
