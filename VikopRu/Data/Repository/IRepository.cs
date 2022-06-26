using System.Collections.Generic;
using System.Threading.Tasks;
using VikopRu.Models;

namespace VikopRu.Data.Repository
{
    public interface IRepository
    {
        ApplicationUser GetUser(string id);
        List<Finding> GetFindings();

        /// <summary>
        /// Gets commens assigned to given finding
        /// </summary>
        List<FindingComment> GetFindingComments(Finding finding);
        List<SubComment> GetSubComments(Comment comment);
        Comment GetComment(int id);

        /// <summary>
        /// Gets all users
        /// </summary>
        List<ApplicationUser> GetUsers();
        List<FindingAction> GetDiggs(int findingId);
        List<FindingAction> GetBuries(int findingId);

        /// <summary>
        /// Gets all diggs and buries
        /// </summary>
        List<FindingAction> GetAllActions();
        void AddAction(FindingAction action);
        void RemoveAction(int id);

        void AddFinding(Finding finding);
        void AddFindingComment(FindingComment comment);
        void AddComment(Comment comment);
        void AddSubComment(SubComment subComment);

        Task<bool> SaveChanges();

        List<CommentReaction> GetPosiviteCommentReactions(int commentId);
        List<CommentReaction> GetNegativeCommentReactions(int commentId);
        List<CommentReaction> GetAllCommentReactions();

        void AddCommentReaction(CommentReaction reaction);
        void RemoveCommentReaction(int id);
    }
}
