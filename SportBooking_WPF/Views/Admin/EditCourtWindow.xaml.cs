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

namespace SportBooking_WPF.Views.Admin
{
    /// <summary>
    /// Interaction logic for EditCourtWindow.xaml
    /// </summary>
    public partial class EditCourtWindow : Window
    {
        private BusinessObjects.Court _court;
        private readonly ICourtService _courtService;
        private bool _isEditMode;
        private readonly ISportService _sportService;
        private readonly ICourtStatusService _courtStatusService;


        public EditCourtWindow(BusinessObjects.Court? court, ICourtService courtService, ISportService sportService, ICourtStatusService courtStatusService)
        {
            InitializeComponent();
            _courtService = courtService;
            _sportService = sportService;
            _courtStatusService = courtStatusService;

            LoadSportComboBox();
            LoadStatusComboBox();

            if (court != null)
            {
                _court = court;
                _isEditMode = true;
                LoadData();
            }
            else
            {
                _court = new BusinessObjects.Court();
                _isEditMode = false;
            }

            
        }

        private void LoadSportComboBox()
        {
            var sports = _sportService.GetAllSports();
            cbSports.ItemsSource = sports;
            cbSports.DisplayMemberPath = "SportName";
            cbSports.SelectedValuePath = "SportId";
        }
        private void LoadStatusComboBox()
        {
            var statuses = _courtStatusService.GetAllStatuses();
            cbStatus.ItemsSource = statuses;
            cbStatus.DisplayMemberPath = "StatusName";
            cbStatus.SelectedValuePath = "StatusId";
        }


        private void LoadData()
        {
            txtCourtName.Text = _court.CourtName;
            txtLocation.Text = _court.Location;
            txtPricePerHour.Text = _court.PricePerHour?.ToString();
            cbStatus.Text = _court.Status;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            _court.CourtName = txtCourtName.Text.Trim();
            _court.Location = txtLocation.Text.Trim();

            // Bổ sung dòng này để gán ID đúng theo combobox
            if (cbSports.SelectedValue != null)
                _court.SportId = (int)cbSports.SelectedValue;

            if (cbStatus.SelectedValue != null)
                _court.StatusId = (int)cbStatus.SelectedValue;

            _court.Status = cbStatus.Text.Trim(); // dòng này có thể giữ hoặc bỏ nếu bạn dùng StatusId là chính

            if (decimal.TryParse(txtPricePerHour.Text, out decimal price))
                _court.PricePerHour = price;
            else
                _court.PricePerHour = null;

            try
            {
                if (_isEditMode)
                {
                    _courtService.UpdateCourt(_court);
                    MessageBox.Show("Cập nhật thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    _courtService.AddCourt(_court);
                    MessageBox.Show("Thêm mới thành công!", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                DialogResult = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + (ex.InnerException?.Message ?? ex.Message), "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
