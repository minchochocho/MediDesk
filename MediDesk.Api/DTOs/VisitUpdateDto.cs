namespace MediDesk.Api.DTOs {
    public class VisitUpdateDto {
        public DateTime VisitDate { get; set; }

        public string Department { get; set; } = string.Empty;

        public string DoctorName { get; set; } = string.Empty;

        public string Symptoms { get; set; } = string.Empty;

        public string Diagnosis { get; set; } = string.Empty;

        public string Memo { get; set; } = string.Empty;
    }
}