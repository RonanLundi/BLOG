using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.Data.Mappings
{
    public class CategoryMap : IEntityTypeConfiguration<Category>
    {
        public void Configure(EntityTypeBuilder<Category> builder)
        {
            // tabela
            builder.ToTable("Category");

            //Identity
            builder.HasKey(c => c.Id);

            // Propriedades
            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd()
                .UseMySqlIdentityColumn();

            builder.Property(c => c.Name)
                .IsRequired()
                .HasColumnName("Name")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            builder.Property(c => c.Slug)
                .IsRequired()
                .HasColumnName("Slug")
                .HasColumnType("VARCHAR")
                .HasMaxLength(80);

            //Índices
            builder.HasIndex(c => c.Slug, "IX_Category_Slug")
                .IsUnique();
        }
    }
}
