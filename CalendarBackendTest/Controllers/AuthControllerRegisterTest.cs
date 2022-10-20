using CalendarBackend.Controllers;
using Capa_Entidad;
using Capa_Negocio;
using Capa_Validacion;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace CalendarBackendTest.Controllers
{

    public class AuthControllerRegisterTest
    {
        [Test]
        public async Task DebePoderRegistarUsuarioOk()
        {
            var regi = new UsuarioRegister()
            {
                Email = "prueba@gmail.com",
                Name = "Prueba",
                Password = "Prueba123"
            };

            var Iuser = new Mock<INR_Usuario>();
            Iuser.Setup(u => u.NR_Buscar_CorreoS("ricardo@gmail.com")).ReturnsAsync(null);
            Iuser.Setup(u => u.NR_Registrar_Usuario(regi)).ReturnsAsync(2);

            var IHash = new Mock<ICreateHash>();
            IHash.Setup(h => h.CreatePasswordEncrypt(regi.Password)).Returns("password");

            var IToken = new Mock<ITokenCreate>();
            IToken.Setup(a => a.TokenCreate(It.IsAny<int>(), regi.Name)).Returns("token");

            var IVali = new Mock<IValidarUsuario>();
            IVali.Setup(v => v.ValidarUsuario(regi)).Returns(null);

            var controller = new AuthController(Iuser.Object, IHash.Object, IToken.Object, IVali.Object);

            var registro = await controller.Register(regi);

            var resul = registro.Result as ObjectResult;
            Assert.IsNotNull(resul);
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task NoDebePoderRegistarUsuarioNombreNoValido()
        {
            var regi = new UsuarioRegister()
            {
                Email = "prueba@gmail.com",
                Name = "",
                Password = "Prueba123"
            };

            var Iuser = new Mock<INR_Usuario>();
            Iuser.Setup(u => u.NR_Buscar_CorreoS("ricardo@gmail.com")).ReturnsAsync(null);
            Iuser.Setup(u => u.NR_Registrar_Usuario(regi)).ReturnsAsync(2);

            var rs = new { ok = false, msg = "El nombre es obligatorio" };

            var IHash = new Mock<ICreateHash>();
            IHash.Setup(h => h.CreatePasswordEncrypt(regi.Password)).Returns("password");

            var IToken = new Mock<ITokenCreate>();
            IToken.Setup(a => a.TokenCreate(It.IsAny<int>(), regi.Name)).Returns("token");

            var IVali = new Mock<IValidarUsuario>();
            IVali.Setup(v => v.ValidarUsuario(regi)).Returns(rs);

            var controller = new AuthController(Iuser.Object, IHash.Object, IToken.Object, IVali.Object);

            var registro = await controller.Register(regi);

            var resul = registro.Result as ObjectResult;

            Assert.IsNotNull(resul);
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(500));
            Assert.AreEqual(rs, (registro.Result as ObjectResult).Value);
        }

        [Test]
        public async Task NodebePoderRegistrarCorreoNoValido()
        {
            var regi = new UsuarioRegister()
            {
                Email = "prueba",
                Name = "Prueba",
                Password = "Prueba123"
            };

            var Iuser = new Mock<INR_Usuario>();
            Iuser.Setup(u => u.NR_Buscar_CorreoS("ricardo@gmail.com")).ReturnsAsync(null);
            Iuser.Setup(u => u.NR_Registrar_Usuario(regi)).ReturnsAsync(2);

            var IHash = new Mock<ICreateHash>();
            IHash.Setup(h => h.CreatePasswordEncrypt(regi.Password)).Returns("password");

            var IToken = new Mock<ITokenCreate>();
            IToken.Setup(t => t.TokenCreate(It.IsAny<int>(), regi.Name)).Returns("token");

            var rs = new { ok = false, msg = "Ingrese un correo valido" };

            var IVali = new Mock<IValidarUsuario>();
            IVali.Setup(v => v.ValidarUsuario(regi)).Returns(rs);

            var controller = new AuthController(Iuser.Object, IHash.Object, IToken.Object, IVali.Object);

            var registro = await controller.Register(regi);
            var resul = registro.Result as ObjectResult;
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(500));
            Assert.AreEqual(rs, (registro.Result as ObjectResult).Value);
        }
        [Test]
        public async Task NodebePoderRegistrarContraseñaNoValida()
        {
            var regi = new UsuarioRegister()
            {
                Email = "prueba@gmail.com",
                Name = "Prueba",
                Password = ""
            };

            var Iuser = new Mock<INR_Usuario>();
            Iuser.Setup(u => u.NR_Buscar_CorreoS("ricardo@gmail.com")).ReturnsAsync(null);
            Iuser.Setup(u => u.NR_Registrar_Usuario(regi)).ReturnsAsync(2);

          
            var IHash = new Mock<ICreateHash>();
            IHash.Setup(h => h.CreatePasswordEncrypt(regi.Password)).Returns("password");

            var IToken = new Mock<ITokenCreate>();
            IToken.Setup(t => t.TokenCreate(It.IsAny<int>(), regi.Name)).Returns("token");

            var rs = new { ok = false, msg = "La contraseña debe de ser de 6 caracteres" };
            var IVali = new Mock<IValidarUsuario>();
            IVali.Setup(v => v.ValidarUsuario(regi)).Returns(rs);

            var controller = new AuthController(Iuser.Object, IHash.Object, IToken.Object, IVali.Object);

            var registro = await controller.Register(regi);
            var resul = registro.Result as ObjectResult;
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(500));
            Assert.AreEqual(rs, (registro.Result as ObjectResult).Value);
        }

        [Test]
        public async Task NodebePoderRegistrarCorreoYaRegistrado()
        {
            var regi = new UsuarioRegister()
            {
                Email = "prueba@gmail.com",
                Name = "Prueba",
                Password = "Prueba123"
            };

            var rs = new { ok = false, msg = "El correo ya esta registrado" };

            var Iuser = new Mock<INR_Usuario>();
            Iuser.Setup(u => u.NR_Buscar_CorreoS("prueba@gmail.com")).ReturnsAsync(rs);
            Iuser.Setup(u => u.NR_Registrar_Usuario(regi)).ReturnsAsync(2);

            var IHash = new Mock<ICreateHash>();
            IHash.Setup(h => h.CreatePasswordEncrypt(regi.Password)).Returns("password");

            var IToken = new Mock<ITokenCreate>();
            IToken.Setup(t => t.TokenCreate(It.IsAny<int>(), regi.Name)).Returns("token");

            var IVali = new Mock<IValidarUsuario>();
            IVali.Setup(v => v.ValidarUsuario(regi)).Returns(null);

            var controller = new AuthController(Iuser.Object, IHash.Object, IToken.Object, IVali.Object);

            var registro = await controller.Register(regi);
            var resul = registro.Result as ObjectResult;
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(500));
            Assert.AreEqual(rs, (registro.Result as ObjectResult).Value);
        }
    }
}
