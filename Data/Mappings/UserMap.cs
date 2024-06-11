using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace blog.Data.Mappings
{
    internal class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            //tabela
            builder.ToTable("users");

            //Chave Primária
            builder.HasKey(u => u.Id);

            //Identity
            builder.Property(u => u.Id)
                .ValueGeneratedOnAdd()
                .UseMySqlIdentityColumn();
            //Propriedades
            builder.Property(u => u.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder.Property(u => u.Bio).IsRequired(false);

            builder.Property(u => u.Email)
                .IsRequired()
                .HasColumnName("Email")
                .HasColumnType("VARCHAR")
                .HasMaxLength(160);
            
            builder.Property(u => u.Image).IsRequired(false);

            builder.Property(u => u.PasswordHash)
                .IsRequired()
                .HasColumnName("PasswordHash")
                .HasColumnType("VARCHAR")
                .HasMaxLength(255);

            builder.Property(u => u.Slug)
                .IsRequired()
                .HasColumnName("Slug")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            //Índices
            builder.HasIndex(u => u.Slug, "IX_User_Slug").IsUnique();

            //Relacionamentos

            builder.HasMany(x => x.Roles)
                .WithMany(x => x.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "PostTag",
                    role => role.HasOne<Role>()
                    .WithMany()
                    .HasForeignKey("RoleId")
                    .HasConstraintName("FK_UserRole_RoleId")
                    .OnDelete(DeleteBehavior.NoAction),
                    user => user.HasOne<User>()
                    .WithMany()
                    .HasForeignKey("UserId")
                    .HasConstraintName("FK_UserRole_UserId")
                    .OnDelete(DeleteBehavior.NoAction)
                );


        }
    }
}
