using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
