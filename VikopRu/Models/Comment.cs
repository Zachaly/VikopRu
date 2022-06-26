using System;

namespace VikopRu.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string PosterId { get; set; }
        public string Image { get; set; } = null;
        public string Content { get; set; } = "";
        public DateTime Created { get; set; } = DateTime.Now;
    }
}
