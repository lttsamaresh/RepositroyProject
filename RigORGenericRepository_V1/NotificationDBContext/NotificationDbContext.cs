using Microsoft.EntityFrameworkCore;
using RigorDomain.Notification;
using System.Diagnostics.CodeAnalysis;

namespace RigORGenericRepository.NotificationDBContext
{
    [ExcludeFromCodeCoverage]
    public class NotificationDbContext : DbContext
    {
        public NotificationDbContext(DbContextOptions<NotificationDbContext> options) : base(options)
        {

        }

        public DbSet<Notification> Notification { get; set; }
       // public DbSet<logs> logs { get; set; }
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
