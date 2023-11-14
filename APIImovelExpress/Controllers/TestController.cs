using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace APIImovelExpress.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public async void MockCurrentUser(string userId)
        {
            var context = new Mock<HttpContext>();
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, userId)
            }, "TestAuthentication"));

             context.Setup(c => c.User).Returns(user);

            var controllerContext = new ControllerContext
            {
                HttpContext = context.Object,
                RouteData = new RouteData(),
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor(),
            };

             ControllerContext =  controllerContext;
        }
    }
}

