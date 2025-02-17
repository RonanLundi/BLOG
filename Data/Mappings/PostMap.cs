﻿using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace blog.Data.Mappings
{
    internal class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            //Tabela
            builder.ToTable("post");

            //Chave Primária
            builder.HasKey(x => x.Id);

            //Identity
            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .UseMySqlIdentityColumn();

            //Propriedades
            builder.Property(x => x.LastUpdateDate)
                .IsRequired()
                .HasColumnName("LastUpdateDate")
                .HasColumnName("SMALLDATETIME")
                .HasMaxLength(60)
                //.HasDefaultValueSql("GETDATE()");//mysqlserver
                .HasDefaultValue(DateTime.Now.ToUniversalTime());//utc

            //Índices
            builder.HasIndex(x => x.Slug, "IX_Post_Slug").IsUnique();

            //Relacionamentos

            builder.HasOne(x => x.Author)
                .WithMany(x => x.Posts)
                .HasConstraintName("FK_Post_Author")
                .OnDelete(DeleteBehavior.NoAction);
            builder.HasOne(x => x.Category)
                .WithMany(x => x.Posts).HasConstraintName("FK_Post_Category")
                .OnDelete(DeleteBehavior.NoAction);

            builder.HasMany(x => x.Tags)
                .WithMany(x=>x.Posts)
                .UsingEntity<Dictionary<string, object>>(
                    "PostTag",
                    post => post.HasOne<Tag>()
                    .WithMany()
                    .HasForeignKey("PostId")
                    .HasConstraintName("FK_PostTag_PostId")
                    .OnDelete(DeleteBehavior.NoAction),                    
                    tag => tag.HasOne<Post>()
                    .WithMany()
                    .HasForeignKey("TagId")
                    .HasConstraintName("FK_PostTag_TagId")
                    .OnDelete(DeleteBehavior.NoAction)
                );
        }
    }
}
