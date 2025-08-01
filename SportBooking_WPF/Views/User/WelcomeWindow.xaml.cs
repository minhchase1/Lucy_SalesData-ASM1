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
using SportBooking_WPF.Views.Admin;
using SportBooking_WPF.Views.Manager;
using SportBooking_WPF.Views.Staff;
using SportBooking_WPF.Views.User;

namespace SportBooking_WPF.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private readonly IUserService _userService = new UserService(new UserRepository(new SportsBookingDbContext()));
        private readonly ICourtService courtService;
        private readonly ISportService sportService;
        private readonly ICourtStatusService courtStatusService;
        private readonly IBookingService bookingService;
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Login_Click(object sender, RoutedEventArgs e)
        {
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password;

            // Kiểm tra dữ liệu đầu vào đơn giản
            if (string.IsNullOrEmpty(email) || email == "Email" ||
                string.IsNullOrEmpty(password) || password == "Password")
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            try
            {
                var user = _userService.Login(email, password);

                if (user != null)
                {
                    Helpers.LoggedInUser.Instance.User = user;
                    MessageBox.Show($"Đăng nhập thành công! Xin chào {user.FullName}", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Chuyển sang trang chính của User
                    //UserMainWindow main = new UserMainWindow(user); // nếu có constructor truyền user
                    string role = user.Role?.RoleName;
                    switch (role)
                    {
                        case "Admin":
                            var adminMain = new AdminDashboard(_userService, courtService, sportService, courtStatusService, bookingService);
                            adminMain.Show();
                            this.Close();
                            break;
                        case "Manager":
                            var managerMain = new ManagerDashboardWindow(user);
                            managerMain.ShowDialog();
                            this.Close();
                            break;
                        case "Staff":
                            var staffMain = new StaffMainWindow();
                            staffMain.Show();
                            this.Close();
                            break;
                        case "User":
                        default:
                            // Mặc định là User
                            var main = new UserMainWindow(user);
                            main.Show();
                            this.Close();
                            break;
                    }

                }
                else
                {
                    MessageBox.Show("Email hoặc mật khẩu không đúng!", "Lỗi đăng nhập", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Đã xảy ra lỗi: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            RegisterWindow register = new RegisterWindow();
            register.ShowDialog();
            this.Show(); // Hiển thị lại cửa sổ đăng nhập sau khi đăng ký
        }

        // Placeholder tạm: Email
        private void TextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtEmail.Text == "Email")
            {
                txtEmail.Text = "";
                txtEmail.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void TextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                txtEmail.Text = "Email";
                txtEmail.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }

        // Placeholder tạm: Password
        private void PasswordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtPassword.Password == "Password")
            {
                txtPassword.Clear();
                txtPassword.Foreground = System.Windows.Media.Brushes.Black;
            }
        }

        private void PasswordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtPassword.Password))
            {
                txtPassword.Password = "Password";
                txtPassword.Foreground = System.Windows.Media.Brushes.Gray;
            }
        }
    }
}
