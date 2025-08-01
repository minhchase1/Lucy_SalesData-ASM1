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
using SportBooking_WPF.Views.Court;
using SportBooking_WPF.Views.User;



namespace SportBooking_WPF.Views.User
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        public HomePage()
        {
            InitializeComponent();
        }

        private void BasketballCourt_Click(object sender, RoutedEventArgs e)
        {
            var page = new BasketballCourtPage();
            NavigationService?.Navigate(page);
        }

        private void FootballCourt_Click(object sender, RoutedEventArgs e)
        {
            var page = new FootballCourtPage();
            NavigationService?.Navigate(page);
        }
        private void BadmintonCourt_Click(object sender, RoutedEventArgs e)
        {
            var page = new BadmintonCourtPage();
            NavigationService?.Navigate(page);
        }
    }
}
