﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopRu.Models;

namespace VikopRu.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Finding> Findings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FindingComment> FindingsComments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }
    }
}