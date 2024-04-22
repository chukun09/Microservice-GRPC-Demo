using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Base
{
    public abstract class BaseEntity<T>
    {
        public T Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string? CreatorUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public string? LastModifierUserId { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletionTime { get; set; }
        public string? DeleterUserId { get; set; }
    }
    public class BaseEntity : BaseEntity<string>
    {
        public BaseEntity()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
