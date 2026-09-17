using MediDesk.Api.DTOs;
using System.ComponentModel.DataAnnotations;

namespace MediDesk.Api.Tests;

public class DtoValidationTests {
    [Fact]
    public void PatientCreate_RequiresName() {
        var dto = new PatientCreateDto {
            Name = "",
            BirthDate = new DateTime(1990, 5, 20),
            Gender = "여성"
        };

        Assert.Contains(Validate(dto), HasErrorFor(nameof(dto.Name)));
    }

    [Fact]
    public void PatientCreate_RejectsFutureBirthDate() {
        var dto = new PatientCreateDto {
            Name = "테스트 환자",
            BirthDate = DateTime.UtcNow.Date.AddDays(1),
            Gender = "여성"
        };

        Assert.Contains(Validate(dto), HasErrorFor(nameof(dto.BirthDate)));
    }

    [Fact]
    public void VisitCreate_RequiresPatientId() {
        var dto = new VisitCreateDto {
            PatientId = 0,
            VisitDate = DateTime.UtcNow,
            Department = "내과",
            DoctorName = "테스트 의사"
        };

        Assert.Contains(Validate(dto), HasErrorFor(nameof(dto.PatientId)));
    }

    [Fact]
    public void VisitCreate_RequiresVisitDate() {
        var dto = new VisitCreateDto {
            PatientId = 1,
            Department = "내과",
            DoctorName = "테스트 의사"
        };

        Assert.Contains(Validate(dto), HasErrorFor(nameof(dto.VisitDate)));
    }

    [Fact]
    public void PrescriptionCreate_RejectsInvalidFrequencyAndDays() {
        var dto = new PrescriptionCreateDto {
            VisitId = 1,
            DrugName = "테스트약",
            Dosage = "500mg",
            Frequency = 0,
            Days = 3651
        };

        var errors = Validate(dto);
        Assert.Contains(errors, HasErrorFor(nameof(dto.Frequency)));
        Assert.Contains(errors, HasErrorFor(nameof(dto.Days)));
    }

    [Fact]
    public void PrescriptionCreate_AcceptsValidValues() {
        var dto = new PrescriptionCreateDto {
            VisitId = 1,
            DrugName = "테스트약",
            Dosage = "500mg",
            Frequency = 3,
            Days = 7,
            Instructions = "식후 복용"
        };

        Assert.Empty(Validate(dto));
    }

    private static List<ValidationResult> Validate(object dto) {
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(dto, new ValidationContext(dto), results, true);
        return results;
    }

    private static Predicate<ValidationResult> HasErrorFor(string propertyName) =>
        result => result.MemberNames.Contains(propertyName);
}
