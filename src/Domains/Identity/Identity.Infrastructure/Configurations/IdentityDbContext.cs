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

            // Configurar a tabela e colunas em minúsculas
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users"); // Nome da tabela em minúsculas

                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .HasColumnName("id"); // Nome da coluna em minúsculas

                entity.Property(e => e.Email)
                    .HasColumnName("email")
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.PasswordHash)
                    .HasColumnName("passwordhash")
                    .IsRequired();

                entity.Property(e => e.Role)
                    .HasColumnName("role")
                    .IsRequired()
                    .HasMaxLength(255);

                // Adicione outras propriedades conforme necessário
                // entity.Property(e => e.CreatedAt)
                //     .HasColumnName("created_at");
            });
        }
    }
}
