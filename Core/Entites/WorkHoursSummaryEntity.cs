using Entities.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entites
{
    [Table("WorkHoursSummary")]
    public class WorkHoursSummaryEntity : BaseEntity
    {
        public string EmployeeId { get; set; }
        [ForeignKey(nameof(EmployeeId))]
        public EmployeeEntity Employee { get; set; }
        public DateOnly SummaryDate { get; set; }
        public short? TotalWorkedHours { get; set; }
        public short? OvertimeHours => (TotalWorkedHours != null && TotalWorkedHours > 8) ? (short?)(TotalWorkedHours - 8) : 0;
    }
}


