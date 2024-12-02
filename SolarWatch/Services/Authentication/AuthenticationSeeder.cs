using Microsoft.AspNetCore.Identity;

namespace SolarWatch.Services.Authentication
{
    public class AuthenticationSeeder
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<IdentityUser> _userManager;
        public AuthenticationSeeder(RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void AddRoles()
        {
            var tAdmin = CreateAdminRole(_roleManager);
            tAdmin.Wait();

            var tUser = CreateUserRole(_roleManager);
            tUser.Wait();
        }

        private async Task CreateAdminRole(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("Admin")); //The role string should better be stored as a constant or a value in appsettings
        }

        async Task CreateUserRole(RoleManager<IdentityRole> roleManager)
        {
            await roleManager.CreateAsync(new IdentityRole("User")); //The role string should better be stored as a constant or a value in appsettings
        }

        public void AddAdmin()
        {
            var tAdmin = CreateAdminIfNotExists();
            tAdmin.Wait();
        }

        private async Task CreateAdminIfNotExists()
        {
            var adminInDb = await _userManager.FindByEmailAsync("admin@admin.com");
            if (adminInDb == null)
            {
                var admin = new IdentityUser { UserName = "admin", Email = "admin@admin.com" };
                var adminCreated = await _userManager.CreateAsync(admin, "admin123");

                if (adminCreated.Succeeded)
                {
                    await _userManager.AddToRoleAsync(admin, "Admin");
                }
            }
        }
    }
}
