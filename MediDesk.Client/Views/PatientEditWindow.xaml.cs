using MediDesk.Client.Models;
using MediDesk.Client.Services;
using System.Windows;
using System.Windows.Controls;

namespace MediDesk.Client.Views {
    public partial class PatientEditWindow : Window {
        private readonly PatientService _patientService;
        private readonly Patient _patient;

        public PatientEditWindow(Patient patient) {
            InitializeComponent();

            _patientService = new PatientService();
            _patient = patient;

            TxtName.Text = patient.Name;
            DpBirthDate.SelectedDate = patient.BirthDate;
            TxtPhone.Text = patient.Phone;
            TxtAddress.Text = patient.Address;

            foreach (ComboBoxItem item in CbGender.Items) {
                if (item.Content?.ToString() == patient.Gender) {
                    CbGender.SelectedItem = item;
                    break;
                }
            }
        }

        private async void BtnSave_Click(
            object sender,
            RoutedEventArgs e) {
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

            var updatedPatient = new Patient {
                PatientId = _patient.PatientId,
                Name = TxtName.Text,
                BirthDate = DateTime.SpecifyKind(
                    DpBirthDate.SelectedDate.Value,
                    DateTimeKind.Utc
                ),
                Gender = genderItem.Content?.ToString() ?? "",
                Phone = TxtPhone.Text,
                Address = TxtAddress.Text,
                CreatedAt = _patient.CreatedAt
            };

            bool success =
                await _patientService.UpdatePatientAsync(
                    _patient.PatientId,
                    updatedPatient
                );

            if (success) {
                MessageBox.Show("환자 정보가 수정되었습니다.");

                DialogResult = true;
                Close();
            } else {
                MessageBox.Show("환자 수정에 실패했습니다.");
            }
        }

        private void BtnCancel_Click(
            object sender,
            RoutedEventArgs e) {
            Close();
        }
    }
}