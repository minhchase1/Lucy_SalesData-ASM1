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
    /// Interaction logic for EditBookingWindow.xaml
    /// </summary>
    public partial class EditBookingWindow : Window
    {
        private readonly IBookingService _bookingService;
        private readonly IUserService _userService;
        private readonly ICourtService _courtService;
        private BusinessObjects.Booking _booking;
        private bool _isEditMode;

        public EditBookingWindow(BusinessObjects.Booking? booking, IBookingService bookingService,
                                 IUserService userService, ICourtService courtService)
        {
            InitializeComponent();
            _bookingService = bookingService;
            _userService = userService;
            _courtService = courtService;

            LoadUsers();
            LoadCourts();

            if (booking != null)
            {
                _booking = booking;
                _isEditMode = true;
                LoadData();
            }
            else
            {
                _booking = new BusinessObjects.Booking();
                _isEditMode = false;
            }
        }

        private void LoadUsers()
        {
            var users = _userService.GetAllUsers();
            cbUsers.ItemsSource = users;
        }

        private void LoadCourts()
        {
            var courts = _courtService.GetAllCourts();
            cbCourts.ItemsSource = courts;
        }

        private void LoadData()
        {
            cbUsers.SelectedValue = _booking.UserId;
            cbCourts.SelectedValue = _booking.CourtId;
            dpBookingDate.SelectedDate = _booking.BookingDate.ToDateTime(new TimeOnly(0, 0));
            txtStartTime.Text = _booking.StartTime.ToString("HH:mm");
            txtEndTime.Text = _booking.EndTime.ToString("HH:mm");
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _booking.UserId = (int)cbUsers.SelectedValue;
                _booking.CourtId = (int)cbCourts.SelectedValue;
                _booking.BookingDate = DateOnly.FromDateTime(dpBookingDate.SelectedDate ?? DateTime.Today);
                _booking.StartTime = TimeOnly.Parse(txtStartTime.Text);
                _booking.EndTime = TimeOnly.Parse(txtEndTime.Text);

                if (_isEditMode)
                {
                    _bookingService.UpdateBooking(_booking);
                    MessageBox.Show("Cập nhật đặt sân thành công!");
                }
                else
                {
                    _booking.CreatedAt = DateTime.Now;
                    _booking.Status = "Pending"; // hoặc bạn cho chọn trạng thái
                    _bookingService.AddBooking(_booking);
                    MessageBox.Show("Thêm đặt sân thành công!");
                }

                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
