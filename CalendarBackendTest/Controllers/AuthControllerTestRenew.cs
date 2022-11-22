using CalendarBackend.Controllers;
using Capa_Entidad;
using Capa_Negocio;
using Capa_Validacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace CalendarBackendTest.Controllers
{
    public class AuthControllerTestRenew
    {
        [Test]
        public  void DebePoderValidarTokenOk()
        {
            var ITok = new Mock<ITokenCreate>();
            var Iuser = new Mock<INrUsuario>();
            var IHash = new Mock<ICreateHash>();
            var IVali = new Mock<IValidarUsuario>();

            ClaimsIdent user = new()
            {
                Id_Usuario = 1,
                Names = "Ricardo",
                Token = "tokenrecibido"
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Surname, "Ricardo"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "Bear Token"));


            ITok.Setup(a => a.TokenCreate(1, "Ricardo")).Returns("tokenrecibido");
            ITok.Setup(g => g.GetUser(principal.Claims)).Returns(user);
            ITok.Setup(t => t.ValidarToken("tokenrecibido", principal.Claims)).Returns(new ClaimsIdent() { Id_Usuario = 1, Names = "Ricardo", Token = "tokenrecibido" });

            var controller = new AuthController(Iuser.Object, IHash.Object, ITok.Object, IVali.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = principal
                    }
                }
            };

            var renew =  controller.Renew("tokenrecibido");
            var resul = renew.Result as ObjectResult;
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public void NoDebePoderValidarTokenOk()
        {
            var ITok = new Mock<ITokenCreate>();
            var Iuser = new Mock<INrUsuario>();
            var IHash = new Mock<ICreateHash>();
            var IVali = new Mock<IValidarUsuario>();

            ClaimsIdent user = new()
            {
                Id_Usuario = 1,
                Names = "Ricardo",
                Token = "tokenrecibido"
            };

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Surname, "Ricardo"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "Bear Token"));


            ITok.Setup(a => a.TokenCreate(1, "Ricardo")).Returns("tokenrecibido");
            ITok.Setup(g => g.GetUser(principal.Claims)).Returns(user);
            ITok.Setup(t => t.ValidarToken("tokenrecibido", principal.Claims)).Returns(new ClaimsIdent());

            var controller = new AuthController(Iuser.Object, IHash.Object, ITok.Object, IVali.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = principal
                    }
                }
            };

            var renew =  controller.Renew("tokenrecibido");
            var resul = renew.Result as ObjectResult;
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(500));
        }
    }
}
