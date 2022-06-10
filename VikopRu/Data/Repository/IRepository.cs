using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopRu.Models;

namespace VikopRu.Data.Repository
{
    public interface IRepository
    {
        ApplicationUser GetUser(string id);
        List<Finding> GetFindings();
        List<FindingComment> GetFindingComments(Finding finding);
        List<SubComment> GetSubComments(Comment comment);
        Comment GetComment(int id);
        List<ApplicationUser> GetUsers();
        List<FindingAction> GetDiggs(int findingId);
        List<FindingAction> GetBuries(int findingId);
    }
}
