using MediDesk.Client.Models;
using MediDesk.Client.Services;
using System.Windows;
using System.Windows.Controls;

namespace MediDesk.Client.Views {
    public partial class PatientAddWindow : Window {
        private readonly PatientService _patientService;

        public PatientAddWindow() {
            InitializeComponent();
            _patientService = new PatientService();
        }

        private async void BtnSave_Click(object sender, RoutedEventArgs e) {
            if (string.IsNullOrWhiteSpace(TxtName.Text)) {
                MessageBox.Show("이름을 입력해주세요.");
                return;
            }

            if (DpBirthDate.SelectedDate == null) {
                MessageBox.Show("생년월일을 선택해주세요.");
                return;
            }

            if (CbGender.SelectedItem is not ComboBoxItem genderItem) {
                MessageBox.Show("성별을 선택해주세요.");
                return;
            }

            var patient = new Patient {
                Name = TxtName.Text,
                BirthDate = DateTime.SpecifyKind(
                    DpBirthDate.SelectedDate.Value,
                    DateTimeKind.Utc
                ),
                Gender = genderItem.Content?.ToString() ?? "",
                Phone = TxtPhone.Text,
                Address = TxtAddress.Text
            };

            try {
                bool success =
                    await _patientService.CreatePatientAsync(patient);

                if (success) {
                    MessageBox.Show("환자가 등록되었습니다.");

                    DialogResult = true;
                    Close();
                } else {
                    MessageBox.Show("환자 등록에 실패했습니다.");
                }
            } catch (Exception ex) {
                MessageBox.Show(
                    $"환자 등록 중 오류가 발생했습니다.\n{ex.Message}");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e) {
            Close();
        }
    }
}