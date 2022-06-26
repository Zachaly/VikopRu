using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using VikopRu.Models;

namespace VikopRu.ViewModels
{
    public class CommentViewModel
    {
        public int CommentId { get; set; }
        public ApplicationUser Creator { get; set; }
        public string Content { get; set; }
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        public List<SubCommentViewModel> SubComments { get; set; } = new List<SubCommentViewModel>();
        public int FindingId { get; set; }
        public List<CommentReaction> PositiveReactions { get; set; }
        public List<CommentReaction> NegativeReactions { get; set; }
    }
}
