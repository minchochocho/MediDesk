using MediDesk.Api.Data;
using MediDesk.Api.DTOs;
using MediDesk.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediDesk.Api.Controllers {
    [ApiController]
    [Route("api/visits")]
    public class VisitController : ControllerBase {
        private readonly MediDeskDbContext _context;

        public VisitController(MediDeskDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<VisitResponseDto>>> GetVisits() {
            var visits = await _context.Visits
                .AsNoTracking()
                .Include(v => v.Patient)
                .Select(v => new VisitResponseDto {
                    VisitId = v.VisitId,
                    PatientId = v.PatientId,
                    PatientName = v.Patient.Name,
                    VisitDate = v.VisitDate,
                    Department = v.Department,
                    DoctorName = v.DoctorName,
                    Symptoms = v.Symptoms,
                    Diagnosis = v.Diagnosis,
                    Memo = v.Memo
                })
                .ToListAsync();

            return Ok(visits);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<VisitResponseDto>> GetVisit(int id) {
            var visit = await _context.Visits
                .AsNoTracking()
                .Include(v => v.Patient)
                .Where(v => v.VisitId == id)
                .Select(v => new VisitResponseDto {
                    VisitId = v.VisitId,
                    PatientId = v.PatientId,
                    PatientName = v.Patient.Name,
                    VisitDate = v.VisitDate,
                    Department = v.Department,
                    DoctorName = v.DoctorName,
                    Symptoms = v.Symptoms,
                    Diagnosis = v.Diagnosis,
                    Memo = v.Memo
                })
                .FirstOrDefaultAsync();

            if (visit == null) {
                return NotFound();
            }

            return Ok(visit);
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<IEnumerable<VisitResponseDto>>> GetVisitsByPatient(
            int patientId) {
            var visits = await _context.Visits
                .AsNoTracking()
                .Include(v => v.Patient)
                .Where(v => v.PatientId == patientId)
                .OrderByDescending(v => v.VisitDate)
                .Select(v => new VisitResponseDto {
                    VisitId = v.VisitId,
                    PatientId = v.PatientId,
                    PatientName = v.Patient.Name,
                    VisitDate = v.VisitDate,
                    Department = v.Department,
                    DoctorName = v.DoctorName,
                    Symptoms = v.Symptoms,
                    Diagnosis = v.Diagnosis,
                    Memo = v.Memo
                })
                .ToListAsync();

            return Ok(visits);
        }

        [HttpPost]
        public async Task<ActionResult<VisitResponseDto>> CreateVisit(
            VisitCreateDto dto) {
            var patient = await _context.Patients
                .FindAsync(dto.PatientId);

            if (patient == null) {
                return BadRequest("존재하지 않는 환자입니다.");
            }

            var visit = new Visit {
                PatientId = dto.PatientId,
                VisitDate = dto.VisitDate,
                Department = dto.Department,
                DoctorName = dto.DoctorName,
                Symptoms = dto.Symptoms,
                Diagnosis = dto.Diagnosis,
                Memo = dto.Memo
            };

            _context.Visits.Add(visit);
            await _context.SaveChangesAsync();

            var response = new VisitResponseDto {
                VisitId = visit.VisitId,
                PatientId = visit.PatientId,
                PatientName = patient.Name,
                VisitDate = visit.VisitDate,
                Department = visit.Department,
                DoctorName = visit.DoctorName,
                Symptoms = visit.Symptoms,
                Diagnosis = visit.Diagnosis,
                Memo = visit.Memo
            };

            return CreatedAtAction(
                nameof(GetVisit),
                new { id = visit.VisitId },
                response
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateVisit(
            int id,
            VisitUpdateDto dto) {
            var visit = await _context.Visits.FindAsync(id);

            if (visit == null) {
                return NotFound();
            }

            visit.VisitDate = dto.VisitDate;
            visit.Department = dto.Department;
            visit.DoctorName = dto.DoctorName;
            visit.Symptoms = dto.Symptoms;
            visit.Diagnosis = dto.Diagnosis;
            visit.Memo = dto.Memo;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVisit(int id) {
            var visit = await _context.Visits.FindAsync(id);

            if (visit == null) {
                return NotFound();
            }

            _context.Visits.Remove(visit);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}