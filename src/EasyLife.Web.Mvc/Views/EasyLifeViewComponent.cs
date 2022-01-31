using Abp.AspNetCore.Mvc.ViewComponents;

namespace EasyLife.Web.Views
{
    public abstract class EasyLifeViewComponent : AbpViewComponent
    {
        protected EasyLifeViewComponent()
        {
            LocalizationSourceName = EasyLifeConsts.LocalizationSourceName;
        }
    }
}
