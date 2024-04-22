using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants
{
    public static class Constants
    {
        public const string MigrationName = "Base.EntityFramework";

        public static class ClaimTypes
        {
            public const string UserId = "UserId";
            public const string Username = "Username";
            public const string Roles = "Roles";
        }

        public static class Role
        {
            public const string ADMIN = "Admin";
            public const string EMPLOYEE = "Employee";
        }
    }

}
