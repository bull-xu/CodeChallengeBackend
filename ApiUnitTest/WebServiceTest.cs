using System.Linq;
using System.Threading.Tasks;
using api.Services;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace ApiUnitTest
{
    public class WebServiceTest: BaseTests
    {
        private WebService _service;
        
        [SetUp]
        public void Setup()
        {
            _service = Container.GetService<WebService>();
        }

        [Test]
        public async Task TestSearchAsync()
        {
            // Arrange
            var keyword = "Gin";
            
            // Act
            var list = await _service.SearchAsync(keyword);
            
            // Assert
            Assert.NotNull(list);
            Assert.IsNotEmpty(list);
            Assert.NotNull(list.FirstOrDefault(x => x.Id == 15300));
            
        }


        [Test]
        public async Task TestDetailAsync()
        {
            // Arrange
            var id = 11007;
            
            // Act
            var drink = await _service.GetDetailsAsync(id);
            
            // Assert
            Assert.NotNull(drink);
            Assert.AreEqual(id, drink.Id);
            Assert.AreEqual("Margarita", drink.Name);
            Assert.IsNotEmpty(drink.Ingredients);

        }
        
        [Test]
        public async Task TestRandomAsync()
        {
            // Arrange
            
            // Act
            var drink = await _service.RandomAsync();
            
            // Assert
            Assert.NotNull(drink);

        }
        
    }
}