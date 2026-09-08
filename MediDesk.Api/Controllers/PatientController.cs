using MediDesk.Api.Data;
using MediDesk.Api.DTOs;
using MediDesk.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediDesk.Api.Controllers {
    [ApiController]
    [Route("api/patients")]
    public class PatientController : ControllerBase {
        private readonly MediDeskDbContext _context;

        public PatientController(MediDeskDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PatientResponseDto>>> GetPatients() {
            var patients = await _context.Patients
                .AsNoTracking()
                .Select(p => new PatientResponseDto {
                    PatientId = p.PatientId,
                    Name = p.Name,
                    BirthDate = p.BirthDate,
                    Gender = p.Gender,
                    Phone = p.Phone,
                    Address = p.Address,
                    CreatedAt = p.CreatedAt
                })
                .ToListAsync();

            return Ok(patients);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PatientResponseDto>> GetPatient(int id) {
            var patient = await _context.Patients
                .AsNoTracking()
                .Where(p => p.PatientId == id)
                .Select(p => new PatientResponseDto {
                    PatientId = p.PatientId,
                    Name = p.Name,
                    BirthDate = p.BirthDate,
                    Gender = p.Gender,
                    Phone = p.Phone,
                    Address = p.Address,
                    CreatedAt = p.CreatedAt
                })
                .FirstOrDefaultAsync();

            if (patient == null) {
                return NotFound();
            }

            return Ok(patient);
        }

        [HttpPost]
        public async Task<ActionResult<PatientResponseDto>> CreatePatient(
            PatientCreateDto dto) {
            var patient = new Patient {
                Name = dto.Name,
                BirthDate = dto.BirthDate,
                Gender = dto.Gender,
                Phone = dto.Phone,
                Address = dto.Address,
                CreatedAt = DateTime.UtcNow
            };

            _context.Patients.Add(patient);
            await _context.SaveChangesAsync();

            var response = new PatientResponseDto {
                PatientId = patient.PatientId,
                Name = patient.Name,
                BirthDate = patient.BirthDate,
                Gender = patient.Gender,
                Phone = patient.Phone,
                Address = patient.Address,
                CreatedAt = patient.CreatedAt
            };

            return CreatedAtAction(
                nameof(GetPatient),
                new { id = patient.PatientId },
                response
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(
            int id,
            PatientUpdateDto dto) {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null) {
                return NotFound();
            }

            patient.Name = dto.Name;
            patient.BirthDate = dto.BirthDate;
            patient.Gender = dto.Gender;
            patient.Phone = dto.Phone;
            patient.Address = dto.Address;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id) {
            var patient = await _context.Patients.FindAsync(id);

            if (patient == null) {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}