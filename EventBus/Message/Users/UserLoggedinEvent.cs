using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventBus.Message.Users
{
    public class UserLoggedinEvent : IntegrationBaseEvent
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
