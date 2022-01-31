using System.Threading.Tasks;
using Abp.Authorization;
using Abp.Runtime.Session;
using EasyLife.Configuration.Dto;

namespace EasyLife.Configuration
{
    [AbpAuthorize]
    public class ConfigurationAppService : EasyLifeAppServiceBase, IConfigurationAppService
    {
        public async Task ChangeUiTheme(ChangeUiThemeInput input)
        {
            await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), AppSettingNames.UiTheme, input.Theme);
        }
    }
}
