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
    /// Interaction logic for TournamentRequestApprovalWindow.xaml
    /// </summary>
    public partial class TournamentRequestApprovalWindow : Window
    {
        private readonly ITournamentRequestService _requestService =
            new TournamentRequestService(new TournamentRequestRepository(new SportsBookingDbContext()));

        private readonly ITournamentService _tournamentService =
            new TournamentService(new TournamentRepository(new SportsBookingDbContext()));


        public TournamentRequestApprovalWindow()
        {
            InitializeComponent();
            LoadRequests();
        }

        private void LoadRequests()
        {
            dgRequests.ItemsSource = _requestService.GetAllRequests();
        }

        private void ApproveSelected_Click(object sender, RoutedEventArgs e)
        {
            if (dgRequests.SelectedItem is TournamentRequest request)
            {
                if (request.Status == "Đã duyệt" || request.Status == "Từ chối" || request.Status == "Đã hủy")
                {
                    MessageBox.Show("Đơn này đã được xử lý.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("Bạn có chắc chắn muốn duyệt đơn và tạo giải đấu?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
                    return;

                var tournament = new BusinessObjects.Tournament
                {
                    Title = request.Title,
                    SportId = request.SportId,
                    OrganizerId = request.UserId,
                    Description = request.Description,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    Location = request.Location,
                    Status = "Mở đăng ký",
                    RegistrationDeadline = request.RegistrationDeadline,
                    IsTeamBased = request.IsTeamBased,
                    Rules = request.Rules,
                    MaxParticipants = request.MaxParticipants
                };

                bool created = _tournamentService.AddTournament(tournament);

                if (created)
                {
                    _requestService.UpdateRequestStatus(request.RequestId, "Đã duyệt");
                    MessageBox.Show("Đã duyệt đơn và tạo giải đấu thành công.", "Thành công", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Tạo giải đấu thất bại. Vui lòng thử lại.", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                }

                LoadRequests();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một đơn để duyệt.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RejectSelected_Click(object sender, RoutedEventArgs e)
        {
            if (dgRequests.SelectedItem is TournamentRequest request)
            {
                if (request.Status == "Đã duyệt" || request.Status == "Từ chối" || request.Status == "Đã hủy")
                {
                    MessageBox.Show("Đơn này đã được xử lý.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                if (MessageBox.Show("Bạn có chắc chắn muốn từ chối đơn này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    return;

                _requestService.UpdateRequestStatus(request.RequestId, "Từ chối");
                MessageBox.Show("Đã từ chối đơn yêu cầu.", "Đã cập nhật", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadRequests();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một đơn để từ chối.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
