namespace MediDesk.Client.Models {
    public class Prescription {
        public int PrescriptionId { get; set; }

        public int VisitId { get; set; }

        public string DrugName { get; set; } = string.Empty;

        public string Dosage { get; set; } = string.Empty;

        public int Frequency { get; set; }

        public int Days { get; set; }

        public string Instructions { get; set; } = string.Empty;
    }
}