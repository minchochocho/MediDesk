using MediDesk.Client.Models;
using MediDesk.Client.Services;
using System.Windows;

namespace MediDesk.Client.Views {
    public partial class VisitAddWindow : Window {
        private readonly Patient _patient;
        private readonly VisitService _visitService;

        public VisitAddWindow(Patient patient) {
            InitializeComponent();

            _patient = patient;
            _visitService = new VisitService();

            DpVisitDate.SelectedDate = DateTime.Today;
        }

        private async void BtnSave_Click(
            object sender,
            RoutedEventArgs e) {
            if (DpVisitDate.SelectedDate == null) {
                MessageBox.Show("진료일을 선택해주세요.");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtDepartment.Text)) {
                MessageBox.Show("진료과를 입력해주세요.");
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtDoctorName.Text)) {
                MessageBox.Show("담당의를 입력해주세요.");
                return;
            }

            var visit = new Visit {
                PatientId = _patient.PatientId,

                VisitDate = DateTime.SpecifyKind(
                    DpVisitDate.SelectedDate.Value,
                    DateTimeKind.Utc
                ),

                Department = TxtDepartment.Text,
                DoctorName = TxtDoctorName.Text,
                Symptoms = TxtSymptoms.Text,
                Diagnosis = TxtDiagnosis.Text,
                Memo = TxtMemo.Text
            };

            try {
                bool success =
                    await _visitService.CreateVisitAsync(visit);

                if (success) {
                    MessageBox.Show("진료가 등록되었습니다.");

                    DialogResult = true;
                    Close();
                } else {
                    MessageBox.Show("진료 등록에 실패했습니다.");
                }
            } catch (Exception ex) {
                MessageBox.Show(
                    $"진료 등록 중 오류가 발생했습니다.\n{ex.Message}");
            }
        }

        private void BtnCancel_Click(
            object sender,
            RoutedEventArgs e) {
            Close();
        }
    }
}