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
    /// Interaction logic for ViewRegistrationWindow.xaml
    /// </summary>
    public partial class ViewRegistrationWindow : Window
    {
        private readonly BusinessObjects.User _currentUser;
        private readonly ITournamentService _tournamentService =
    new TournamentService(new TournamentRepository(new SportsBookingDbContext()));

        private readonly ITournamentRegistrationService _registrationService =
            new TournamentRegistrationService(new TournamentRegistrationRepository(new SportsBookingDbContext()));


        public ViewRegistrationWindow(BusinessObjects.User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            Loaded += ViewRegistrationWindow_Loaded;
        }

        private void ViewRegistrationWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (_currentUser != null)
            {
                if (_currentUser.RoleId == 2 || _currentUser.RoleId == 3 || _currentUser.RoleId == 4)
                {
                    // Manager, Staff, Admin thấy dropdown chọn giải
                    cbTournaments.Visibility = Visibility.Visible;

                    var tournaments = _tournamentService.GetAllTournaments();
                    cbTournaments.ItemsSource = tournaments;

                    if (tournaments != null && tournaments.Any())
                    {
                        cbTournaments.SelectedIndex = 0;
                    }
                }
                else if (_currentUser.RoleId == 1)
                {
                    // Chỉ user thường => load đơn của chính họ
                    LoadUserRegistrations();
                }
            }
        }


        private void LoadUserRegistrations()
        {
            var registrations = _registrationService.GetUserRegistrations(_currentUser.UserId);
            dgMyRegistrations.ItemsSource = registrations;
        }

        private void cbTournaments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbTournaments.SelectedItem is BusinessObjects.Tournament selectedTournament)
            {
                var registrations = _registrationService.GetRegistrationsByTournament(selectedTournament.TournamentId);
                dgMyRegistrations.ItemsSource = registrations;
            }
        }
        private void btnCancelRegistration_Click(object sender, RoutedEventArgs e)
        {
            if (dgMyRegistrations.SelectedItem is BusinessObjects.TournamentRegistration selectedRegistration)
            {
                if (selectedRegistration.Status == "Đang chờ xác nhận")
                {
                    var confirm = MessageBox.Show("Bạn có chắc chắn muốn hủy đơn đăng ký này?", "Xác nhận hủy", MessageBoxButton.YesNo);
                    if (confirm == MessageBoxResult.Yes)
                    {
                        bool result = _registrationService.UpdateRegistrationStatus(selectedRegistration.RegistrationId, "Đã hủy");
                        if (result)
                        {
                            MessageBox.Show("Hủy đăng ký thành công.");

                            // Refresh lại danh sách
                            if (_currentUser.RoleId == 1)
                                LoadUserRegistrations();
                            else if (cbTournaments.SelectedItem is BusinessObjects.Tournament selectedTournament)
                                dgMyRegistrations.ItemsSource = _registrationService.GetRegistrationsByTournament(selectedTournament.TournamentId);
                        }
                        else
                        {
                            MessageBox.Show("Hủy đăng ký thất bại.");
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Chỉ có thể hủy đơn khi đang chờ xác nhận.");
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn một đơn đăng ký để hủy.");
            }
        }

    }
}
