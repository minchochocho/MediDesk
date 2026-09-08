namespace MediDesk.Api.Models {
    public class Visit {
        public int VisitId { get; set; }

        public int PatientId { get; set; }

        public DateTime VisitDate { get; set; }

        public string Department { get; set; } = string.Empty;

        public string DoctorName { get; set; } = string.Empty;

        public string Symptoms { get; set; } = string.Empty;

        public string Diagnosis { get; set; } = string.Empty;

        public string Memo { get; set; } = string.Empty;

        public Patient Patient { get; set; } = null!;

        public ICollection<Prescription> Prescriptions { get; set; }
         = new List<Prescription>();
    }
}