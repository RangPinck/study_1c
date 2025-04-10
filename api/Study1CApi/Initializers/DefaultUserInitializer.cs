using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Design;
using Study1CApi.Models;

namespace Study1CApi.Initializers
{
    public class DefaultUserInitializer
    {
        private static readonly new List<Role> _roles = new List<Role>()
        {
            new Role
            {
                Id =  Guid.Parse("f47ac10b-58cc-4372-a567-0e02b2c3d479"),
                Name = "Ученик",
                NormalizedName = "УЧЕНИК",
                ConcurrencyStamp = "f47ac10b-58cc-4372-a567-0e02b2c3d479",
                IsNoManipulate = true
            },
            new Role
            {
                Id =  Guid.Parse("c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e"),
                Name = "Куратор",
                NormalizedName = "КУРАТОР",
                ConcurrencyStamp = "c9eb182b-1c3e-4c3b-8c3e-1c3e4c3b8c3e",
                IsNoManipulate= true
            },
            new Role
            {
                Id = Guid.Parse("f45d2396-3e72-4ec7-b892-7bd454248158"),
                Name = "Администратор",
                NormalizedName = "АДМНИСТРАТОР",
                ConcurrencyStamp = "f45d2396-3e72-4ec7-b892-7bd454248158",
                IsNoManipulate = true
            }
        };

        public static async Task UserInitializeAsync(UserManager<AuthUser> userManager, RoleManager<Role> roleManager, Study1cDbContext context, string adminName, string adminLogin, string adminPassword)
        {
            try
            {
                var pendingMigrations = await context.Database.GetPendingMigrationsAsync();

                if (pendingMigrations.Any())
                {
                    await context.Database.MigrateAsync();
                }

                foreach (var item in _roles)
                {
                    if (!await roleManager.RoleExistsAsync(item.Name))
                    {
                        await roleManager.CreateAsync(item);
                    }
                }

                if (await userManager.FindByEmailAsync(adminLogin) == null)
                {
                    Guid adminGuid = Guid.NewGuid();
                    var admin = new AuthUser()
                    {
                        Id = adminGuid,
                        UserName = adminName,
                        Email = adminLogin,
                        NormalizedUserName = adminName.ToUpper(),
                        NormalizedEmail = adminLogin.ToUpper(),
                        EmailConfirmed = true,
                        PasswordHash = new PasswordHasher<AuthUser>().HashPassword(null, "admin1cdbapi"),
                        UserDataCreate = DateTime.UtcNow
                    };

                    var createUserResult = await userManager.CreateAsync(admin);

                    if (createUserResult.Succeeded)
                    {
                        var addUserRoleResult = await userManager.AddToRoleAsync(admin, "Администратор");

                        if (addUserRoleResult.Succeeded)
                        {
                            var adminUser = new User()
                            {
                                UserId = adminGuid,
                                UserName = adminName,
                                UserSurname = adminName,
                                IsFirst = false
                            };

                            await context.Users.AddAsync(adminUser);
                            await context.SaveChangesAsync();
                        }
                        else
                        {
                            throw new Exception("Role dosn't added to user!");
                        }
                    }
                    else
                    {
                        throw new Exception("User in Identity doesn't create!");
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
