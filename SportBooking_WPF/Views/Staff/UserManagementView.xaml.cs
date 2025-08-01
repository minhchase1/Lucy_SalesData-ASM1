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
    /// Interaction logic for UserManagementView.xaml
    /// </summary>
    public partial class UserManagementView : UserControl
    {
        private readonly IUserService _userService =
            new UserService(new UserRepository(new SportsBookingDbContext()));
        public UserManagementView()
        {
            InitializeComponent();
            LoadtUserData();
        }
        private void LoadtUserData()
        {
            var users = _userService.GetAllUsers().Where(u => u.RoleId == 1);
            dgUser.ItemsSource = null;
            dgUser.ItemsSource = users;
        }
        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearchUser.Text.Trim();
            if (string.IsNullOrEmpty(keyword))
            {
                MessageBox.Show("Please enter a keyword to search.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var users = _userService.GetUserByKeyWord(keyword);
            if (users == null)
            {
                MessageBox.Show("No users found with the given keyword.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            else
            {
                dgUser.ItemsSource = null;
                dgUser.ItemsSource = users;
                txtSearchUser.Text = keyword;
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            txtSearchUser.Text = string.Empty;
            LoadtUserData();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var userEditWindow = new UserEditWindow();
            if (userEditWindow.ShowDialog() == true)
            {
                var newUser = userEditWindow.User;
                if (_userService.AddUser(newUser))
                {
                    MessageBox.Show("User added successfully.",
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to add user. Please try again.",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            LoadtUserData();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (dgUser.SelectedItem == null)
            {
                MessageBox.Show("Please select a user to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var selectedUser = dgUser.SelectedItem as BusinessObjects.User;
            if (selectedUser == null)
            {
                MessageBox.Show("Invalid user selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var userEditWindow = new UserEditWindow(selectedUser);
            if (userEditWindow.ShowDialog() == true)
            {
                var updatedUser = userEditWindow.User;
                if (_userService.UpdateUser(updatedUser))
                {
                    MessageBox.Show("User updated successfully.",
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Failed to update user. Please try again.",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            LoadtUserData();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgUser.SelectedItem == null)
            {
                MessageBox.Show("Please select a user to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var selectedUser = dgUser.SelectedItem as BusinessObjects.User;
            if (selectedUser == null)
            {
                MessageBox.Show("Invalid user selected.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            var result = MessageBox.Show($"Are you sure you want to delete user {selectedUser.FullName}?",
                "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var deleted = _userService.DeleteUser(selectedUser.UserId);
                if (deleted)
                {
                    MessageBox.Show("User deleted successfully.",
                        "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadtUserData();
                }
                else
                {
                    MessageBox.Show("Failed to delete user. Please try again.",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            LoadtUserData();
        }
    }
}
