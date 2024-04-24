using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Users
{
    public class UserUpdatedEvent : IntegrationBaseEvent
    {
        public required string UserId { get; set; } // Linked to the AspNet Identity User Id
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        //public DateTime? DateOfBirth { get; set; }
        //public string? Address { get; set; }
    }
}
