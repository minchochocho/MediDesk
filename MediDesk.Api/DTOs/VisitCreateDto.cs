namespace MediDesk.Api.DTOs {
    public class VisitCreateDto {
        public int PatientId { get; set; }

        public DateTime VisitDate { get; set; }

        public string Department { get; set; } = string.Empty;

        public string DoctorName { get; set; } = string.Empty;

        public string Symptoms { get; set; } = string.Empty;

        public string Diagnosis { get; set; } = string.Empty;

        public string Memo { get; set; } = string.Empty;
    }
}