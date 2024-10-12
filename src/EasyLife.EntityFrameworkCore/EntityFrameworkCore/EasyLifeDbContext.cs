using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using EasyLife.Authorization.Roles;
using EasyLife.Authorization.Users;
using EasyLife.MultiTenancy;
using EasyLife.Financial.Expenses;
using EasyLife.Financial.Earning;
using EasyLife.Personal.EncryptedImportantThings;
using EasyLife.Financial.Investment;

namespace EasyLife.EntityFrameworkCore
{
    public class EasyLifeDbContext : AbpZeroDbContext<Tenant, Role, User, EasyLifeDbContext>
    {
        /* Define a DbSet for each entity of the application */

        #region Financial
        public virtual DbSet<ExpenseCategory> ExpenseCategory { get; set; }
        public virtual DbSet<Expenses> Expenses { get; set; }
        public virtual DbSet<EarningCategory> EarningCategory { get; set; }
        public virtual DbSet<Earnings> Earnings { get; set; }
        public virtual DbSet<EL_Financial_Investment_Share_Master> EL_Financial_Investment_Share_Master { get; set; }
        public virtual DbSet<EL_Financial_Investment_Share_Orders> EL_Financial_Investment_Share_Orders { get; set; }
        public virtual DbSet<EL_Financial_Investment_SIP_Master> EL_Financial_Investment_SIP_Master { get; set; }
        public virtual DbSet<EL_Financial_Investment_SIP_Entry> EL_Financial_Investment_SIP_Entry { get; set; }
        #endregion Financial

        #region Personal
        public virtual DbSet<EncryptedImportantInformation> EncryptedImportantInformation { get; set; }
        #endregion Personal
        public EasyLifeDbContext(DbContextOptions<EasyLifeDbContext> options)
            : base(options)
        {
        }
    }
}
