using MediDesk.Client.Models;
using MediDesk.Client.Services;
using System.Windows;

namespace MediDesk.Client.Views {
    public partial class PrescriptionWindow : Window {
        private readonly Visit _visit;
        private readonly PrescriptionService _prescriptionService;

        public PrescriptionWindow(Visit visit) {
            InitializeComponent();

            _visit = visit;
            _prescriptionService = new PrescriptionService();

            TxtVisitInfo.Text =
                $"{visit.PatientName} - {visit.Department} 처방 내역";

            Loaded += PrescriptionWindow_Loaded;
        }

        private async void PrescriptionWindow_Loaded(
            object sender,
            RoutedEventArgs e) {
            await LoadPrescriptionsAsync();
        }

        private async Task LoadPrescriptionsAsync() {
            try {
                var prescriptions =
                    await _prescriptionService
                        .GetPrescriptionsByVisitAsync(
                            _visit.VisitId
                        );

                PrescriptionDataGrid.ItemsSource = prescriptions;
            } catch (Exception ex) {
                MessageBox.Show(
                    $"처방 내역을 불러오지 못했습니다.\n{ex.Message}");
            }
        }

        private async void BtnAddPrescription_Click(
            object sender,
            RoutedEventArgs e) {
            var window = new PrescriptionAddWindow(_visit) {
                Owner = this
            };

            bool? result = window.ShowDialog();

            if (result == true) {
                await LoadPrescriptionsAsync();
            }
        }
    }
}