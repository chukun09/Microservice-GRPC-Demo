using Data.Extensions;
using Entities.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class BaseDbContext<T> : DbContext where T :DbContext
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public BaseDbContext(DbContextOptions<T> options , IHttpContextAccessor httpContextAccessor, IConfiguration configuration) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var entityTypes = modelBuilder.Model.GetEntityTypes();

            //modelBuilder.Entity<PersonEntity>().HasQueryFilter(b => !b.IsDeleted);
            FilterSoftDelete.ApplySoftDeleteQueryFilter(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified
                        || e.State == EntityState.Deleted));
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
            var userId = "";
            var username = "";
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var claimTokens = handler.ReadJwtToken(token);
                userId = claimTokens.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value ?? "";
                username = claimTokens.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value ?? "";
            }
            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreationTime = DateTime.UtcNow.AddHours(7);
                    ((BaseEntity)entityEntry.Entity).CreatorUserId = userId;
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    ((BaseEntity)entityEntry.Entity).LastModificationTime = DateTime.UtcNow.AddHours(7);
                    ((BaseEntity)entityEntry.Entity).LastModifierUserId = userId;
                }

                if (entityEntry.State == EntityState.Deleted)
                {
                    entityEntry.State = EntityState.Modified;
                    ((BaseEntity)entityEntry.Entity).IsDeleted = true;
                    ((BaseEntity)entityEntry.Entity).DeletionTime = DateTime.UtcNow.AddHours(7);
                    ((BaseEntity)entityEntry.Entity).DeleterUserId = userId;
                }
            }
            return base.SaveChangesAsync(cancellationToken);
        }
        public override int SaveChanges()
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                        e.State == EntityState.Added
                        || e.State == EntityState.Modified
                        || e.State == EntityState.Deleted));
            var token = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").LastOrDefault();
            var userId = "";
            var username = "";
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var claimTokens = handler.ReadJwtToken(token);
                userId = claimTokens.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value ?? "";
                username = claimTokens.Claims.FirstOrDefault(x => x.Type == "UserName")?.Value ?? "";
            }
            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreationTime = DateTime.UtcNow.AddHours(7);
                    ((BaseEntity)entityEntry.Entity).CreatorUserId = userId;
                }

                if (entityEntry.State == EntityState.Modified)
                {
                    ((BaseEntity)entityEntry.Entity).LastModificationTime = DateTime.UtcNow.AddHours(7);
                    ((BaseEntity)entityEntry.Entity).LastModifierUserId = userId;
                }

                if (entityEntry.State == EntityState.Deleted)
                {
                    entityEntry.State = EntityState.Modified;
                    ((BaseEntity)entityEntry.Entity).IsDeleted = true;
                    ((BaseEntity)entityEntry.Entity).DeletionTime = DateTime.UtcNow.AddHours(7);
                    ((BaseEntity)entityEntry.Entity).DeleterUserId = userId;
                }
            }
            return base.SaveChanges();
        }
    }
}
