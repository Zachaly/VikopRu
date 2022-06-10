using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikopRu.Models
{
    [Keyless]
    public class FindingAction
    {
        public string UserId { get; set; }
        public int FindingId { get; set; }
    }
}
