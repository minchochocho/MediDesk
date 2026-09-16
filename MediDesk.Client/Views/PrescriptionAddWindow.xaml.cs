using MediDesk.Client.Models;
using MediDesk.Client.Services;
using System.Windows;


namespace MediDesk.Client.Views {
    public partial class PrescriptionAddWindow : Window {
        private readonly Visit _visit;
        private readonly PrescriptionService _prescriptionService;

        public PrescriptionAddWindow(Visit visit) {
            InitializeComponent();

            _visit = visit;
            _prescriptionService = new PrescriptionService();
        }

        private async void BtnSave_Click(
            object sender,
            RoutedEventArgs e
            ) {
            if (string.IsNullOrWhiteSpace(TxtDrugName.Text)) {
                MessageBox.Show("약 이름을 입력해주세요.");
                TxtDrugName.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(TxtDosage.Text)) {
                MessageBox.Show("용량을 입력해주세요.");
                TxtDosage.Focus();
                return;
            }

            if (!int.TryParse(TxtFrequency.Text, out int frequency)
                || frequency <= 0) {
                MessageBox.Show("복용 횟수는 1 이상의 숫자로 입력해주세요.");
                TxtFrequency.Focus();
                return;
            }

            if (!int.TryParse(TxtDays.Text, out int days)
                || days <= 0) {
                MessageBox.Show("복용 일수는 1 이상의 숫자로 입력해주세요.");
                TxtDays.Focus();
                return;
            }

            var prescription = new Prescription {
                VisitId = _visit.VisitId,
                DrugName = TxtDrugName.Text.Trim(),
                Dosage = TxtDosage.Text.Trim(),
                Frequency = frequency,
                Days = days,
                Instructions = TxtInstructions.Text.Trim()
            };

            try {
                bool success = await _prescriptionService.CreatePrescriptionAsync(prescription);

                if (!success) {
                    MessageBox.Show("처방 등록에 실패했습니다.");
                    return;
                }

                MessageBox.Show("처방이 등록되었습니다.");
                DialogResult = true;
                Close();
            } catch (Exception ex) {
                MessageBox.Show(
                    $"처방 등록 중 오류가 발생했습니다.\n{ex.Message}"
                    );
            }
        }

        private void BtnCancel_Click(
            object sender,
            RoutedEventArgs e) {
            Close();
        }
    }
}

