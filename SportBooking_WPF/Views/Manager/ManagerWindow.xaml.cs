using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using BusinessObjects;
using Repositories;
using Services;
using DataAccessLayer;

namespace SportBooking_WPF.Views.Manager
{
    public partial class ManagerWindow : Window
    {
        private readonly IUserService _userService = new UserService(new UserRepository(new SportsBookingDbContext()));
        private System.Collections.ObjectModel.ObservableCollection<BusinessObjects.User> _users = new System.Collections.ObjectModel.ObservableCollection<BusinessObjects.User>();
        private BusinessObjects.User? _selectedUser = null;

        public ManagerWindow()
        {
            InitializeComponent();
            UserDataGrid.ItemsSource = _users;
            LoadUsers();
        }

        private void LoadUsers()
        {
            _users = new System.Collections.ObjectModel.ObservableCollection<BusinessObjects.User>(_userService.GetAllUsers());
            UserDataGrid.ItemsSource = _users;
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(FullNameTextBox.Text) ||
                    string.IsNullOrWhiteSpace(EmailTextBox.Text) ||
                    string.IsNullOrWhiteSpace(PasswordBox.Password))
                {
                    MessageBox.Show("Vui lòng nhập đầy đủ Họ tên, Email và Mật khẩu!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(RoleIdTextBox.Text.Trim(), out int roleId))
                {
                    MessageBox.Show("RoleId phải là số!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                var user = new BusinessObjects.User
                {
                    FullName = FullNameTextBox.Text.Trim(),
                    Email = EmailTextBox.Text.Trim(),
                    PasswordHash = PasswordBox.Password.Trim(),
                    Phone = PhoneTextBox.Text.Trim(),
                    RoleId = roleId,
                    Address = AddressTextBox.Text.Trim(),
                    Dob = DobDatePicker.SelectedDate.HasValue ? DateOnly.FromDateTime(DobDatePicker.SelectedDate.Value) : null,
                    CreatedAt = DateTime.Now
                };

                _userService.Register(user);
                LoadUsers();
                ClearForm();
                MessageBox.Show("Thêm user thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi thêm user: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn user để sửa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                if (!int.TryParse(RoleIdTextBox.Text.Trim(), out int roleId))
                {
                    MessageBox.Show("RoleId phải là số!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                _selectedUser.FullName = FullNameTextBox.Text.Trim();
                _selectedUser.Email = EmailTextBox.Text.Trim();
                if (!string.IsNullOrEmpty(PasswordBox.Password))
                    _selectedUser.PasswordHash = PasswordBox.Password.Trim();
                _selectedUser.Phone = PhoneTextBox.Text.Trim();
                _selectedUser.RoleId = int.TryParse(RoleIdTextBox.Text.Trim(), out int rid) ? rid : 0;
                _selectedUser.Address = AddressTextBox.Text.Trim();
                _selectedUser.Dob = DobDatePicker.SelectedDate.HasValue ? DateOnly.FromDateTime(DobDatePicker.SelectedDate.Value) : null;

                _userService.UpdateUser(_selectedUser);
                LoadUsers();
                ClearForm();
                MessageBox.Show("Cập nhật user thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Lỗi khi cập nhật user: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUser == null)
            {
                MessageBox.Show("Vui lòng chọn user để xóa!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show($"Bạn có chắc muốn xóa user '{_selectedUser.FullName}'?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    _userService.DeleteUser(_selectedUser.UserId);
                    LoadUsers();
                    ClearForm();
                    MessageBox.Show("Xóa user thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Lỗi khi xóa user: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ClearForm_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void ClearForm()
        {
            FullNameTextBox.Text = "";
            EmailTextBox.Text = "";
            PasswordBox.Password = "";
            PhoneTextBox.Text = "";
            RoleIdTextBox.Text = "";
            AddressTextBox.Text = "";
            DobDatePicker.SelectedDate = null;
            _selectedUser = null;
            UserDataGrid.SelectedItem = null;
        }

        private void UserDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UserDataGrid.SelectedItem is BusinessObjects.User user)
            {
                _selectedUser = user;
                FullNameTextBox.Text = user.FullName;
                EmailTextBox.Text = user.Email;
                PasswordBox.Password = string.Empty;
                PhoneTextBox.Text = user.Phone;
                RoleIdTextBox.Text = user.RoleId.ToString();
                AddressTextBox.Text = user.Address;
                DobDatePicker.SelectedDate = user.Dob.HasValue ? user.Dob.Value.ToDateTime(TimeOnly.MinValue) : null;
            }
        }
    }
}
