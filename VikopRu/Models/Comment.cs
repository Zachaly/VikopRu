using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikopRu.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string PosterId { get; set; }
        public string Image { get; set; } = null;
        public string Content { get; set; } = "";
    }
}
