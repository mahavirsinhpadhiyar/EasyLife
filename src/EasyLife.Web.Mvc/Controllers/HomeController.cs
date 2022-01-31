using Microsoft.AspNetCore.Mvc;
using Abp.AspNetCore.Mvc.Authorization;
using EasyLife.Controllers;

namespace EasyLife.Web.Controllers
{
    [AbpMvcAuthorize]
    public class HomeController : EasyLifeControllerBase
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
