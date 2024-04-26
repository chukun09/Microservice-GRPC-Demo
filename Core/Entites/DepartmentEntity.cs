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
    [Table("Department")]
    public class DepartmentEntity : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string? Description { get; set; }
    }
}
