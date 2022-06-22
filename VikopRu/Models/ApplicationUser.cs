using Microsoft.AspNetCore.Identity;

namespace VikopRu.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string ProfilePicture { get; set; } = "default.png";
    }
}
