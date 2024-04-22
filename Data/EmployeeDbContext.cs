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
    public class EmployeeDbContext : BaseDbContext<EmployeeDbContext>
    {
        public EmployeeDbContext(DbContextOptions<EmployeeDbContext> options, IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(options, httpContextAccessor, configuration)
        {
        }

        public DbSet<EmployeeEntity> EmployeeEntities { get; set; }
        public DbSet<WorkHoursSummaryEntity> WorkHoursSummaryEntities { get; set; }
        public DbSet<DepartmentEntity> DepartmentEntities { get; set; }
    }
}
