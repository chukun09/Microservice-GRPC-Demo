using Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites
{
    [Table("Employees")]
    public class EmployeeEntity : BaseEntity
    {
        public required string UserId { get; set; }
        [Required]
        public required string FirstName { get; set; }
        [Required]
        public required string LastName { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
        public string? Address { get; set; }
        public string? Position { get; set; }
        public string? DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        public DepartmentEntity? Department { get; set; }

    }
}
