using MediDesk.Api.Data;
using MediDesk.Api.DTOs;
using MediDesk.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediDesk.Api.Controllers {
    [ApiController]
    [Route("api/prescriptions")]
    public class PrescriptionController : ControllerBase {
        private readonly MediDeskDbContext _context;

        public PrescriptionController(MediDeskDbContext context) {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PrescriptionResponseDto>>> GetPrescriptions() {
            var prescriptions = await _context.Prescriptions
                .AsNoTracking()
                .Select(p => new PrescriptionResponseDto {
                    PrescriptionId = p.PrescriptionId,
                    VisitId = p.VisitId,
                    DrugName = p.DrugName,
                    Dosage = p.Dosage,
                    Frequency = p.Frequency,
                    Days = p.Days,
                    Instructions = p.Instructions
                })
                .ToListAsync();

            return Ok(prescriptions);
        }

        [HttpGet("visit/{visitId}")]
        public async Task<ActionResult<IEnumerable<PrescriptionResponseDto>>>
            GetPrescriptionsByVisit(int visitId) {
            var prescriptions = await _context.Prescriptions
                .AsNoTracking()
                .Where(p => p.VisitId == visitId)
                .Select(p => new PrescriptionResponseDto {
                    PrescriptionId = p.PrescriptionId,
                    VisitId = p.VisitId,
                    DrugName = p.DrugName,
                    Dosage = p.Dosage,
                    Frequency = p.Frequency,
                    Days = p.Days,
                    Instructions = p.Instructions
                })
                .ToListAsync();

            return Ok(prescriptions);
        }

        [HttpPost]
        public async Task<ActionResult<PrescriptionResponseDto>>
            CreatePrescription(PrescriptionCreateDto dto) {
            var visit = await _context.Visits.FindAsync(dto.VisitId);

            if (visit == null) {
                return BadRequest("존재하지 않는 진료 기록입니다.");
            }

            var prescription = new Prescription {
                VisitId = dto.VisitId,
                DrugName = dto.DrugName,
                Dosage = dto.Dosage,
                Frequency = dto.Frequency,
                Days = dto.Days,
                Instructions = dto.Instructions
            };

            _context.Prescriptions.Add(prescription);
            await _context.SaveChangesAsync();

            var response = new PrescriptionResponseDto {
                PrescriptionId = prescription.PrescriptionId,
                VisitId = prescription.VisitId,
                DrugName = prescription.DrugName,
                Dosage = prescription.Dosage,
                Frequency = prescription.Frequency,
                Days = prescription.Days,
                Instructions = prescription.Instructions
            };

            return CreatedAtAction(
                nameof(GetPrescriptionsByVisit),
                new { visitId = prescription.VisitId },
                response
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePrescription(
            int id,
            PrescriptionUpdateDto dto) {
            var prescription = await _context.Prescriptions.FindAsync(id);

            if (prescription == null) {
                return NotFound();
            }

            prescription.DrugName = dto.DrugName;
            prescription.Dosage = dto.Dosage;
            prescription.Frequency = dto.Frequency;
            prescription.Days = dto.Days;
            prescription.Instructions = dto.Instructions;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePrescription(int id) {
            var prescription = await _context.Prescriptions.FindAsync(id);

            if (prescription == null) {
                return NotFound();
            }

            _context.Prescriptions.Remove(prescription);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}