using System.Collections.Generic;
using System.Linq;
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
            => _dbContext.FindingActions.Where(action => !action.IsDig).Where(bury => bury.FindingId == findingId).ToList();
        public List<FindingAction> GetDiggs(int findingId)
            => _dbContext.FindingActions.Where(action => action.IsDig).Where(dig => dig.FindingId == findingId).ToList();
        public List<FindingAction> GetAllActions() => _dbContext.FindingActions.ToList();

        public void AddAction(FindingAction action) => _dbContext.FindingActions.Add(action);
        public void RemoveAction(int id) => _dbContext.FindingActions.Remove(_dbContext.FindingActions.First(action => action.Id == id));
        public void AddFinding(Finding finding) => _dbContext.Findings.Add(finding);
        public void AddFindingComment(FindingComment comment) => _dbContext.FindingsComments.Add(comment);
        public void AddComment(Comment comment) => _dbContext.Comments.Add(comment);
        public void AddSubComment(SubComment subComment) => _dbContext.SubComments.Add(subComment);

        public async Task<bool> SaveChanges()
        {
            if (await _dbContext.SaveChangesAsync() > 0)
                return true;

            return false;
        }

        public List<CommentReaction> GetPosiviteCommentReactions(int commentId)
            => _dbContext.CommentReactions.Where(reaction => reaction.CommentId == commentId && reaction.Positive).ToList();

        public List<CommentReaction> GetNegativeCommentReactions(int commentId)
            => _dbContext.CommentReactions.Where(reaction => reaction.CommentId == commentId && !reaction.Positive).ToList();

        public List<CommentReaction> GetAllCommentReactions() => _dbContext.CommentReactions.ToList();
        public void AddCommentReaction(CommentReaction reaction) => _dbContext.CommentReactions.Add(reaction);
        public void RemoveCommentReaction(int id) 
            => _dbContext.CommentReactions.Remove(_dbContext.CommentReactions.First(reaction => reaction.Id == id));
    }
}
