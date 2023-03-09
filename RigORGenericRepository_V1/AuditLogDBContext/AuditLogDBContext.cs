using Microsoft.EntityFrameworkCore;
using RigorDomain.AuditLog;
using System.Diagnostics.CodeAnalysis;

namespace RigORGenericRepository.AuditLogDBContext
{
    [ExcludeFromCodeCoverage]
    public class AuditLogDBContext : DbContext
    {
        public AuditLogDBContext(DbContextOptions<AuditLogDBContext> options) : base(options)
        {

        }
        public DbSet<ErrorLogMaster> ErrorLogMaster { get; set; }

        public DbSet<AuditTrail> AuditTrail { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {

            base.OnModelCreating(builder);
        }

        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();
            return base.SaveChanges();
        }
    }
}
