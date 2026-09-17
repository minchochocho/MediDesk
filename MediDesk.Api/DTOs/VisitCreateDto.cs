using System.ComponentModel.DataAnnotations;

namespace MediDesk.Api.DTOs {
    public class VisitCreateDto : IValidatableObject {
        [Range(1, int.MaxValue, ErrorMessage = "유효한 환자 번호가 필요합니다.")]
        public int PatientId { get; set; }

        public DateTime VisitDate { get; set; }

        [Required(ErrorMessage = "진료과는 필수입니다.")]
        [StringLength(100, ErrorMessage = "진료과는 100자 이하여야 합니다.")]
        public string Department { get; set; } = string.Empty;

        [Required(ErrorMessage = "담당의는 필수입니다.")]
        [StringLength(100, ErrorMessage = "담당의는 100자 이하여야 합니다.")]
        public string DoctorName { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "증상은 2000자 이하여야 합니다.")]
        public string Symptoms { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "진단은 2000자 이하여야 합니다.")]
        public string Diagnosis { get; set; } = string.Empty;

        [StringLength(2000, ErrorMessage = "메모는 2000자 이하여야 합니다.")]
        public string Memo { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext) {
            if (VisitDate == default) {
                yield return new ValidationResult(
                    "진료일은 필수입니다.",
                    new[] { nameof(VisitDate) }
                );
            }
        }
    }
}