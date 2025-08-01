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
    /// Interaction logic for TournamentRequestWindow.xaml
    /// </summary>
    public partial class TournamentRequestWindow : Window
    {
        BusinessObjects.User _currentUser;
        ISportService _sportService = new SportService(new SportRepository(new SportsBookingDbContext()));
        ITournamentRequestService _requestService = new TournamentRequestService(new TournamentRequestRepository(new SportsBookingDbContext()));


        public TournamentRequestWindow(BusinessObjects.User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;

            txtUserName.Text = _currentUser.FullName;
            cbSport.ItemsSource = _sportService.GetAllSports();
            cbSport.SelectedIndex = 0;
        }

        private void SubmitRequest_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text))
            {
                MessageBox.Show("Vui lòng nhập tên giải đấu.");
                return;
            }

            if (cbSport.SelectedItem == null)
            {
                MessageBox.Show("Vui lòng chọn môn thể thao.");
                return;
            }

            if (dpStartDate.SelectedDate == null || dpEndDate.SelectedDate == null)
            {
                MessageBox.Show("Vui lòng chọn ngày bắt đầu và ngày kết thúc.");
                return;
            }

            DateTime startDate = dpStartDate.SelectedDate.Value;
            DateTime endDate = dpEndDate.SelectedDate.Value;

            if (startDate > endDate)
            {
                MessageBox.Show("Ngày bắt đầu không thể sau ngày kết thúc.");
                return;
            }

            if (dpDeadline.SelectedDate != null)
            {
                DateTime deadline = dpDeadline.SelectedDate.Value;
                if (deadline >= startDate)
                {
                    MessageBox.Show("Hạn đăng ký phải trước ngày bắt đầu.");
                    return;
                }
            }

            int? maxParticipants = null;
            if (!string.IsNullOrWhiteSpace(txtMaxParticipants.Text))
            {
                if (!int.TryParse(txtMaxParticipants.Text.Trim(), out int parsed))
                {
                    MessageBox.Show("Số lượng người tham gia tối đa phải là số nguyên.");
                    return;
                }

                if (parsed <= 0)
                {
                    MessageBox.Show("Số lượng tối đa phải lớn hơn 0.");
                    return;
                }

                maxParticipants = parsed;
            }

            var request = new TournamentRequest
            {
                UserId = _currentUser.UserId,
                Title = txtTitle.Text.Trim(),
                SportId = (int)cbSport.SelectedValue,
                Description = txtDescription.Text.Trim(),
                Location = txtLocation.Text.Trim(),
                StartDate = DateOnly.FromDateTime(startDate),
                EndDate = DateOnly.FromDateTime(endDate),
                RegistrationDeadline = dpDeadline.SelectedDate != null
                    ? DateOnly.FromDateTime(dpDeadline.SelectedDate.Value)
                    : null,
                IsTeamBased = chkIsTeamBased.IsChecked ?? false,
                Rules = txtRules.Text.Trim(),
                RequestDate = DateTime.Now,
                Status = "Chờ duyệt",
                MaxParticipants = maxParticipants
            };

            bool result = _requestService.AddRequest(request);

            if (result)
            {
                MessageBox.Show("Gửi đơn thành công. Vui lòng chờ quản trị viên duyệt!");
                this.Close();
            }
            else
            {
                MessageBox.Show("Gửi đơn thất bại. Vui lòng thử lại.");
            }
        }

    }
}
