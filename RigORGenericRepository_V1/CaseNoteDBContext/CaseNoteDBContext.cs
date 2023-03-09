using Microsoft.EntityFrameworkCore;
using RigorDomain.CaseNote;

namespace RigORGenericRepository.CaseNoteDBContext
{
    public class CaseNoteDBContext : DbContext
    {
        public CaseNoteDBContext(DbContextOptions<CaseNoteDBContext> options) : base(options)
        {

        }
        public DbSet<CaseNotes> CaseNotes { get; set; }
        public DbSet<CaseNoteType> CaseNoteType { get; set; }


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
