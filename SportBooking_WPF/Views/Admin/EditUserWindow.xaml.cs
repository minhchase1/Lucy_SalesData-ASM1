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
    /// Interaction logic for EditUserWindow.xaml
    /// </summary>
    public partial class EditUserWindow : Window
    {
        private readonly IUserService _userService;
        private readonly BusinessObjects.User _user;
        private readonly bool _isEditMode;

        // Constructor thêm mới
        public EditUserWindow(IUserService userService)
        {
            InitializeComponent();
            _userService = userService;
            _user = new BusinessObjects.User(); // tạo mới
            _isEditMode = false;
        }

        // Constructor sửa
        public EditUserWindow(BusinessObjects.User user, IUserService userService)
        {
            InitializeComponent();
            _user = user ?? throw new ArgumentNullException(nameof(user));
            _userService = userService;
            _isEditMode = true;

            // Gán dữ liệu lên form
            txtFullName.Text = _user.FullName;
            txtEmail.Text = _user.Email;
            txtPhone.Text = _user.Phone;
            dpDob.SelectedDate = _user.Dob?.ToDateTime(TimeOnly.MinValue);
            txtAddress.Text = _user.Address;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            // Validate cơ bản
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                MessageBox.Show("Vui lòng nhập đầy đủ thông tin bắt buộc.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Gán lại thông tin từ form vào _user
            _user.FullName = txtFullName.Text.Trim();
            _user.Email = txtEmail.Text.Trim();
            _user.Phone = txtPhone.Text.Trim();
            _user.Dob = dpDob.SelectedDate.HasValue
                ? DateOnly.FromDateTime(dpDob.SelectedDate.Value)
                : null;
            _user.Address = txtAddress.Text.Trim();

            try
            {
                if (_isEditMode)
                {
                    _userService.UpdateUser(_user);
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _userService.AddUser(_user);
                    MessageBox.Show("Thêm người dùng thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }

                DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi lưu: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
