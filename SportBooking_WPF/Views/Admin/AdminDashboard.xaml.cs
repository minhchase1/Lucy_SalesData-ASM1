using BusinessObjects;
using DataAccessLayer;
using Repositories;
using Services;
using SportBooking_WPF.Views;
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
namespace SportBooking_WPF.Views.Admin
{
    /// <summary>
    /// Interaction logic for AdminDashboard.xaml
    /// </summary>
    public partial class AdminDashboard : Window
    {
        private readonly IUserService _userService;
        private readonly ICourtService _courtService;
        private readonly ISportService _sportService;
        private readonly ICourtStatusService _courtStatusService;
        private readonly IBookingService _bookingService;

        public AdminDashboard(IUserService userService, ICourtService courtService, ISportService sportService, ICourtStatusService courtStatusService, IBookingService bookingService)
        {
            InitializeComponent();

            // Khởi tạo các service sử dụng cùng 1 DbContext
            var context = new SportsBookingDbContext();
            _userService = new UserService(new UserRepository(context));
            _courtService = new CourtService(new CourtRepository(context));
            _sportService = new SportService(new SportRepository(context));
            _courtStatusService = new CourtStatusService(new CourtStatusRepository(context));


            _bookingService = new BookingService(new BookingRepository(context));
            ShowView(HomeView);

        }

        private void ShowView(UIElement view)
        {
            HomeView.Visibility = Visibility.Collapsed;
            UserView.Visibility = Visibility.Collapsed;
            FieldView.Visibility = Visibility.Collapsed;
            BookingView.Visibility = Visibility.Collapsed;

            view.Visibility = Visibility.Visible;
        }

        private void btnHome_Click(object sender, RoutedEventArgs e)
        {
            ShowView(HomeView);
        }

        private void btnUsers_Click(object sender, RoutedEventArgs e)
        {
            var userView = new UserView(_userService, _courtService, _sportService, _courtStatusService, _bookingService);

            userView.Show();
            this.Hide();
        }

        private void btnFields_Click(object sender, RoutedEventArgs e)
        {
            var courtWindow = new CourtView(_courtService, _sportService, _userService, _courtStatusService, _bookingService);


            courtWindow.Show();
            this.Hide();
        }

        private void btnBookings_Click(object sender, RoutedEventArgs e)
        {
            var bookingService = new BookingService(new BookingRepository(new SportsBookingDbContext()));
            var bookingView = new BookingView(bookingService, _userService, _courtService);

            bookingView.Show();
            this.Hide();
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Đăng xuất thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

            // Mở lại cửa sổ đăng nhập
            LoginWindow loginWindow = new LoginWindow();
            loginWindow.Show();

            // Đóng dashboard
            this.Close();
        }
    }
}
