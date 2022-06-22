using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using VikopRu.Models;

namespace VikopRu.ViewModels
{
    public class FindingViewModel
    {
        public ApplicationUser Creator { get; set; }
        public List<FindingAction> Diggs { get; set; }
        public List<FindingAction> Buries { get; set; }

        [Required]
        public string Title { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public IFormFile Image { get; set; }

        public int FindingId { get; set; }
        public string ImageName { get; set; }

        public List<CommentViewModel> Comments { get; set; }
    }
}
