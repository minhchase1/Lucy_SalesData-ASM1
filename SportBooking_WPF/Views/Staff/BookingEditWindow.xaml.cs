using DataAccessLayer;
using Repositories;
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

namespace SportBooking_WPF.Views.Staff
{
    /// <summary>
    /// Interaction logic for BookingEditWindow.xaml
    /// </summary>
    public partial class BookingEditWindow : Window
    {
        public BusinessObjects.Booking BookingResult { get; set; } = null!;
        private bool isEditMode = false;
        private readonly ICourtService courtService = 
            new CourtService(new CourtRepository(new SportsBookingDbContext()));
        private readonly IUserService _userService =
            new UserService(new UserRepository(new SportsBookingDbContext()));

        public BookingEditWindow(BusinessObjects.Booking booking = null)
        {
            InitializeComponent();
            isEditMode = booking != null;
            BookingResult = isEditMode ? booking! : new BusinessObjects.Booking();

            LoadCourts();
            LoadUser();

            if (isEditMode)
                LoadBookingDetails();
        }

        private void LoadCourts()
        {
            var courts = courtService.GetAllCourts();
            cbCourt.ItemsSource = courts;
            cbCourt.DisplayMemberPath = "CourtName";
            cbCourt.SelectedValuePath = "CourtId";
        }
        private void LoadUser()
        {
            var users = _userService.GetAllUsers();
            cbUser.ItemsSource = users;
            cbUser.DisplayMemberPath = "FullName";
            cbUser.SelectedValuePath = "UserId";
        }
        private void LoadBookingDetails()
        {
            cbUser.SelectedValue = BookingResult.UserId;
            cbCourt.SelectedValue = BookingResult.CourtId;
            dpBookingDate.SelectedDate = BookingResult.BookingDate.ToDateTime(new TimeOnly(0, 0, 0));
            txtStartTime.Text = BookingResult.StartTime.ToString("hh\\:mm");
            txtEndTime.Text = BookingResult.EndTime.ToString("hh\\:mm");
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbCourt.SelectedValue == null || dpBookingDate.SelectedDate == null || cbUser.SelectedValue == null)
                {
                    MessageBox.Show("All fields must be filled.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!TimeOnly.TryParse(txtStartTime.Text, out var startTime) ||
                    !TimeOnly.TryParse(txtEndTime.Text, out var endTime))
                {
                    MessageBox.Show("Invalid time format. Please use HH:mm format.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (startTime >= endTime)
                {
                    MessageBox.Show("Start time must be earlier than end time.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                BookingResult.UserId = (int)cbUser.SelectedValue;
                BookingResult.CourtId = (int)cbCourt.SelectedValue;
                BookingResult.BookingDate = DateOnly.FromDateTime(dpBookingDate.SelectedDate.Value);
                BookingResult.StartTime = startTime;
                BookingResult.EndTime = endTime;

                if (!isEditMode)
                {
                    BookingResult.Status = "Confirmed";
                    BookingResult.CreatedAt = DateTime.Now;
                }

                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving booking: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
