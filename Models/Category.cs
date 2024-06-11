using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.Mime;

namespace Blog.Models
{
    public class Category
    {
        public int Id { get; set; }

        public required string Name { get; set; }

        public required string Slug { get; set; }

        public List<Post> Posts { get; set; }
    }
}
