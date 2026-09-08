namespace MediDesk.Api.DTOs {
    public class PrescriptionUpdateDto {
        public string DrugName { get; set; } = string.Empty;

        public string Dosage { get; set; } = string.Empty;

        public int Frequency { get; set; }

        public int Days { get; set; }

        public string Instructions { get; set; } = string.Empty;
    }
}