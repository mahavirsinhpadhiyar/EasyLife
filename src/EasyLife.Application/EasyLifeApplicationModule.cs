using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using EasyLife.Authorization;

namespace EasyLife
{
    [DependsOn(
        typeof(EasyLifeCoreModule), 
        typeof(AbpAutoMapperModule))]
    public class EasyLifeApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            Configuration.Authorization.Providers.Add<EasyLifeAuthorizationProvider>();
        }

        public override void Initialize()
        {
            var thisAssembly = typeof(EasyLifeApplicationModule).GetAssembly();

            IocManager.RegisterAssemblyByConvention(thisAssembly);

            Configuration.Modules.AbpAutoMapper().Configurators.Add(
                // Scan the assembly for classes which inherit from AutoMapper.Profile
                cfg => cfg.AddMaps(thisAssembly)
            );
        }
    }
}
