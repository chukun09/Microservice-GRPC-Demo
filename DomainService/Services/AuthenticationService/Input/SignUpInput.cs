using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainService.Services.AuthenticationService.Input
{
    public class SignUpInput
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        // Add other properties as needed, such as:
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
