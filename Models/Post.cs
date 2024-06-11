using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blog.Models
{
    public class Post
    {
        public int Id { get; set; }

        public required string Title { get; set; }
        public required string Summary { get; set; }
        public required string Body { get; set; }
        public required string Slug { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public required Category Category { get; set; }
        public required User Author { get; set; }
        public required List<Tag> Tags { get; set; }


    }
}
