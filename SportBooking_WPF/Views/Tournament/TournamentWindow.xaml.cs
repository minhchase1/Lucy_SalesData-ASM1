using BusinessObjects;
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

namespace SportBooking_WPF.Views.Tournament
{
    /// <summary>
    /// Interaction logic for TournamentWindow.xaml
    /// </summary>
    public partial class TournamentWindow : Window
    {
        private readonly ITournamentService tournamentService = new TournamentService(
            new TournamentRepository(new SportsBookingDbContext()));

        private readonly BusinessObjects.User _currentUser;
        private List<BusinessObjects.Tournament> allTournaments;

        public TournamentWindow(BusinessObjects.User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            Loaded += TournamentWindow_Loaded;

            txtSearch.KeyDown += TxtSearch_KeyDown; // Bắt phím Enter
        }

        private void TournamentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTournaments();
            ApplyRolePermissions();
        }

        private void LoadTournaments()
        {
            if (tournamentService.UpdateTournamentStatuses())
            {
                MessageBox.Show("Đã cập nhật trạng thái các giải đấu.");
            }

            allTournaments = tournamentService.GetAllTournaments();
            dgTournaments.ItemsSource = allTournaments;
        }

        private void ApplyRolePermissions()
        {
            bool isAdmin = _currentUser.RoleId == 4;
            bool isManager = _currentUser.RoleId == 3;
            bool isStaff = _currentUser.RoleId == 2;
            

            btnAdd.Visibility = (isManager || isAdmin) ? Visibility.Visible : Visibility.Collapsed;
            btnEdit.Visibility = (isManager || isAdmin) ? Visibility.Visible : Visibility.Collapsed;
            btnDelete.Visibility = (isManager || isAdmin) ? Visibility.Visible : Visibility.Collapsed;
            btnApprove.Visibility = (isManager || isStaff || isAdmin) ? Visibility.Visible : Visibility.Collapsed;
            
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearch.Text.Trim();

            if (string.IsNullOrWhiteSpace(keyword))
            {
                dgTournaments.ItemsSource = allTournaments;
                return;
            }

            var results = tournamentService.SearchTournaments(keyword);
            dgTournaments.ItemsSource = results;
        }

        private void TxtSearch_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnSearch_Click(sender, e);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new TournamentEditDialog(_currentUser);
            if (dialog.ShowDialog() == true)
            {
                if (tournamentService.AddTournament(dialog.Tournament))
                {
                    LoadTournaments();
                    MessageBox.Show("Thêm giải đấu thành công.");
                }
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (dgTournaments.SelectedItem is not BusinessObjects.Tournament selected)
            {
                MessageBox.Show("Vui lòng chọn một giải đấu để sửa.");
                return;
            }

            var dialog = new TournamentEditDialog(selected, _currentUser);
            if (dialog.ShowDialog() == true)
            {
                if (tournamentService.UpdateTournament(dialog.Tournament))
                {
                    LoadTournaments();
                    MessageBox.Show("Cập nhật thành công.");
                }
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (dgTournaments.SelectedItem is not BusinessObjects.Tournament selected)
            {
                MessageBox.Show("Vui lòng chọn một giải đấu để xoá.");
                return;
            }

            var confirm = MessageBox.Show("Bạn có chắc chắn muốn xoá giải đấu này?", "Xác nhận", MessageBoxButton.YesNo);
            if (confirm == MessageBoxResult.Yes)
            {
                if (tournamentService.DeleteTournament(selected.TournamentId))
                {
                    LoadTournaments();
                    MessageBox.Show("Xoá thành công.");
                }
            }
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            if (dgTournaments.SelectedItem is not BusinessObjects.Tournament selected)
            {
                MessageBox.Show("Vui lòng chọn một giải đấu để duyệt đăng ký.");
                return;
            }

            var approvalWindow = new RegistrationApprovalWindow(selected.TournamentId);
            approvalWindow.ShowDialog();
        }

        private void btnViewMyRegistrations_Click(object sender, RoutedEventArgs e)
        {
            var viewWindow = new ViewRegistrationWindow(_currentUser);
            viewWindow.ShowDialog();
        }
        private void btnApproveRequest_Click(object sender, RoutedEventArgs e)
        {
            var approvalWindow = new TournamentRequestApprovalWindow();
            approvalWindow.ShowDialog();
            LoadTournaments(); // Tải lại danh sách giải đấu nếu có giải mới được tạo
        }

    }
}
    

