
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ZeroBlog.Core.Domain.IdentityEntities;

namespace ZeroBlog.Infrastructure.DBContext
{
    public class AppDBContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DbSet<Post> Posts { get; set; }
        public AppDBContext(DbContextOptions<AppDBContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Define roles
            var adminRoleId = Guid.NewGuid();
            var userRoleId = Guid.NewGuid();
            var adminId = Guid.NewGuid();
            var userId = Guid.NewGuid();

            var roles = new List<IdentityRole<Guid>>
        {
            new IdentityRole<Guid> { Id = adminRoleId, Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole<Guid> { Id = userRoleId, Name = "User", NormalizedName = "USER" }
        };

            // Define users
            var hasher = new PasswordHasher<ApplicationUser>();

            var adminUser = new ApplicationUser
            {
                Id = adminId,
                PersonName = "Admin",
                UserName = "admin@example.com",
                NormalizedUserName = "ADMIN@EXAMPLE.COM",
                Email = "admin@example.com",
                NormalizedEmail = "ADMIN@EXAMPLE.COM",
                EmailConfirmed = true
            };
            adminUser.PasswordHash = hasher.HashPassword(adminUser, "Admin34");

            var normalUser = new ApplicationUser
            {
                Id = userId,
                UserName = "user@example.com",
                PersonName = "User",
                NormalizedUserName = "USER@EXAMPLE.COM",
                Email = "user@example.com",
                NormalizedEmail = "USER@EXAMPLE.COM",
                EmailConfirmed = true
            };
            normalUser.PasswordHash = hasher.HashPassword(normalUser, "User34");

            // Define role assignments
            var userRoles = new List<IdentityUserRole<Guid>>
        {
            new IdentityUserRole<Guid> { UserId = adminId, RoleId = adminRoleId },
            new IdentityUserRole<Guid> { UserId = userId, RoleId = userRoleId }
        };

            // Apply seeding
            builder.Entity<IdentityRole<Guid>>().HasData(roles);
            builder.Entity<ApplicationUser>().HasData(adminUser, normalUser);
            builder.Entity<IdentityUserRole<Guid>>().HasData(userRoles);
        }
    }
}
