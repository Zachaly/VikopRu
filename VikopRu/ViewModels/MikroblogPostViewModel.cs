using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VikopRu.Models;

namespace VikopRu.ViewModels
{
    public class MikroblogPostViewModel
    {
        public int CommentId { get; set; }
        public ApplicationUser Creator { get; set; }
        public string Content { get; set; }
        public IFormFile Image { get; set; }
        public string ImageName { get; set; }
        public List<MikroblogCommentViewModel> SubComments { get; set; } = new List<MikroblogCommentViewModel>();
        public DateTime Created { get; set; }
        public List<MikroblogPostReaction> Pluses { get; set; }
    }
}
