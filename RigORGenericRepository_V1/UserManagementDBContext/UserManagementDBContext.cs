using Microsoft.EntityFrameworkCore;
using RigorDomain.Role;
using RigorDomain.RoleGroup;
using RigorDomain.User;
using RigorDomain.UserRoleMap;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RigORGenericRepository.UserManagementDBContext
{
    [ExcludeFromCodeCoverage]
    public class UserManagementDBContext : DbContext
    {
        public UserManagementDBContext(DbContextOptions<UserManagementDBContext> options) : base(options)
        {

        }

        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }

        public DbSet<Modules> Modules { get; set; }

        public DbSet<ModuleAction> ModuleAction { get; set; }

        public DbSet<AccessRights> AccessRights { get; set; }
        public DbSet<RoleGroup> RoleGroup { get; set; }
        public DbSet<RoleGroupMapping> RoleGroupMapping { get; set; }
        public DbSet<UserRoleMapping> UserRoleMapping { get; set; }

        public DbSet<RefreshToken> RefreshToken { get; set; }

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
