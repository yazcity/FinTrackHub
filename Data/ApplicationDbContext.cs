using FinTrackHub.Entities;
using FinTrackHub.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FinTrackHub.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        { }

        public DbSet<ApplicationRole> ApplicationRoles { get; set; }

        public DbSet<IncomeExpenseCategoryType> IncomeExpenseCategoryTypes { get; set; }
        public DbSet<IncomeExpenseCategory> IncomeExpenseCategories { get; set; }
        public DbSet<AccountGroupType> AccountGroupTypes { get; set; }
        public DbSet<AccountGroup> AccountGroups { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Budget> Budgets { get; set; }

    }
}
