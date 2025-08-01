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
using DataAccessLayer;
using Repositories;
using Services;
using BusinessObjects;
using CourtModel = BusinessObjects.Court;
using UserModel = BusinessObjects.User;

namespace SportBooking_WPF.Views.Court
{
    /// <summary>
    /// Interaction logic for CourtDetailPage.xaml
    /// </summary>
    public partial class CourtDetailPage : Page
    {
        private CourtModel _court;
        private UserModel _user;

        public CourtDetailPage( CourtModel court, UserModel user)
        {
            InitializeComponent();

            _court = court;
            _user = user;
            LoadCourtDetail();


        }

        private void LoadCourtDetail()
        {
            txtCourtName.Text = _court.CourtName;
            txtLocation.Text = $"📍 Địa điểm: {_court.Location}";
            txtPrice.Text = $"💲 Giá thuê mỗi giờ: {_court.PricePerHour:N0} VND";

            // Hiển thị hình ảnh theo CourtId
            try
            {
                string imagePath = $"/Resources/Images/court_{_court.CourtId}.jpg";
                CourtImage.Source = new BitmapImage(new Uri($"pack://application:,,,{imagePath}", UriKind.Absolute));
            }
            catch
            {
                // Nếu không tìm thấy ảnh thì hiển thị ảnh mặc định
                CourtImage.Source = new BitmapImage(new Uri("pack://application:,,,/Resources/Images/default-court.jpg", UriKind.Absolute));
            }
        }


        private void BookCourt_Click(object sender, RoutedEventArgs e)
        {
            /*MessageBox.Show($"Bạn đã chọn đặt sân: {_court.CourtName}",
                "Xác nhận", MessageBoxButton.OK,
                MessageBoxImage.Information);*/
            var bookingPage = new SportBooking_WPF.Views.Booking.CourtBookingPage(_court, _user);
            NavigationService?.Navigate(bookingPage);
            // Chuyển đến form chọn giờ, ngày, người dùng,...
        }
    }
}
