using Microsoft.EntityFrameworkCore;
using RigorDomain.Configuration.CaseNoteConfiguration;
using RigorDomain.Configuration.MessageConfiguration;
using RigorDomain.Configuration.NotificationConfiguration;
using RigorDomain.Configuration.SmtpConfiguration;
using System.Diagnostics.CodeAnalysis;

namespace RigORGenericRepository.ConfigurationDBContext
{
    [ExcludeFromCodeCoverage]
   
    public class ConfigurationDBContext : DbContext
    {
        public ConfigurationDBContext(DbContextOptions<ConfigurationDBContext> options) : base(options)
        {

        }
        public DbSet<CaseNoteStatus> CaseNoteStatus { get; set; }
        public DbSet<MessageTemplate> MessageTemplate { get; set; }

        public DbSet<SmtpConfig> SmtpConfig { get; set; }

        public DbSet<MessageTemplateTags> MessageTemplateTags { get; set; }

        public DbSet<NotificationType> NotificationType { get; set; }

        public DbSet<NotificationConfig> NotificationConfig { get; set; }


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
