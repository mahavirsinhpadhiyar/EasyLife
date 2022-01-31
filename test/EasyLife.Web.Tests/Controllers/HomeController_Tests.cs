using System.Threading.Tasks;
using EasyLife.Models.TokenAuth;
using EasyLife.Web.Controllers;
using Shouldly;
using Xunit;

namespace EasyLife.Web.Tests.Controllers
{
    public class HomeController_Tests: EasyLifeWebTestBase
    {
        [Fact]
        public async Task Index_Test()
        {
            await AuthenticateAsync(null, new AuthenticateModel
            {
                UserNameOrEmailAddress = "admin",
                Password = "123qwe"
            });

            //Act
            var response = await GetResponseAsStringAsync(
                GetUrl<HomeController>(nameof(HomeController.Index))
            );

            //Assert
            response.ShouldNotBeNullOrEmpty();
        }
    }
}