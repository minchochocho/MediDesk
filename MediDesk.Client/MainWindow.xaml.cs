using MediDesk.Client.Models;
using MediDesk.Client.Services;
using MediDesk.Client.Views;
using System.Windows;

namespace MediDesk.Client {
    public partial class MainWindow : Window {
        private readonly PatientService _patientService;

        public MainWindow() {
            InitializeComponent();

            _patientService = new PatientService();

            Loaded += MainWindow_Loaded;
        }
        private async void MainWindow_Loaded(
           object sender,
           RoutedEventArgs e) {
            await LoadPatientsAsync();
        }
        private async Task LoadPatientsAsync() {
            try {
                var patients =
                    await _patientService.GetPatientsAsync();

                PatientDataGrid.ItemsSource = patients;
            } catch (Exception ex) {
                MessageBox.Show(
                    $"환자 목록을 불러오지 못했습니다.\n{ex.Message}");
            }
        }
        private async void BtnAddPatient_Click(
            object sender,
            RoutedEventArgs e) {
            var window = new PatientAddWindow {
                Owner = this
            };

            bool? result = window.ShowDialog();

            if (result == true) {
                await LoadPatientsAsync();
            }
        }

        // 수정버튼 클릭
        private async void BtnEditPatient_Click(
            object sender,
            RoutedEventArgs e) {
            if (PatientDataGrid.SelectedItem is not Patient selectedPatient) {
                MessageBox.Show("수정할 환자를 선택해주세요.");
                return;
            }

            var window = new PatientEditWindow(selectedPatient) {
                Owner = this
            };

            bool? result = window.ShowDialog();

            if (result == true) {
                await LoadPatientsAsync();
            }
        }

        // 삭제버튼 클릭
        private async void BtnDeletePatient_Click(
            object sender,
            RoutedEventArgs e) {
            if (PatientDataGrid.SelectedItem is not Patient selectedPatient) {
                MessageBox.Show("삭제할 환자를 선택해주세요.");
                return;
            }

            var result = MessageBox.Show(
                $"{selectedPatient.Name} 환자를 삭제하시겠습니까?",
                "환자 삭제",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result != MessageBoxResult.Yes) {
                return;
            }

            bool success =
                await _patientService.DeletePatientAsync(
                    selectedPatient.PatientId
                );

            if (success) {
                MessageBox.Show("환자가 삭제되었습니다.");

                await LoadPatientsAsync();
            } else {
                MessageBox.Show("환자 삭제에 실패했습니다.");
            }
        }

        private void PatientDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (PatientDataGrid.SelectedItem is not Patient selectedPatient) {
                return;
            }

            var window = new PatientVisitWindow(selectedPatient) {
                Owner = this
            };

            window.ShowDialog();
        }
    }
}