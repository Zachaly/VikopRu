using System.ComponentModel.DataAnnotations;

namespace VikopRu.ViewModels
{
    public class LoginViewModel
    {
        [Required, MinLength(5)]
        public string UserName { get; set; }
        [Required, DataType(DataType.Password), MinLength(8)]
        public string Password { get; set; }
    }
}
