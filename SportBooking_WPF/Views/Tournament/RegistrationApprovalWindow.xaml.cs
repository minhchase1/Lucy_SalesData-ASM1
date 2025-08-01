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
    /// Interaction logic for RegistrationApprovalWindow.xaml
    /// </summary>
    public partial class RegistrationApprovalWindow : Window
    {
        private readonly int _tournamentId;
        private readonly ITournamentRegistrationService _registrationService;

        public RegistrationApprovalWindow(int tournamentId)
        {
            InitializeComponent();
            _tournamentId = tournamentId;
            _registrationService = new TournamentRegistrationService(new TournamentRegistrationRepository(new SportsBookingDbContext()));
            Loaded += RegistrationApprovalWindow_Loaded;
        }

        private void RegistrationApprovalWindow_Loaded(object sender, RoutedEventArgs e)
        {
            LoadRegistrations();
        }

        private void LoadRegistrations()
        {
            var list = _registrationService.GetRegistrationsByTournament(_tournamentId);
            dgRegistrations.ItemsSource = list;
        }

        private void btnApprove_Click(object sender, RoutedEventArgs e)
        {
            if (dgRegistrations.SelectedItem is TournamentRegistration selected)
            {
                // Kiểm tra còn suất duyệt không
                if (!_registrationService.CanApproveMore(_tournamentId))
                {
                    MessageBox.Show("Đã đủ số lượng người tham gia. Không thể duyệt thêm.");
                    return;
                }

                if (_registrationService.ApproveRegistration(selected.RegistrationId))
                {
                    MessageBox.Show("Duyệt thành công!");
                    LoadRegistrations();
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi duyệt. Có thể giải đấu đã đủ người.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một đăng ký.");
            }
        }

        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            if (dgRegistrations.SelectedItem is TournamentRegistration selected)
            {
                if (_registrationService.RejectRegistration(selected.RegistrationId))
                {
                    MessageBox.Show("Từ chối thành công!");
                    LoadRegistrations();
                }
                else
                {
                    MessageBox.Show("Có lỗi xảy ra khi từ chối.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một đăng ký.");
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
