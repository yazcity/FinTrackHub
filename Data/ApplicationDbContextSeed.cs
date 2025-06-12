using FinTrackHub.Entities;
using FinTrackHub.Identity;
using Microsoft.AspNetCore.Identity;

namespace FinTrackHub.Data
{
    public class ApplicationDbContextSeed
    {
        public static async Task Initialize(IServiceProvider serviceProvider, UserManager<ApplicationUser> userManager, RoleManager<ApplicationRole> roleManager, ApplicationDbContext context)
        {
            // Seed Roles
            var roleNames = new[] { "Admin", "User" };
            foreach (var roleName in roleNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(roleName);
                if (!roleExist)
                {
                    var role = new ApplicationRole
                    {
                        Name = roleName,
                        NormalizedName = roleName.ToUpper(),
                        CreatedDate = DateTime.UtcNow,
                        UpdatedDate = DateTime.UtcNow
                    };
                    await roleManager.CreateAsync(role);
                }
            }

            // Ensure the admin user exists
            var adminUser = await userManager.FindByEmailAsync("admin@yazcity.com");

            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = "admin@yazcity.com",
                    Email = "admin@yazcity.com",
                    FullName = "Administrator",
                    CreatedDate = DateTime.UtcNow,
                    UpdatedDate = DateTime.UtcNow
                };

                var userCreationResult = await userManager.CreateAsync(adminUser, "123asd.Com");

                if (userCreationResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // Add categories and other seeding logic
            var createdById = adminUser.Id;

            if (!context.IncomeExpenseCategoryTypes.Any())
            {
                context.IncomeExpenseCategoryTypes.AddRange(
                    new IncomeExpenseCategoryType { TypeId = 1, TypeName = "Initial deposit", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, CreatedById = createdById, UpdatedById = createdById },
                    new IncomeExpenseCategoryType { TypeId = 2, TypeName = "Balance adjustment", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, CreatedById = createdById, UpdatedById = createdById },
                    new IncomeExpenseCategoryType { TypeId = 3, TypeName = "Income", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, CreatedById = createdById, UpdatedById = createdById },
                    new IncomeExpenseCategoryType { TypeId = 5, TypeName = "Expense", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, CreatedById = createdById, UpdatedById = createdById },
                    new IncomeExpenseCategoryType { TypeId = 6, TypeName = "Transfer", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, CreatedById = createdById, UpdatedById = createdById }
                );
            }

            if (!context.IncomeExpenseCategories.Any())
            {
                context.IncomeExpenseCategories.AddRange(
                    new IncomeExpenseCategory { TypeId = 1, CategoryName = "Initial deposit", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, CreatedById = createdById, UpdatedById = createdById, IsDeletable = false },
                    new IncomeExpenseCategory { TypeId = 2, CategoryName = "Balance adjustment", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, CreatedById = createdById, UpdatedById = createdById, IsDeletable = false },
                    new IncomeExpenseCategory { TypeId = 6, CategoryName = "Transfer", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, CreatedById = createdById, UpdatedById = createdById, IsDeletable = false }
                );
            }

            if (!context.AccountGroupTypes.Any())
            {
                context.AccountGroupTypes.AddRange(
                    new AccountGroupType { AccountGroupTypeName = "Payable", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, CreatedById = createdById, UpdatedById = createdById, IsDeletable = false },
                    new AccountGroupType { AccountGroupTypeName = "Holding", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, CreatedById = createdById, UpdatedById = createdById, IsDeletable = false },
                    new AccountGroupType { AccountGroupTypeName = "Credit Card", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, CreatedById = createdById, UpdatedById = createdById, IsDeletable = false },
                    new AccountGroupType { AccountGroupTypeName = "Investments", IsActive = true, CreatedDate = DateTime.UtcNow, UpdatedDate = DateTime.UtcNow, CreatedById = createdById, UpdatedById = createdById, IsDeletable = false }
                );
            }

            await context.SaveChangesAsync();
        }

    }
}
