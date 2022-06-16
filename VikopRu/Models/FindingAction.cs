﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikopRu.Models
{
    public class FindingAction
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int FindingId { get; set; }
    }
}
