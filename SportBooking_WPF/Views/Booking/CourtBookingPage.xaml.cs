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
using CourtModel = BusinessObjects.Court;
using UserModel = BusinessObjects.User;
using BookingModel = BusinessObjects.Booking;


namespace SportBooking_WPF.Views.Booking
{
    /// <summary>
    /// Interaction logic for BookingWindow.xaml
    /// </summary>
    public partial class CourtBookingPage : Page
    {
        private CourtModel selectedCourt;
        private UserModel currentUser;
        private readonly IBookingService _bookingService =
    new BookingService(new BookingRepository(new SportsBookingDbContext()));
        public CourtBookingPage(CourtModel court, UserModel user)
        {
            InitializeComponent();
            selectedCourt = court;
            currentUser = user;

            // Gán thông tin tên sân vào TextBox
            txtboxCourtName.Text = selectedCourt.CourtName;


            // Load ảnh theo CourtId
            string imagePath = $"/Resources/Images/court_{selectedCourt.CourtId}.jpg";
            try
            {
                imgCourt.Source = new BitmapImage(new Uri($"pack://application:,,,{imagePath}", UriKind.Absolute));
            }
            catch
            {
                MessageBox.Show("Không tìm thấy hình ảnh sân.");
            }

            // Gợi ý tên người dùng nếu cần
            txtboxBookerName.Text = currentUser.FullName;
        }

        private void ConfirmBooking_Click(object sender, RoutedEventArgs e)
        {
            string customerName = txtboxBookerName.Text;
            DateTime bookingDate = dpDate.SelectedDate ?? DateTime.Now;
            string selectedTime = (cbStartTime.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";

            if (!TimeOnly.TryParse(selectedTime, out TimeOnly start))
            {
                MessageBox.Show("Thời gian bắt đầu không hợp lệ. Định dạng đúng: HH:mm");
                return;
            }

            if (!double.TryParse(txtDuration.Text, out double durationHours) || durationHours <= 0)
            {
                MessageBox.Show("Thời lượng phải là số dương.");
                return;
            }

            TimeOnly end = start.Add(TimeSpan.FromHours(durationHours));
            decimal pricePerHour = selectedCourt.PricePerHour ?? 0;
            decimal totalPrice = (decimal)durationHours * pricePerHour;

            var booking = new BookingModel
            {
                CourtId = selectedCourt.CourtId,
                UserId = currentUser.UserId,
                BookingDate = DateOnly.FromDateTime(bookingDate),
                StartTime = start,
                EndTime = end,
                CreatedAt = DateTime.Now,
                Status = "Pending",
                TotalPrice = totalPrice
            };

            var existingBookings = _bookingService.GetBookingsByCourtAndDate(selectedCourt.CourtId, DateOnly.FromDateTime(bookingDate));

            bool isOverlapping = existingBookings.Any(b =>
                (start >= b.StartTime && start < b.EndTime) ||
                (end > b.StartTime && end <= b.EndTime) ||
                (start <= b.StartTime && end >= b.EndTime) // phủ toàn bộ
            );

            if (isOverlapping)
            {
                MessageBox.Show("Khung giờ đã có người đặt. Vui lòng chọn giờ khác.",
                    "Thông báo",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return;
            }




            // TODO: Thêm logic lưu vào DB ở đây
            try
            {
                _bookingService.AddBooking(booking);
                MessageBox.Show(
                $"Thông tin đặt sân:\n" +
                $"- Người đặt: {customerName}\n" +
                $"- Sân: {selectedCourt.CourtName}\n" +
                $"- Ngày: {bookingDate:dd/MM/yyyy}\n" +
                $"- Giờ bắt đầu: {start}\n" +
                $"- Thời lượng: {durationHours} giờ\n" +
                $"- Tổng tiền: {totalPrice:N0} VND",
                "Xác nhận đặt sân",
                MessageBoxButton.OK, MessageBoxImage.Information);
                // Optionally điều hướng lại
                // NavigationService?.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi đặt sân: {ex.Message}", "Lỗi",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void txtDuration_TextChanged(object sender, TextChangedEventArgs e)
        {
            TinhToanGiaTien();
        }

        private void cbStartTime_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TinhToanGiaTien();
        }

        private void TinhToanGiaTien()
        {
            if (!double.TryParse(txtDuration.Text, out double hours) || hours <= 0)
            {
                txtTotalPrice.Text = "0 VND";
                return;
            }

            decimal pricePerHour = selectedCourt.PricePerHour ?? 0;
            decimal total = (decimal)hours * pricePerHour;
            txtTotalPrice.Text = $"{total:N0} VND";
        }


    }
}
