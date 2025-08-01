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
    /// Interaction logic for UserView.xaml
    /// </summary>
    public partial class UserView : Window
    {
        private readonly IUserService _userService;
        private List<BusinessObjects.User> _users;
        private readonly ICourtService _courtService;
        private readonly ISportService _sportService;
        private readonly ICourtStatusService _courtStatusService;
        private readonly IBookingService _bookingService;

        public UserView(IUserService userService, ICourtService courtService, ISportService sportService, ICourtStatusService courtStatusService, IBookingService bookingService)
        {
            InitializeComponent();
            _userService = userService;
            _courtService = courtService;
            _sportService = sportService;

            LoadUsers();
            this.Closing += UserView_Closing;
            _courtStatusService = courtStatusService;
            _bookingService = bookingService;
        }
        private void UserView_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Mở lại AdminDashboard khi UserView bị đóng
            var admin = new AdminDashboard(_userService, _courtService, _sportService, _courtStatusService,_bookingService);
            admin.Show();
        }

        private void LoadUsers()
        {
            try
            {
                var users = _userService.GetAllUsers();
                dgUsers.ItemsSource = users;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tải danh sách người dùng: " + ex.Message);
            }
        }




        private void BtnEditUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is BusinessObjects.User userToEdit)
            {
                var editWindow = new EditUserWindow(userToEdit, _userService); // Sửa
                if (editWindow.ShowDialog() == true)
                {
                    LoadUsers();
                }
            }
        }
        private void BtnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(keyword))
            {
                // Hiển thị lại toàn bộ danh sách nếu trống
                LoadUsers();
                return;
            }

            try
            {
                var filteredUsers = _userService.GetAllUsers()
                    .Where(u => u.FullName.ToLower().Contains(keyword)
                             || u.Email.ToLower().Contains(keyword))
                    .ToList();

                dgUsers.ItemsSource = filteredUsers;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi tìm kiếm: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnDeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button btn && btn.Tag is BusinessObjects.User userToDelete)
            {
                var result = MessageBox.Show($"Bạn có chắc muốn xoá người dùng: {userToDelete.FullName}?",
                                             "Xác nhận xoá", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    _userService.DeleteUser(userToDelete.UserId);
                    LoadUsers();
                }
            }
        }
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            
            var admin = new AdminDashboard(_userService, _courtService, _sportService, _courtStatusService, _bookingService);
            admin.Show();
        }
    }
}
