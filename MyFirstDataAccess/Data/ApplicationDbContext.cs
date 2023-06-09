using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyFirstModel;

namespace MyFirstDataAccess
{
    public class ApplicationDbContext:IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> k):base(k)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>(entity => entity.Property(m => m.Id).HasMaxLength(450));

            builder.Entity<IdentityUserLogin<string>>(entity =>
            {
                entity.Property(m => m.LoginProvider).HasMaxLength(127);
                entity.Property(m => m.ProviderKey).HasMaxLength(127);
            });

            builder.Entity<IdentityUserRole<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(127);
                entity.Property(m => m.RoleId).HasMaxLength(127);
            });

            builder.Entity<IdentityUserToken<string>>(entity =>
            {
                entity.Property(m => m.UserId).HasMaxLength(127);
                entity.Property(m => m.LoginProvider).HasMaxLength(127);
                entity.Property(m => m.Name).HasMaxLength(127);
            });
        }
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<ClassesInSchool> ClassesInSchools { get; set; }
        public DbSet<SessionYear> SessionYears { get; set; }
        public DbSet<SubClasses> SubClasses { get; set; }
        public DbSet<Subjects> Subjects { get; set; }
        public DbSet<TermregistrationTable> Termregistrations { get; set; }
        public DbSet<ResultTable> ResultTables { get; set; }
        public DbSet<RemarkPosition> RemarkPositions { get; set; }
        public DbSet<ClassGeneralTable> ClassGeneralTables { get; set; }
    }
}
