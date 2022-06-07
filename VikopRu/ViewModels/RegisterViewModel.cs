using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VikopRu.ViewModels
{
    public class RegisterViewModel : LoginViewModel
    {
        [Required, EmailAddress]
        public string Email { get; set; }
    }
}
