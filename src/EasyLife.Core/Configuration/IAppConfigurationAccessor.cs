using Microsoft.Extensions.Configuration;

namespace EasyLife.Configuration
{
    public interface IAppConfigurationAccessor
    {
        IConfigurationRoot Configuration { get; }
    }
}
