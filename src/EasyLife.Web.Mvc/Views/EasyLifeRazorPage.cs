using Abp.AspNetCore.Mvc.Views;
using Abp.Runtime.Session;
using Microsoft.AspNetCore.Mvc.Razor.Internal;

namespace EasyLife.Web.Views
{
    public abstract class EasyLifeRazorPage<TModel> : AbpRazorPage<TModel>
    {
        [RazorInject]
        public IAbpSession AbpSession { get; set; }

        protected EasyLifeRazorPage()
        {
            LocalizationSourceName = EasyLifeConsts.LocalizationSourceName;
        }
    }
}
