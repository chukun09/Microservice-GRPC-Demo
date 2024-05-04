using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Employee
{
    public class DeleteEmployeeEvent : IntegrationBaseEvent
    {
        public string EmployeeId { get; set; }
    }
}
