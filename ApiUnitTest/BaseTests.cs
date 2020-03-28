using System;
using System.IO;
using api.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ApiUnitTest
{
    public class BaseTests
    {
        protected IConfigurationRoot Configuration;
        protected IServiceProvider Container;

        public BaseTests()
        {
            
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("config.json", optional: true, reloadOnChange: true)
                .AddEnvironmentVariables();
            
            Configuration = builder.Build();
            MockContainer();
            
        }
        
        private void MockContainer()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            Container = services.BuildServiceProvider();
        }

        private void ConfigureServices(IServiceCollection services)
        {

            services.AddHttpClient();
            services.AddSingleton<WebService>();
        }



        protected static void MockContext(Controller controller, IHeaderDictionary headers)
        {

            controller.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext()
            };

            if (headers == null)
            {
                return;
            }

            
            foreach (var (key, value) in headers)
            {
                controller.ControllerContext.HttpContext.Request.Headers[key] = value;
            }
        }

    }

}