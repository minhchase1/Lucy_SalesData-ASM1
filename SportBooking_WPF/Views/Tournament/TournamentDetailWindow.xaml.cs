using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for TournamentDetailWindow.xaml
    /// </summary>
    public partial class TournamentDetailWindow : Window
    {
        private readonly BusinessObjects.User _currentUser;
        private readonly BusinessObjects.Tournament _tournament;

        public TournamentDetailWindow(BusinessObjects.User currentUser, BusinessObjects.Tournament tournament)
        {
            InitializeComponent();
            _currentUser = currentUser;
            _tournament = tournament;
            DataContext = _tournament;
            IsTeamBasedTextBlock.Text = _tournament.IsTeamBased switch
            {
                true => "Có",
                false => "Không",
                null => "Không rõ"
            };

        }

        private void Register_Click(object sender, RoutedEventArgs e)
        {
            var registerWindow = new TournamentRegisterWindow(_currentUser, _tournament.TournamentId);
            registerWindow.ShowDialog();
        }
        
    }

}
