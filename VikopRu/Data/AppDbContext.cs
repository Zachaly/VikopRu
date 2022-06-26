using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VikopRu.Models;

namespace VikopRu.Data
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Finding> Findings { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FindingComment> FindingsComments { get; set; }
        public DbSet<SubComment> SubComments { get; set; }

        public DbSet<FindingAction> FindingActions { get; set; }
        public DbSet<CommentReaction> CommentReactions { get; set; }
    }
}
