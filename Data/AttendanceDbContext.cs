using Core.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class AttendanceDbContext : BaseDbContext<AttendanceDbContext>
    {
        public AttendanceDbContext(DbContextOptions<AttendanceDbContext> options, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(options, httpContextAccessor, configuration)
        {
        }

        public DbSet<AttendanceEntity> AttendanceEntities { get; set; }
    }
}
