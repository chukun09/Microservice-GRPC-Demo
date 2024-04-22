using AutoMapper;
using Core.Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Attendances
{
    [AutoMap(typeof(AttendanceEntity), ReverseMap = true)]
    public class UserAttendancedEvent : IntegrationBaseEvent
    {
        public string? EmployeeId { get; set; }
        public DateOnly Date { get; set; }
        public DateTime CheckinTime { get; set; }
        public DateTime CheckoutTime { get; set; }
    }
}
