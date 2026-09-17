using MediDesk.Client.Models;
using MediDesk.Client.Services;
using System.Net.Http;
using System.Windows;

namespace MediDesk.Client.Views {
    /// <summary>
    /// PatientVisitWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PatientVisitWindow : Window {
        private readonly Patient _patient;
        private readonly VisitService _visitService;
        public PatientVisitWindow(Patient patient) {
            InitializeComponent();

            _patient = patient;
            _visitService = new VisitService();

            TxtPatientName.Text =
                $"{patient.Name} 환자 진료 기록";

            Loaded += PatientVisitWindow_Loaded;
        }

        private async void PatientVisitWindow_Loaded(
            object sender,
            RoutedEventArgs e) {
            await LoadVisitsAsync();
        }

        private async Task LoadVisitsAsync() {
            try {
                var visits =
                    await _visitService
                        .GetVisitsByPatientAsync(
                            _patient.PatientId
                        );

                VisitDataGrid.ItemsSource = visits;
            } catch (HttpRequestException ex) when (ex.StatusCode is null) {
                MessageBox.Show("API 서버에 연결할 수 없습니다.");
            } catch (HttpRequestException) {
                MessageBox.Show("서버에서 진료 기록 요청을 처리하지 못했습니다.");
            } catch (TaskCanceledException) {
                MessageBox.Show("요청 시간이 초과되었습니다.");
            } catch (Exception) {
                MessageBox.Show("진료 기록을 불러오지 못했습니다.");
            }
        }

        private async void Button_Click(object sender, RoutedEventArgs e) {
            var window = new VisitAddWindow(_patient) {
                Owner = this
            };

            bool? result = window.ShowDialog();

            if (result == true) {
                await LoadVisitsAsync();
            }
        }

        private void VisitDataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e) {
            if (VisitDataGrid.SelectedItem is not Visit visit) {
                return;
            }

            var window = new PrescriptionWindow(visit) {
                Owner = this
            };

            window.ShowDialog();
        }
    }
}
