using BusinessObjects;
using Repositories;
using Services;
using SportBooking_WPF.Views.Manager;
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
namespace SportBooking_WPF.Views.Manager
{
    /// <summary>
    /// Interaction logic for RevenueReportWindow.xaml
    /// </summary>
    public partial class RevenueReportWindow : Window
    {
        private readonly IReportService _reportService;
        private readonly IBookingService _bookingService;

        public RevenueReportWindow()
        {
            InitializeComponent();

            // Khởi tạo repository
            IReportRepository reportRepo = new ReportRepository();
            IBookingRepository bookingRepo = new BookingRepository(new DataAccessLayer.SportsBookingDbContext());

            // Khởi tạo service và truyền repository tương ứng
            _reportService = new ReportService(reportRepo);
            _bookingService = new BookingService(bookingRepo);
            LoadData();
        }

        private void LoadData()
        {
            List<RevenueReport> data = _reportService.GetDailyRevenue();
            dgRevenue.ItemsSource = data;
        }

        private void BtnDaily_Click(object sender, RoutedEventArgs e)
        {
            LoadData();
        }

        private void BtnMonthly_Click(object sender, RoutedEventArgs e)
        {
            List<RevenueReport> data = _reportService.GetMonthlyRevenue();
            dgRevenue.ItemsSource = data;
        }

        private void BtnChart_Click(object sender, RoutedEventArgs e)
        {
            var chartWindow = new PieChartWindow(_bookingService);
            chartWindow.ShowDialog();
        }
    }
}
