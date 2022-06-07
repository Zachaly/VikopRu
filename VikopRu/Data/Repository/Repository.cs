using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopRu.Models;

namespace VikopRu.Data.Repository
{
    public class Repository : IRepository
    {
        private AppDbContext _dbContext;

        public Repository(AppDbContext dbContext) => _dbContext = dbContext;
        
        public Comment GetComment(int id) => _dbContext.Comments.First(comment => comment.Id == id);

        public List<FindingComment> GetFindingComments(Finding finding)
            => _dbContext.FindingsComments.Where(comment => comment.FindingId == finding.Id).ToList();

        public List<Finding> GetFindings() => _dbContext.Findings.ToList();

        public List<SubComment> GetSubComments(Comment comment)
            => _dbContext.SubComments.Where(subcomment => subcomment.MainCommentId == comment.Id).ToList();

        public ApplicationUser GetUser(string id) => _dbContext.Users.First(user => user.Id == id);
    }
}
