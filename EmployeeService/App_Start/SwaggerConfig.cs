using System.Web.Http;
using WebActivatorEx;
using EmployeeService;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace EmployeeService
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            GlobalConfiguration.Configuration
                .EnableSwagger(c =>
                    {
                        c.SingleApiVersion("v1", "EmployeeService");
                    })
                .EnableSwaggerUi(c =>
                    {
                    });
        }
    }
}
