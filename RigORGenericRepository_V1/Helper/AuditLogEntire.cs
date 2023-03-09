using MassTransit;
using Microsoft.EntityFrameworkCore;
using RigorDomain.AuditLog;
using RigorDomain.Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace RigORGenericRepository.Helper
{
    public class AuditLogEntire
    {
        protected readonly DbContext _dbContext;
        public AuditLogEntire(DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async void OnBeforeSaveChanges(long? UserId = 0)
        {
            try
            {
                List<Audit> auditEntries = new List<Audit>();
                _dbContext.ChangeTracker.DetectChanges();
                foreach (var entry in _dbContext.ChangeTracker.Entries())
                {

                    if (entry.State == EntityState.Detached || entry.State == EntityState.Unchanged)
                        continue;
                    Audit auditEntry = new Audit();

                    auditEntry.TableName = entry.Entity.GetType().Name;
                    auditEntry.UserId = UserId;
                    auditEntries.Add(auditEntry);
                    foreach (var property in entry.Properties)
                    {
                        string propertyName = property.Metadata.Name;

                        if (property.Metadata.IsPrimaryKey())
                        {
                            if (entry.State == EntityState.Modified)
                            {
                                auditEntry.KeyValues[propertyName] = property.CurrentValue;
                            }
                            continue;
                        }

                        switch (entry.State)
                        {
                            case EntityState.Added:
                                auditEntry.AuditType = AuditType.Create;
                                auditEntry.EntiteNewValues[propertyName] = property.CurrentValue;
                                break;

                            case EntityState.Deleted:
                                auditEntry.AuditType = AuditType.Delete;
                                auditEntry.EntiteOldValues[propertyName] = property.OriginalValue;
                                break;

                            case EntityState.Modified:
                                if (property.IsModified)
                                {
                                    auditEntry.ChangedColumns.Add(propertyName);
                                    auditEntry.AuditType = AuditType.Update;
                                    auditEntry.EntiteOldValues[propertyName] = property.OriginalValue;
                                    auditEntry.EntiteNewValues[propertyName] = property.CurrentValue;
                                }
                                break;
                        }
                    }
                }
                if (auditEntries.Any())
                {

                    AuditLogToDB auditDB = new AuditLogToDB
                    {
                        AuditType= (int)AuditConstantEnum.Audit,
                        Audits = auditEntries
                    };

                    var sendToUri = new Uri($"{RabbitMqConsts.RabbitMqUri}{RabbitMqConsts.AuditLogQueue}");
                    var bus = Bus.Factory.CreateUsingRabbitMq(sbc =>
                    {
                        sbc.Host(new Uri(RabbitMqConsts.RabbitMqUri), h =>
                        {
                            h.Username(RabbitMqConsts.UserName);
                            h.Password(Convert.ToString(RabbitMqConsts.Password));
                        });
                    });
                 
                    var health= bus.CheckHealth();
                    await bus.StartAsync();
                  
                    var endPoint = await bus.GetSendEndpoint(sendToUri);
                    await endPoint.Send<AuditLogToDB>(auditDB);
                }
            }
            catch
            {

            }
        }
    }
}
