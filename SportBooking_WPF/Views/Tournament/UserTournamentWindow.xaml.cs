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
    /// Interaction logic for UserTournamentWindow.xaml
    /// </summary>
    public partial class UserTournamentWindow : Window
    {
        private readonly BusinessObjects.User _currentUser;
        private readonly ITournamentService tournamentService =
            new TournamentService(new TournamentRepository(new SportsBookingDbContext()));


        public UserTournamentWindow(BusinessObjects.User currentUser)
        {
            InitializeComponent();
            _currentUser = currentUser;
            Loaded += UserTournamentWindow_Loaded;
        }

        private void UserTournamentWindow_Loaded(object sender, RoutedEventArgs e)
        {
            var tournaments = tournamentService.GetAllTournaments();
            icTournaments.ItemsSource = tournaments;
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is BusinessObjects.Tournament selected)
            {
                var detailWindow = new TournamentDetailWindow(_currentUser, selected);
                detailWindow.ShowDialog();
            }
        }

        private void btnViewMyRegistrations_Click(object sender, RoutedEventArgs e)
        {
            var viewWindow = new ViewRegistrationWindow(_currentUser);
            viewWindow.ShowDialog();
        }
        private void btnCreateTournamentRequest_Click(object sender, RoutedEventArgs e)
        {
            var requestWindow = new TournamentRequestWindow(_currentUser);
            requestWindow.ShowDialog();
        }
        private void btnViewMyTournamentRequests_Click(object sender, RoutedEventArgs e)
        {
            var viewRequestWindow = new UserTournamentRequestListWindow(_currentUser);
            viewRequestWindow.ShowDialog();
        }

    }
}
