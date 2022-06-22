using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace VikopRu.ViewModels
{
    public class RegisterViewModel : LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        public IFormFile Image { get; set; }
    }
}
