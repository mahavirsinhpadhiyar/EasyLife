using System.Threading.Tasks;
using EasyLife.Configuration.Dto;

namespace EasyLife.Configuration
{
    public interface IConfigurationAppService
    {
        Task ChangeUiTheme(ChangeUiThemeInput input);
    }
}
