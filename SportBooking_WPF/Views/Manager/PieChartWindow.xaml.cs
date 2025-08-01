using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using LiveChartsCore.SkiaSharpView.Painting;
using Services;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for PieChartWindow.xaml
    /// </summary>
    public partial class PieChartWindow : Window
    {
        public ObservableCollection<ISeries> Series { get; set; }

        public PieChartWindow(IBookingService bookingService)
        {
            InitializeComponent();

            var statistics = bookingService.GetBookingStatisticsByCourt();

            // Một vài màu định nghĩa trước
            var colors = new[]
            {
                SKColors.CornflowerBlue,
                SKColors.MediumSeaGreen,
                SKColors.OrangeRed,
                SKColors.MediumPurple,
                SKColors.Gold,
                SKColors.Tomato,
                SKColors.Teal
            };

            Series = new ObservableCollection<ISeries>(
                statistics.Select((item, index) =>
                    new PieSeries<int>
                    {
                        Name = item.CourtName,
                        Values = new[] { item.BookingCount },
                        DataLabelsPosition = LiveChartsCore.Measure.PolarLabelsPosition.Middle,
                        DataLabelsPaint = new SolidColorPaint(SKColors.White),
                        Fill = new SolidColorPaint(colors[index % colors.Length]),
                        Stroke = null
                    }));

            DataContext = this;
        }
    }
}

