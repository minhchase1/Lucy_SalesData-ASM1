using System.Windows;
using BusinessObjects;
using SportBooking_WPF.Views.Tournament;

namespace SportBooking_WPF.Views.Manager
{
    public partial class ManagerDashboardWindow : Window
    {
        BusinessObjects.User _currentUser { get; set; }
        public ManagerDashboardWindow(BusinessObjects.User user)
        {
            InitializeComponent();
            _currentUser = user;
        }

        private void BtnUserManagement_Click(object sender, RoutedEventArgs e)
        {
            var userWindow = new ManagerWindow();
            userWindow.ShowDialog();
        }

        

        private void BtnRevenueReport_Click(object sender, RoutedEventArgs e)
        {
            RevenueReportWindow window = new RevenueReportWindow();
            window.ShowDialog(); 
        }

        
        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            var welWindon = new LoginWindow();
            welWindon.Show();
            this.Close();
        }

        private void BtnTournamentManagement_Click(object sender, RoutedEventArgs e)
        {
            var tournamentWindow = new TournamentWindow(_currentUser);
            tournamentWindow.Show();
            Close();
        }
    }
}
