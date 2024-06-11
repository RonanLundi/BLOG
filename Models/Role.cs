using System;

namespace Blog.Models
{
    public class Role
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Slug { get; set; }

        public required IList<User> Users { get; set; }
    }
}
