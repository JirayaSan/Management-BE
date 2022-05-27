using Management_BE.Models.DatabaseModels;
using Microsoft.EntityFrameworkCore;

namespace Management_BE.Data.AuthenticationData
{
    public class ApplicationDataContext : DbContext
    {
        public ApplicationDataContext(DbContextOptions<ApplicationDataContext> options) : base(options) { }

        // Utente
        public DbSet<User> User { get; set; }
        // Ruolo
        public DbSet<Role> Role { get; set; }
        // Documenti
        public DbSet<Document> Document { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relazione one to one con le tabelle Role e User
            //modelBuilder.Entity<Role>()
            //            .HasOne(role => role.User)
            //            .WithOne(user => user.Role)
            //            .HasForeignKey<User>(fk => fk.RoleId);

            //modelBuilder.Entity<User>()
            //            .HasOne(user => user.Role)
            //            .WithOne(role => role.User)
            //            .HasForeignKey<Role>(fk => fk.UserId);

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<Document>().ToTable("Document");
        }
    }
}
