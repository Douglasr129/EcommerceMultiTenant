using Identity.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Identity.Infrastructure.Configurations
{
    public class IdentityDbContext : DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .IsRequired()
                    .HasMaxLength(255);

                // DICA: Adicionar um índice único no email para performance e integridade
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Name)
                    .HasColumnName("name")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PasswordHash)
                    .HasColumnName("passwordhash")
                    .IsRequired();

                entity.Property(e => e.Role)
                    .HasColumnName("role")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Active)
                    .HasColumnName("active")
                    .IsRequired();
            });

            // Filtro Global: Ignora usuários inativos em todas as queries automaticamente
            modelBuilder.Entity<User>().HasQueryFilter(u => u.Active);
        }
    }
}

