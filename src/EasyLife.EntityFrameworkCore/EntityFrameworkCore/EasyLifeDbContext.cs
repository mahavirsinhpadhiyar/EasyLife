using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using EasyLife.Authorization.Roles;
using EasyLife.Authorization.Users;
using EasyLife.MultiTenancy;
using EasyLife.Financial.Expenses;

namespace EasyLife.EntityFrameworkCore
{
    public class EasyLifeDbContext : AbpZeroDbContext<Tenant, Role, User, EasyLifeDbContext>
    {
        /* Define a DbSet for each entity of the application */

        public virtual DbSet<ExpenseCategory> ExpenseCategory { get; set; }
        public virtual DbSet<Expenses> Expenses { get; set; }
        public EasyLifeDbContext(DbContextOptions<EasyLifeDbContext> options)
            : base(options)
        {
        }
    }
}
