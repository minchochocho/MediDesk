using MediDesk.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace MediDesk.Api.Data {
    public class MediDeskDbContext : DbContext {
        public MediDeskDbContext(
            DbContextOptions<MediDeskDbContext> options)
            : base(options) {
        }

        public DbSet<Patient> Patients { get; set; }
        public DbSet<Visit> Visits { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
    }
}