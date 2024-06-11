using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public required string Name{ get; set; }
        public required string Slug { get; set; }
    
        public required List<Post> Posts { get; set; }
    }
}
