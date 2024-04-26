using System.ComponentModel.DataAnnotations;

namespace WebAppBlazor.Data.Model
{
    public class DepartmentModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Department { get; set; }
    }
}
