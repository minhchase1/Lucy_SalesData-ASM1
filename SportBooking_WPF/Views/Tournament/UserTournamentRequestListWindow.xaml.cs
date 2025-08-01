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
    /// Interaction logic for UserTournamentRequestListWindow.xaml
    /// </summary>
    public partial class UserTournamentRequestListWindow : Window
    {
        private readonly BusinessObjects.User _currentUser;
        private readonly ITournamentRequestService _requestService =
            new TournamentRequestService(new TournamentRequestRepository(new SportsBookingDbContext()));


        public UserTournamentRequestListWindow(BusinessObjects.User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            LoadRequests();
        }

        private void LoadRequests()
        {
            var requests = _requestService.GetRequestsByUserId(_currentUser.UserId);
            dgRequests.ItemsSource = requests;
        }

        private void btnCancelSelected_Click(object sender, RoutedEventArgs e)
        {
            if (dgRequests.SelectedItem is TournamentRequest selectedRequest)
            {
                if (selectedRequest.Status != "Chờ duyệt")
                {
                    MessageBox.Show("Chỉ có thể hủy những đơn đang chờ duyệt.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (MessageBox.Show("Bạn có chắc chắn muốn hủy đơn này?", "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
                    return;

                _requestService.UpdateRequestStatus(selectedRequest.RequestId, "Đã hủy");
                MessageBox.Show("Đơn đã được hủy thành công.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                LoadRequests();
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một đơn để hủy.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

    }
}
