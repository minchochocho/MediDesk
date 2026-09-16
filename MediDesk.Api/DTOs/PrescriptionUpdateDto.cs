using System.ComponentModel.DataAnnotations;

namespace MediDesk.Api.DTOs {
    public class PrescriptionUpdateDto {
        [Required(ErrorMessage = "약 이름은 필수입니다.")]
        [StringLength(100, ErrorMessage = "약 이름은 100자 이하여야 합니다.")]
        public string DrugName { get; set; } = string.Empty;

        [Required(ErrorMessage = "용량은 필수입니다.")]
        [StringLength(100, ErrorMessage = "용량은 100자 이하여야 합니다.")]
        public string Dosage { get; set; } = string.Empty;

        [Range(1, 100, ErrorMessage = "복용 횟수는 1에서 100 사이여야 합니다.")]
        public int Frequency { get; set; }

        [Range(1, 3650, ErrorMessage = "복용 일수는 1에서 3650 사이여야 합니다.")]
        public int Days { get; set; }

        [StringLength(500, ErrorMessage = "복용 안내는 500자 이하여야 합니다.")]
        public string Instructions { get; set; } = string.Empty;
    }
}