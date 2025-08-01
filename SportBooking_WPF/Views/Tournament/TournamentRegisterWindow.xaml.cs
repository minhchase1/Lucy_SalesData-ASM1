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
    /// Interaction logic for TournamentRegisterWindow.xaml
    /// </summary>
    public partial class TournamentRegisterWindow : Window
    {
        private readonly BusinessObjects.User _currentUser;
        private readonly int _tournamentId;
        private readonly ITournamentService _tournamentService = new TournamentService(new TournamentRepository(new SportsBookingDbContext()));
        private readonly ITournamentRegistrationService _registrationService =
            new TournamentRegistrationService(new TournamentRegistrationRepository(new SportsBookingDbContext()));


        public TournamentRegisterWindow(BusinessObjects.User currentUser, int tournamentId)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _tournamentId = tournamentId;

            LoadTournamentInfo();
        }

        private void LoadTournamentInfo()
        {
            var tournament = _tournamentService.GetTournamentById(_tournamentId);
            if (tournament != null)
            {
                txtRegistrantName.Text = _currentUser.FullName;
                txtTournamentTitle.Text = tournament.Title;
                txtRegisterDate.Text = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
            }
            else
            {
                MessageBox.Show("Không tìm thấy thông tin giải đấu.");
                Close();
            }
        }

        private void btnRegister_Click(object sender, RoutedEventArgs e)
        {
            string? teamName = string.IsNullOrWhiteSpace(txtTeamName.Text) ? null : txtTeamName.Text.Trim();

            int? numberOfMembers = null;
            if (!string.IsNullOrWhiteSpace(txtMembers.Text))
            {
                if (int.TryParse(txtMembers.Text, out int parsed))
                {
                    numberOfMembers = parsed;
                }
                else
                {
                    MessageBox.Show("Số thành viên không hợp lệ.");
                    return;
                }
            }

            var success = _registrationService.RegisterUser(_currentUser.UserId, _tournamentId, teamName, numberOfMembers);

            if (success)
            {
                MessageBox.Show("Đăng ký thành công.");
                Close();
            }
            else
            {
                MessageBox.Show("Đăng ký thất bại. Có thể đã đủ số lượng hoặc bạn đã đăng ký trước đó.");
            }
        }
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}
