using System.ComponentModel.DataAnnotations;

namespace MediDesk.Api.DTOs {
    public class PatientUpdateDto : IValidatableObject {
        [Required(ErrorMessage = "환자 이름은 필수입니다.")]
        [StringLength(100, ErrorMessage = "환자 이름은 100자 이하여야 합니다.")]
        public string Name { get; set; } = string.Empty;

        public DateTime BirthDate { get; set; }

        [Required(ErrorMessage = "성별은 필수입니다.")]
        [StringLength(20, ErrorMessage = "성별은 20자 이하여야 합니다.")]
        public string Gender { get; set; } = string.Empty;

        [StringLength(30, ErrorMessage = "전화번호는 30자 이하여야 합니다.")]
        public string Phone { get; set; } = string.Empty;

        [StringLength(300, ErrorMessage = "주소는 300자 이하여야 합니다.")]
        public string Address { get; set; } = string.Empty;

        public IEnumerable<ValidationResult> Validate(
            ValidationContext validationContext) {
            if (BirthDate == default) {
                yield return new ValidationResult(
                    "생년월일은 필수입니다.",
                    new[] { nameof(BirthDate) }
                );
            } else if (BirthDate.Date > DateTime.UtcNow.Date) {
                yield return new ValidationResult(
                    "생년월일은 미래 날짜일 수 없습니다.",
                    new[] { nameof(BirthDate) }
                );
            }
        }
    }
}