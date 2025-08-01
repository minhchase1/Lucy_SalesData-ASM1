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
        /// Interaction logic for TournamentEditDialog.xaml
        /// </summary>
        public partial class TournamentEditDialog : Window
        {
        ISportService sportService = new SportService(new SportRepository(new SportsBookingDbContext()));

        public BusinessObjects.Tournament Tournament { get; private set; }
            private readonly BusinessObjects.User _currentUser;
            private readonly bool _isEdit;

            public TournamentEditDialog(BusinessObjects.User currentUser)
            {
                InitializeComponent();
                _currentUser = currentUser;
                Tournament = new BusinessObjects.Tournament();
                _isEdit = false;

                Loaded += TournamentEditDialog_Loaded;
            }

            public TournamentEditDialog(BusinessObjects.Tournament tournament, BusinessObjects.User currentUser)
            {
                InitializeComponent();
                _currentUser = currentUser;
                Tournament = tournament;
                _isEdit = true;

                Loaded += TournamentEditDialog_Loaded;
            }

            private void TournamentEditDialog_Loaded(object sender, RoutedEventArgs e)
            {
                cbSports.DisplayMemberPath = "SportName";
                cbSports.SelectedValuePath = "SportId";
                cbSports.ItemsSource = sportService.GetAllSports();

                // Ẩn phần người tổ chức vì lấy tự động từ current user
                cbOrganizers.Visibility = Visibility.Collapsed;
                lblOrganizer.Visibility = Visibility.Collapsed;

                cbTeamType.SelectionChanged += (s, ev) => UpdateMaxLabel();

                if (_isEdit && Tournament != null)
                {
                    txtTitle.Text = Tournament.Title;
                    cbSports.SelectedValue = Tournament.SportId;
                    txtDescription.Text = Tournament.Description;

                    dpStartDate.SelectedDate = Tournament.StartDate.ToDateTime(TimeOnly.MinValue);
                    dpEndDate.SelectedDate = Tournament.EndDate.ToDateTime(TimeOnly.MinValue);
                    dpRegistrationDeadline.SelectedDate = Tournament.RegistrationDeadline?.ToDateTime(TimeOnly.MinValue);

                    txtLocation.Text = Tournament.Location;
                    txtMaxParticipants.Text = Tournament.MaxParticipants?.ToString();
                    txtRules.Text = Tournament.Rules;

                    foreach (ComboBoxItem item in cbTeamType.Items)
                    {
                        if (item.Tag?.ToString() == (Tournament.IsTeamBased == true ? "true" : "false"))
                        {
                            cbTeamType.SelectedItem = item;
                            break;
                        }
                    }

                    foreach (ComboBoxItem item in cbStatus.Items)
                    {
                        if (item.Tag?.ToString() == Tournament.Status)
                        {
                            cbStatus.SelectedItem = item;
                            break;
                        }
                    }

                    UpdateMaxLabel();
                }
            }

            private void UpdateMaxLabel()
            {
                string selected = (cbTeamType.SelectedItem as ComboBoxItem)?.Tag?.ToString();
                lblMaxParticipants.Text = selected == "true" ? "Số đội tối đa:" : "Số người tối đa:";
            }

            private void BtnSave_Click(object sender, RoutedEventArgs e)
            {
                try
                {
                    // Lấy ngày
                    var startDate = dpStartDate.SelectedDate ?? DateTime.MinValue;
                    var endDate = dpEndDate.SelectedDate ?? DateTime.MinValue;
                    var regDeadline = dpRegistrationDeadline.SelectedDate ?? DateTime.MinValue;
                    var today = DateTime.Today;

                    // Kiểm tra ngày bắt đầu
                    if (startDate < today)
                    {
                        MessageBox.Show("Ngày bắt đầu phải từ hôm nay trở đi.");
                        return;
                    }

                    // Kiểm tra ngày kết thúc
                    if (endDate < startDate)
                    {
                        MessageBox.Show("Ngày kết thúc phải sau hoặc bằng ngày bắt đầu.");
                        return;
                    }

                    // Kiểm tra hạn đăng ký
                    if (dpRegistrationDeadline.SelectedDate != null)
                    {
                        if (regDeadline < today)
                        {
                            MessageBox.Show("Hạn đăng ký không được nhỏ hơn hôm nay.");
                            return;
                        }

                        if (regDeadline >= startDate)
                        {
                            MessageBox.Show("Hạn đăng ký phải trước ngày bắt đầu.");
                            return;
                        }

                    }

                    // Tiếp tục lưu nếu hợp lệ
                    Tournament.Title = txtTitle.Text.Trim();
                    Tournament.SportId = (int?)cbSports.SelectedValue ?? 0;
                    Tournament.OrganizerId = _currentUser.UserId;
                    Tournament.Description = txtDescription.Text.Trim();
                    Tournament.StartDate = DateOnly.FromDateTime(startDate);
                    Tournament.EndDate = DateOnly.FromDateTime(endDate);
                    Tournament.RegistrationDeadline = dpRegistrationDeadline.SelectedDate != null
                        ? DateOnly.FromDateTime(regDeadline)
                        : null;
                    Tournament.Location = txtLocation.Text.Trim();
                    Tournament.Rules = txtRules.Text.Trim();

                    Tournament.IsTeamBased = bool.TryParse((cbTeamType.SelectedItem as ComboBoxItem)?.Tag?.ToString(), out bool isTeam) && isTeam;
                    Tournament.Status = (cbStatus.SelectedItem as ComboBoxItem)?.Tag?.ToString();

                    if (int.TryParse(txtMaxParticipants.Text, out int max))
                    {
                        Tournament.MaxParticipants = max;
                    }
                    else
                    {
                        MessageBox.Show("Số lượng không hợp lệ.");
                        return;
                    }

                    DialogResult = true;
                    Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message);
                }
            }


            private void BtnCancel_Click(object sender, RoutedEventArgs e)
            {
                DialogResult = false;
                Close();
            }
    }
}
