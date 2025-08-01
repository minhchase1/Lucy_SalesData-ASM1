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
using BusinessObjects;
using SportBooking_WPF.Views.Tournament;
using SportBooking_WPF.Views.User;



namespace SportBooking_WPF.Views.User
{
    /// <summary>
    /// Interaction logic for UserMainWindow.xaml
    /// </summary>
    public partial class UserMainWindow : Window
    {
        private BusinessObjects.User _currentUser;
        public UserMainWindow(BusinessObjects.User user)
        {
            InitializeComponent();
            _currentUser = user;
            UserMenuItem.Header = $" {user.FullName}";
            this.Title = $" {user.FullName}";
            MainFrame.Navigate(new HomePage());
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new HomePage());
        }

        private void Football_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService?.Navigate(new FootballCourtPage());
        }

        private void Basketball_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService?.Navigate(new BasketballCourtPage());
        }

        private void Badminton_Click(object sender, RoutedEventArgs e)
        {
            // Điều hướng tới trang đặt sân cầu lông
        }

        private void Tournament_Click(object sender, RoutedEventArgs e)
        {
            var tournamentPage = new UserTournamentWindow(_currentUser);
            tournamentPage.Show();
            this.Close();
        }

        private void Profile_Click(object sender, RoutedEventArgs e)
        {
            // Hiển thị trang hồ sơ người dùng
        }

        private void History_Click(object sender, RoutedEventArgs e)
        {
            // Hiển thị lịch sử đặt sân
        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            // Đăng xuất → quay về màn hình đăng nhập
           
            var login = new LoginWindow();
            login.Show();
            this.Close();
        }
    }
}
