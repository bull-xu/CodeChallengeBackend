using System.Threading.Tasks;
using api.Controllers;
using api.Models.Response;
using api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace ApiUnitTest
{
    public class BoozeControllerTests: BaseTests
    {

        [Fact]
        public async Task GetIngredientSearchAsyncTest()
        {
            // Arrange
            var webservice = Container.GetService<WebService>();
            var controller = new BoozeController(webservice);
            var keyword = "Gin";

            // Act
            var result = await controller.GetIngredientSearchAsync(keyword);
            
            var okObjectResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okObjectResult);
            
            var response = Assert.IsType<CocktailList>(okObjectResult.Value);
            Assert.NotNull(response);

        }
    }
}