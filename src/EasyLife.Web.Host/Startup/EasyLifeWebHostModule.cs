using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using EasyLife.Configuration;

namespace EasyLife.Web.Host.Startup
{
    [DependsOn(
       typeof(EasyLifeWebCoreModule))]
    public class EasyLifeWebHostModule: AbpModule
    {
        private readonly IWebHostEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public EasyLifeWebHostModule(IWebHostEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EasyLifeWebHostModule).GetAssembly());
        }
    }
}
