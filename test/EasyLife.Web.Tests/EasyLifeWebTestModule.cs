using Abp.AspNetCore;
using Abp.AspNetCore.TestBase;
using Abp.Modules;
using Abp.Reflection.Extensions;
using EasyLife.EntityFrameworkCore;
using EasyLife.Web.Startup;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace EasyLife.Web.Tests
{
    [DependsOn(
        typeof(EasyLifeWebMvcModule),
        typeof(AbpAspNetCoreTestBaseModule)
    )]
    public class EasyLifeWebTestModule : AbpModule
    {
        public EasyLifeWebTestModule(EasyLifeEntityFrameworkModule abpProjectNameEntityFrameworkModule)
        {
            abpProjectNameEntityFrameworkModule.SkipDbContextRegistration = true;
        } 
        
        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsTransactional = false; //EF Core InMemory DB does not support transactions.
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EasyLifeWebTestModule).GetAssembly());
        }
        
        public override void PostInitialize()
        {
            IocManager.Resolve<ApplicationPartManager>()
                .AddApplicationPartsIfNotAddedBefore(typeof(EasyLifeWebMvcModule).Assembly);
        }
    }
}