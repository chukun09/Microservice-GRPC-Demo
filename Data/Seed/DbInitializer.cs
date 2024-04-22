using Core.Constants;
using Core.Entites;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static Core.Constants.Constants;

namespace EntityFrameworkCore.Seed
{
    public interface IDbInitializer
    {
        Task SeedUsers();
    }
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<UserEntity> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public DbInitializer(
            UserManager<UserEntity> userManager,
            RoleManager<IdentityRole> roleManager
            )
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task SeedUsers()
        {
            await CreateRole(Constants.Role.ADMIN);
            await CreateRole(Constants.Role.EMPLOYEE);
            var isAdmin = await _userManager.FindByNameAsync(Constants.Role.ADMIN);
            if (isAdmin == null)
            {
                // User
                var userId = Guid.NewGuid().ToString();
                var user = new UserEntity()
                {
                    Id = userId,
                    UserName = "Admin",
                    Email = "admin@gmail.com",
                    LockoutEnabled = false,
                };
                var rs = await _userManager.CreateAsync(user, "Admin123@@");
                if (rs.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, Constants.Role.ADMIN);

                }
            }
        }
        /// <summary>
        /// Init role for database
        /// </summary>
        /// <param name="roleName"></param>
        /// <returns></returns>
        public async Task CreateRole(string roleName)
        {
            Random randomNumber = new();
            var role = await _roleManager.FindByNameAsync(roleName);

            if (role == null)
            {
                var roleId = Guid.NewGuid().ToString();
                var roleNew = new IdentityRole()
                {
                    Id = roleId,
                    Name = roleName,
                    ConcurrencyStamp = randomNumber.Next(1, 100).ToString(),
                    NormalizedName = roleName
                };
                await _roleManager.CreateAsync(roleNew);
            }
        }
    }
}
