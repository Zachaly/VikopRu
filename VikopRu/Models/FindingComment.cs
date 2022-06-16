using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikopRu.Models
{
    public class FindingComment
    {
        public int Id { get; set; }
        public int CommentId { get; set; }
        public int FindingId { get; set; }
    }
}
