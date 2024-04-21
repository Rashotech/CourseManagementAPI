using CourseManagement.Models;
using Microsoft.AspNetCore.Identity;

namespace CourseManagement.Helpers
{
    public class Seeder
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            if(_roleManager != null)
            {
                var roles = new[] { Roles.Administrator, Roles.RegularUser };
 
                foreach (var role in roles)
                {
                    if (!await _roleManager.RoleExistsAsync(role))
                    {
                        await _roleManager.CreateAsync(new IdentityRole(role));
                    }
                }
            }
        }
    }
}