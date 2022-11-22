using CalendarBackend.Controllers;
using Capa_Entidad;
using Capa_Negocio;
using Capa_Validacion;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CalendarBackendTest.Controllers
{
    public class AuthControllerTestLogin
    {
        [Test]
        public async Task DebePoderHacerLoginOk()
        {

            UsuarioLogin request = new()
            {
                Email = "ricardo@gmail.com",
                Password = "password123"
            };

            Usuario loginOk = new() { Email  = "ricardo@gmail.com", Name = "Ricardo", uid = 1, Password = "password123" };

            var IVali = new Mock<IValidarUsuario>();
            IVali.Setup(v => v.ValidarLogin(request)).Returns(new Resp() { Ok = true });

            var Iuser = new Mock<INrUsuario>();
            Iuser.Setup(u => u.NR_Login("ricardo@gmail.com")).ReturnsAsync(loginOk);

            var IHash = new Mock<ICreateHash>();
            IHash.Setup(h => h.PasswordDecrypt(request.Password)).Returns("password123");

            var IToken = new Mock<ITokenCreate>();
            IToken.Setup(t => t.TokenCreate(It.IsAny<int>(), loginOk.Name)).Returns("token");

            var controller = new AuthController(Iuser.Object, IHash.Object, IToken.Object, IVali.Object);

            var login = await controller.Login(request);

            var resul = login.Result as ObjectResult;
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task NodebePoderHacerLoginEmailNoValido()
        {
            UsuarioLogin request = new()
            {
                Email = "",
                Password = "password123"
            };

            Usuario loginOk = new() { Email = "ricardo@gmail.com", Name = "Ricardo", uid = 1, Password = "password123" };

            var rs = new { ok = false, msg = "Ingrese un correo valido" };

            var IVali = new Mock<IValidarUsuario>();
            IVali.Setup(v => v.ValidarLogin(request)).Returns(new Resp() { Ok = true });

            var Iuser = new Mock<INrUsuario>();
            Iuser.Setup(u => u.NR_Login("ricardo@gmail.com")).ReturnsAsync(loginOk);

            var IHash = new Mock<ICreateHash>();
            IHash.Setup(h => h.PasswordDecrypt(request.Password)).Returns("password123");

            var IToken = new Mock<ITokenCreate>();
            IToken.Setup(t => t.TokenCreate(It.IsAny<int>(), loginOk.Name)).Returns("token");

            var controller = new AuthController(Iuser.Object, IHash.Object, IToken.Object, IVali.Object);

            var login = await controller.Login(request);

            var resul = login.Result as ObjectResult;
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(500));
        }

        [Test]
        public async Task NodebePoderHacerLoginPasswordNoValido()
        {

            UsuarioLogin request = new()
            {
                Email = "ricardo@gmail.com",
                Password = ""
            };

            Usuario loginOk = new() { Email = "ricardo@gmail.com", Name = "Ricardo", uid = 1, Password = "password123" };
          
            var rs = new { ok = false, msg = "Ingrese un contraseña valida" };
            var IVali = new Mock<IValidarUsuario>();
            IVali.Setup(v => v.ValidarLogin(request)).Returns(new Resp() { Ok = true });

            var Iuser = new Mock<INrUsuario>();
            Iuser.Setup(u => u.NR_Login("ricardo@gmail.com")).ReturnsAsync(loginOk);

            var IHash = new Mock<ICreateHash>();
            IHash.Setup(h => h.PasswordDecrypt(request.Password)).Returns("password123");

            var IToken = new Mock<ITokenCreate>();
            IToken.Setup(t => t.TokenCreate(It.IsAny<int>(), loginOk.Name)).Returns("");

            var controller = new AuthController(Iuser.Object, IHash.Object, IToken.Object, IVali.Object);

            var login = await controller.Login(request);

            var resul = login.Result as ObjectResult;

            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(500));
        }

        [Test]
        public async Task NodebePoderHacerLoginCorreoNoExistente()
        {

            UsuarioLogin request = new()
            {
                Email = "ricardo@gmail.com",
                Password = "password123"
            };

            Usuario loginOk = new();
           
            var IVali = new Mock<IValidarUsuario>();
            IVali.Setup(v => v.ValidarLogin(request)).Returns(new Resp() { Ok = true });

            var Iuser = new Mock<INrUsuario>();
            Iuser.Setup(u => u.NR_Login("ricardo@gmail.com")).ReturnsAsync(loginOk);

            var IHash = new Mock<ICreateHash>();
            IHash.Setup(h => h.PasswordDecrypt(request.Password)).Returns("password123");

            var IToken = new Mock<ITokenCreate>();
            IToken.Setup(t => t.TokenCreate(It.IsAny<int>(), loginOk.Name)).Returns("token");

            var controller = new AuthController(Iuser.Object, IHash.Object, IToken.Object, IVali.Object);

            var login = await controller.Login(request);

            var result = login.Result as ObjectResult;
            Assert.That(result, Is.Not.Null);
            Assert.That(result.StatusCode, Is.EqualTo(500));
        }

        [Test]
        public async Task NodebePoderHacerLoginPasswordIncorrecto()
        {
            UsuarioLogin request = new()
            {
                Email = "ricardo@gmail.com",
                Password = "password1236"
            };


            Usuario loginOk = new() { Email = "ricardo@gmail.com", Name = "Ricardo", uid = 1, Password = "password1236" };

            var IVali = new Mock<IValidarUsuario>();
            IVali.Setup(v => v.ValidarLogin(request)).Returns(new Resp() { Ok = true });

            var Iuser = new Mock<INrUsuario>();
            Iuser.Setup(u => u.NR_Login("ricardo@gmail.com")).ReturnsAsync(loginOk);

            var IHash = new Mock<ICreateHash>();
            IHash.Setup(h => h.PasswordDecrypt(request.Password)).Returns("password123");

            var IToken = new Mock<ITokenCreate>();
            IToken.Setup(t => t.TokenCreate(It.IsAny<int>(), loginOk.Name)).Returns("token");

            var controller = new AuthController(Iuser.Object, IHash.Object, IToken.Object, IVali.Object);

            var login = await controller.Login(request);

            var resul = login.Result as ObjectResult;
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(500));
        }
    }
}
