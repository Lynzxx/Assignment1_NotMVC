using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Assignment1_NotMVC.Models
{
    public class AuthDbContext : IdentityDbContext<ApplicationUser>
    {
        private readonly IConfiguration _configuration;
        //public AuthDbContext(DbContextOptions<AuthDbContext> options):base(options){ }
        public AuthDbContext(IConfiguration configuration, DbContextOptions<AuthDbContext> options):base(options)
        {
            _configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.Name)
                .HasMaxLength(250);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.Gender)
                .HasMaxLength(1);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.CreditCardNo);
  

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.DeliveryAddr)
                .HasMaxLength(200);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.AboutMe)
                .HasMaxLength(300);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.Photo)
                .HasMaxLength(300);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.PastPassword1)
                .HasMaxLength(300);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.PastPassword2)
                .HasMaxLength(300);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.LastPasswordChangedDate);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.LastOTPGeneratedDate);

            modelBuilder.Entity<ApplicationUser>()
                .Property(e => e.OTP);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = _configuration.GetConnectionString("AuthConnectionString"); optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Audit> Audits { get; set; }
    }
}
