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

        public List<ApplicationUser> GetUsers() => _dbContext.Users.ToList();

        public List<FindingAction> GetBuries(int findingId) 
            => _dbContext.Buries.Where(bury => bury.FindingId == findingId).ToList();
        public List<FindingAction> GetDiggs(int findingId)
            => _dbContext.Diggs.Where(dig => dig.FindingId == findingId).ToList();

        public void AddFinding(Finding finding) => _dbContext.Findings.Add(finding);

        public async Task<bool> SaveChanges()
        {
            if (await _dbContext.SaveChangesAsync() > 0)
                return true;

            return false;
        }
    }
}
