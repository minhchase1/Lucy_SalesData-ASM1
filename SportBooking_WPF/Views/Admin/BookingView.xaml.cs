using Services;
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

namespace SportBooking_WPF.Views.Admin
{
    /// <summary>
    /// Interaction logic for BookingView.xaml
    /// </summary>
    public partial class BookingView : Window
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly ICourtService _courtService;
        private List<BusinessObjects.Booking> _bookings;
       

        public BookingView(IBookingService bookingService, IUserService userService, ICourtService courtService)
        {
            InitializeComponent();
            _bookingService = bookingService;
            _userService = userService;
            _courtService = courtService;
            this.Closing += Window_Closing; // Đăng ký sự kiện Closing

            LoadBookingList();
        }

        private void LoadBookingList()
        {
            try
            {
                _bookings = _bookingService.GetAllBookings(true);
                dgBookings.ItemsSource = _bookings;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách đặt sân: " + ex.Message);
            }
        }
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearchBooking.Text.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(keyword) || keyword == "nhập tên người dùng hoặc tên sân...")
            {
                dgBookings.ItemsSource = _bookings;
            }
            else
            {
                var filtered = _bookings.Where(b =>
                    b.User.FullName.ToLower().Contains(keyword) ||
                    b.Court.CourtName.ToLower().Contains(keyword)).ToList();

                dgBookings.ItemsSource = filtered;
            }
        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EditBookingWindow(null, _bookingService, _userService, _courtService);
            if (editWindow.ShowDialog() == true)
            {
                LoadBookingList();
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is BusinessObjects.Booking booking)
            {
                var editWindow = new EditBookingWindow(booking, _bookingService, _userService, _courtService);
                if (editWindow.ShowDialog() == true)
                {
                    LoadBookingList();
                }
            }
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is BusinessObjects.Booking booking)
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xoá đặt sân ID {booking.BookingId} không?", "Xác nhận", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _bookingService.DeleteBooking(booking.BookingId);
                        LoadBookingList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi xoá đặt sân: " + ex.Message);
                    }
                }
            }
        }

        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Khởi tạo lại AdminDashboard với các service cần thiết
            var context = new DataAccessLayer.SportsBookingDbContext();

            var newUserService = new UserService(new Repositories.UserRepository(context));
            var newCourtService = new CourtService(new Repositories.CourtRepository(context));
            var newSportService = new SportService(new Repositories.SportRepository(context));
            var newCourtStatusService = new CourtStatusService(new Repositories.CourtStatusRepository(context));
            var newBookingService = new BookingService(new Repositories.BookingRepository(context));
            var adminWindow = new AdminDashboard(newUserService, newCourtService, newSportService, newCourtStatusService, newBookingService);

            adminWindow.Show();
        }
    }
}
