using Abp.Configuration.Startup;
using Abp.Localization.Dictionaries;
using Abp.Localization.Dictionaries.Xml;
using Abp.Reflection.Extensions;

namespace EasyLife.Localization
{
    public static class EasyLifeLocalizationConfigurer
    {
        public static void Configure(ILocalizationConfiguration localizationConfiguration)
        {
            localizationConfiguration.Sources.Add(
                new DictionaryBasedLocalizationSource(EasyLifeConsts.LocalizationSourceName,
                    new XmlEmbeddedFileLocalizationDictionaryProvider(
                        typeof(EasyLifeLocalizationConfigurer).GetAssembly(),
                        "EasyLife.Localization.SourceFiles"
                    )
                )
            );
        }
    }
}
