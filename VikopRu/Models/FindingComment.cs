﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikopRu.Models
{
    [Keyless]
    public class FindingComment
    {
        public int CommentId { get; set; }
        public int FindingId { get; set; }
    }
}
