using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikopRu.Models
{
    public class MikroblogPostReaction
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string UserId { get; set; }
    }
}
