using Core.Entites;
using Entities.Base;
using Data.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;

namespace Data
{
    public class AuthenticationDbContext : IdentityDbContext<UserEntity>
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        //public DbSet<PersonEntity> People { get; set; }
        public DbSet<RefreshTokenEntity> RefreshTokenEntities { get; set; }
        public AuthenticationDbContext(DbContextOptions<AuthenticationDbContext> options,
            IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //var entityTypes = modelBuilder.Model.GetEntityTypes();

            //modelBuilder.Entity<PersonEntity>().HasQueryFilter(b => !b.IsDeleted);
            FilterSoftDelete.ApplySoftDeleteQueryFilter(modelBuilder);
            base.OnModelCreating(modelBuilder);
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
