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

namespace SportBooking_WPF.Views
{
    /// <summary>
    /// Interaction logic for RegisterWindow.xaml
    /// </summary>
    public partial class RegisterWindow : Window
    {
        private readonly IUserService _userService = new UserService(new UserRepository(new SportsBookingDbContext()));
        public RegisterWindow()
        {
            InitializeComponent();
        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            string fullName = txtFullName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string password = txtPassword.Password.Trim();
            DateTime? dob = dpDOB.SelectedDate;
            string address = txtAddress.Text.Trim();
            string phone = txtPhone.Text.Trim();

            if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email)
                || string.IsNullOrWhiteSpace(password) || dob == null || string.IsNullOrWhiteSpace(address) || string.IsNullOrWhiteSpace(phone))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var newUser = new BusinessObjects.User
            {
                FullName = fullName,
                Email = email,
                PasswordHash = password,
                Dob = DateOnly.FromDateTime(dob.Value),
                Address = address,
                CreatedAt = DateTime.Now,
                RoleId = 1, 
                Phone = phone,

            };

            bool success = _userService.Register(newUser);

            if (success)
            {
                MessageBox.Show("Đăng ký thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("Email đã tồn tại!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

    }
}
