using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Services.AuthenticationService.Input
{
    public class SignUpInput
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        // Add other properties as needed, such as:
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
    }
}
