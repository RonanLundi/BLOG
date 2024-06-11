using blog.Data.Mappings;
using Blog.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Blog.Data
{
    public class BlogDataContext : DbContext
    {
        public BlogDataContext(DbContextOptions<BlogDataContext> options) : base(options) 
        {
        
        }
        

        public required DbSet<Category> Categories { get; set; }
        public required DbSet<Post> Posts { get; set; }
        public required DbSet<User> Users { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder opcoes)
        {
            if (!opcoes.IsConfigured)
                opcoes.UseMySql("Server=localhost;Database=blog;User=root;Password=lundi;",
                    new MySqlServerVersion(new Version(5,0,27)));

            //opcoes.LogTo(Console.WriteLine);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CategoryMap());
            modelBuilder.ApplyConfiguration(new PostMap());
            modelBuilder.ApplyConfiguration(new UserMap());
        }
    }
}
