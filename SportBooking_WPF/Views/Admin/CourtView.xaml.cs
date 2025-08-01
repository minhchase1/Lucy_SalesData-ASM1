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
using BusinessObjects;
using Repositories;
namespace SportBooking_WPF.Views.Admin
{
    /// <summary>
    /// Interaction logic for CourtView.xaml
    /// </summary>
    public partial class CourtView : Window
    {
        private readonly IUserService _userService;

            private readonly ICourtService _courtService;
            private readonly ISportService _sportService;
            private readonly ICourtStatusService _courtStatusService;
        private readonly IBookingService _bookingService;
        private List<BusinessObjects.Court> _allCourts;


        public CourtView(ICourtService courtService, ISportService sportService, IUserService userService, ICourtStatusService courtStatusService, IBookingService bookingService)
        {
            InitializeComponent();
            _courtService = courtService;
            _sportService = sportService;
            _userService = userService;
            _courtStatusService = courtStatusService;
            LoadCourtList();
            _bookingService = bookingService;

        }

        private void LoadCourtList()
        {
            try
            {
                _allCourts = (List<BusinessObjects.Court>)_courtService.GetAllCourts();
                dgCourts.ItemsSource = _allCourts;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi tải danh sách sân: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnAddCourt_Click(object sender, RoutedEventArgs e)
        {
            EditCourtWindow editWindow = new EditCourtWindow(null, _courtService, _sportService, _courtStatusService);
            if (editWindow.ShowDialog() == true)
            {
                LoadCourtList();
            }
        }

        private void BtnEditCourt_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is BusinessObjects.Court selectedCourt)
            {
                EditCourtWindow editWindow = new EditCourtWindow(selectedCourt, _courtService, _sportService, _courtStatusService);
                if (editWindow.ShowDialog() == true)
                {
                    LoadCourtList();
                }
            }
        }

        private void BtnDeleteCourt_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.Tag is BusinessObjects.Court selectedCourt)
            {
                var result = MessageBox.Show($"Bạn có chắc chắn muốn xoá sân '{selectedCourt.CourtName}' không?", "Xác nhận xoá", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                if (result == MessageBoxResult.Yes)
                {
                    try
                    {
                        _courtService.DeleteCourt(selectedCourt.CourtId);
                        LoadCourtList();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi xoá sân: " + ex.Message, "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void BtnSearchCourt_Click(object sender, RoutedEventArgs e)
        {
            string keyword = txtSearchCourt.Text.ToLower().Trim();

            if (string.IsNullOrWhiteSpace(keyword) || keyword == "nhập tên hoặc vị trí sân...")
            {
                dgCourts.ItemsSource = _allCourts;
            }
            else
            {
                var filtered = _allCourts.Where(c =>
                    (c.CourtName != null && c.CourtName.ToLower().Contains(keyword)) ||
                    (c.Location != null && c.Location.ToLower().Contains(keyword))
                ).ToList();

                dgCourts.ItemsSource = filtered;
            }
        }


        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            // Mở lại AdminDashboard khi đóng
            var admin = new AdminDashboard(_userService, _courtService, _sportService, _courtStatusService,_bookingService);
            admin.Show();
        }
    }
}
