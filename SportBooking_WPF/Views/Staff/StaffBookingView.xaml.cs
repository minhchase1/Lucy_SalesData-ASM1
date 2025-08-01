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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SportBooking_WPF.Views.Staff
{
    /// <summary>
    /// Interaction logic for StaffBookingView.xaml
    /// </summary>
    public partial class StaffBookingView : UserControl
    {
        private readonly IBookingService _bookingService;
        BusinessObjects.Booking booking { get; set; }
        public StaffBookingView()
        {
            InitializeComponent();

            var dbContext = new SportsBookingDbContext(); // Tạo DbContext
            var bookingRepo = new BookingRepository(dbContext); // Tạo repository
            _bookingService = new BookingService(bookingRepo); // Truyền vào service
            LoadBookingData();
        }
        private void LoadBookingData()
        {
            var bookings = _bookingService.GetAllBookings(true);
            dgBooking.ItemsSource = bookings;
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            // search by user name or court name
            string keyword = txtSearch.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Please enter a keyword to search.",
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            else
            {
                var filteredBookings = _bookingService.GetBookingsByUserOrCourt(keyword);
                if (filteredBookings.Count == 0)
                {
                    MessageBox.Show("No bookings found for the given keyword.",
                        "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadBookingData();
                }
                else
                {
                    dgBooking.ItemsSource = filteredBookings;
                }
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearch.Text = string.Empty;
            LoadBookingData();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            var selectedBooking = dgBooking.SelectedItem as BusinessObjects.Booking;
            if (selectedBooking == null)
            {
                MessageBox.Show("Please select a booking to update.",
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var editBookingWindow = new BookingEditWindow(selectedBooking);
            editBookingWindow.Owner= Window.GetWindow(this);
            if (editBookingWindow.ShowDialog() == true)
            {
                var updatedBooking = editBookingWindow.BookingResult;
                var updated = _bookingService.UpdateBooking(updatedBooking);
                if (updated)
                {
                    MessageBox.Show("Booking updated successfully.",
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information); 
                }
                else
                {
                    MessageBox.Show("Failed to update booking.", "Failure",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                LoadBookingData();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var editBookingWindow = new BookingEditWindow();
            editBookingWindow.Owner = Window.GetWindow(this);
            if (editBookingWindow.ShowDialog() == true)
            {
                var newBooking = editBookingWindow.BookingResult;
                var created = _bookingService.AddBooking(newBooking);
                if(created)
                {
                    MessageBox.Show("New booking created successfully.",
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to create new booking.", "Failure",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                LoadBookingData();
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgBooking.SelectedItems == null )
            {
                MessageBox.Show("Please select a booking to delete.",
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var selectedBooking = dgBooking.SelectedItem as BusinessObjects.Booking;
            if (selectedBooking == null)
            {
                MessageBox.Show("No booking exist.",
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!selectedBooking.Status.Equals("Pending"))
            {
                MessageBox.Show("You cannot delete a booking that is confirmed, completed or cancelled.",
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var ret = MessageBox.Show("Are you sure you want to delete the selected booking(s)?",
                "Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (ret == MessageBoxResult.Yes)
            {
                var deleted = _bookingService.DeleteBooking(selectedBooking.BookingId);
                if (deleted)
                {
                    MessageBox.Show("Selected booking(s) deleted successfully.",
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to delete the selected booking(s).", "Failure",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
                LoadBookingData();
            }
        }

        private void dgBooking_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selected = dgBooking.SelectedItem as BusinessObjects.Booking;
            if (selected == null) return;
            booking = selected;
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            if (dgBooking.SelectedItems == null)
            {
                MessageBox.Show("Please select a booking to approve.",
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var selectedBooking = dgBooking.SelectedItem as BusinessObjects.Booking;
            if (selectedBooking == null)
            {
                MessageBox.Show("No booking exist.",
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (!selectedBooking.Status.Equals("Pending"))
            {
                MessageBox.Show("You cannot approve a booking that is confirmed, completed or cancelled.",
                    "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var approved = _bookingService.ApproveBooking(selectedBooking.BookingId);
            if (approved)
            {
                MessageBox.Show("Selected booking(s) approved successfully.",
                    "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Failed to approve the selected booking(s).", "Failure",
                                MessageBoxButton.OK, MessageBoxImage.Error);
            }
            LoadBookingData();
        }
    }
}
