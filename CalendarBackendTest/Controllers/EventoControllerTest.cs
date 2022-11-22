using CalendarBackend.Controllers;
using Capa_Entidad;
using Capa_Negocio;
using Capa_Validacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework.Internal.Execution;
using System.Security.Claims;

namespace CalendarBackendTest.Controllers
{
    public class EventoControllerTest
    {
        [Test]
        public async Task DebePoderDevolverUnaLista()
        {
            var IEvento = new Mock<INrEvento> ();
            var IToken = new Mock<ITokenCreate>();
            var IVent = new Mock<IValidarEvento>();

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Surname, "Ricardo"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "Bear Token"));

            var list = new List<Event>();

            IToken.Setup(p => p.GetUser(principal.Claims)).Returns(new ClaimsIdent() { Id_Usuario = 1, Names = "Ricardo" });
            IEvento.Setup(p => p.INR_Mostar_Todos_Evento_Usuario(1)).ReturnsAsync(new List<EventoPart>());

            var controller = new EventoController(IVent.Object, IEvento.Object, IToken.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = principal
                    }
                }
            };

            var events = await controller.GetAllEventosUsuario();
            Assert.That(events, Is.Not.Null);
            var resul = events.Result as ObjectResult;
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task DebePoderRegistrarUnaNota()
        {
            var IEvento = new Mock<INrEvento>();
            var IToken = new Mock<ITokenCreate>();
            var validate = new Mock<IValidarEvento>();

            var identity = new ClaimsIdentity();
            identity.AddClaims(new List<Claim>()
            {
                new Claim(ClaimTypes.Surname, "Ricardo"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            });

            var principal = new ClaimsPrincipal(identity);

            var list = new List<Event>();

            EventoRegister register = new ()
            {
                Title = "evento 1",
                Notes = "descrip 1",
                Start = "1234",
                End = "1234"
            };

            Evento evento = new()
            {
                Id = 1,
                Title = "evento 1",
                Notes = "descrip 1",
                Start = "1234",
                End = "1234",
                User = "ricardo",
                UserUid = 3
            };

            ClaimsIdent ident = new()
            {
                Id_Usuario = 1,
                Names = "Ricardo",
                Token = "token"
            };

            IToken.Setup(p => p.GetUser(principal.Claims)).Returns(ident);
            IEvento.Setup(p => p.RegisterDataSn(ident, register)).Returns(evento);
            validate.Setup(p => p.ValidarEvento(evento)).Returns(new Resp() { Ok = true });
            IEvento.Setup(p => p.INR_Registrar_Evento(evento)).ReturnsAsync(1);
            IEvento.Setup(p => p.INR_Registrar_Evento_Relacion(evento.Id, ident.Id_Usuario)).ReturnsAsync(1);

            var controller = new EventoController(validate.Object, IEvento.Object, IToken.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = principal
                    }
                }
            };

            var events = await controller.Create(register);
            Assert.That(events, Is.Not.Null);
            var resul = events.Result as ObjectResult;
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task DebePoderActualizarUnaNota()
        {
            var IEvento = new Mock<INrEvento>();
            var IToken = new Mock<ITokenCreate>();
            var validate = new Mock<IValidarEvento>();

            var identity = new ClaimsIdentity();
            identity.AddClaims(new List<Claim>()
            {
                new Claim(ClaimTypes.Surname, "Ricardo"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            });

            var principal = new ClaimsPrincipal(identity);

            var list = new List<Event>();

            EventoUpdate update = new()
            {
                Id = 1,
                Title = "evento 1",
                Notes = "descrip 1",
                Start = "1234",
                End = "1234"
            };

            Evento evento = new()
            {

                Id = 1,
                Title = "evento 1",
                Notes = "descrip 1",
                Start = "1234",
                End = "1234",
                User = "Ricardo",
                UserUid = 1,
                IdCre = 1,
                State = "Active",
            };

            ClaimsIdent ident = new()
            {
                Id_Usuario = 1,
                Names = "Ricardo",
                Token = "token"
            };

            IToken.Setup(p => p.GetUser(principal.Claims)).Returns(ident);
            IEvento.Setup(p => p.UpdateData(ident, update)).Returns(evento);
            validate.Setup(p => p.ValidarEvento(evento)).Returns(new Resp() { Ok = true });
            validate.Setup(p => p.ValidarEventoId(1)).Returns(new Resp() { Ok = true });
            IEvento.Setup(p => p.INR_Buscar_Evento(1)).ReturnsAsync(evento);
            IEvento.Setup(p => p.INR_Editar_Evento(evento)).ReturnsAsync(new Resp() { Ok = true });

            var controller = new EventoController(validate.Object, IEvento.Object, IToken.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = principal
                    }
                }
            };

            var events = await controller.Update(update);
            Assert.That(events, Is.Not.Null);
            var resul = events.Result as ObjectResult;
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task DebePoderEliminarUnaNota()
        {
            var IEvento = new Mock<INrEvento>();
            var IToken = new Mock<ITokenCreate>();
            var validate = new Mock<IValidarEvento>();

            var identity = new ClaimsIdentity();
            identity.AddClaims(new List<Claim>()
            {
                new Claim(ClaimTypes.Surname, "Ricardo"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            });

            var principal = new ClaimsPrincipal(identity);

            var list = new List<Event>();

            Evento evento = new()
            {
                Id = 1,
                Title = "evento 1",
                Notes = "descrip 1",
                Start = "1234",
                End = "1234",
                User = "ricardo",
                UserUid = 1,
                IdCre = 1
            };

            ClaimsIdent ident = new()
            {
                Id_Usuario = 1,
                Names = "Ricardo",
                Token = "token"
            };

            IToken.Setup(p => p.GetUser(principal.Claims)).Returns(ident);
            validate.Setup(p => p.ValidarEventoId(1)).Returns(new Resp() { Ok = true });
            IEvento.Setup(p => p.INR_Buscar_Evento(1)).ReturnsAsync(evento);
            IEvento.Setup(r => r.INR_Eliminar_Notificacion(1)).ReturnsAsync(new Resp() { Ok = true });
            IEvento.Setup(r => r.INR_Eliminar_Evento_Usuario(1)).ReturnsAsync(new Resp() { Ok = true });
            IEvento.Setup(p => p.INR_Eliminar_Evento(1, 1)).ReturnsAsync(new Resp() { Ok = true });

            var controller = new EventoController(validate.Object, IEvento.Object, IToken.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = principal
                    }
                }
            };

            var events = await controller.Delete(1);
            Assert.That(events, Is.Not.Null);
            var resul = events.Result as ObjectResult;
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task DebePoderEliminarRelacionEvento()
        {
            var IEvento = new Mock<INrEvento>();
            var IToken = new Mock<ITokenCreate>();
            var validate = new Mock<IValidarEvento>();

            var identity = new ClaimsIdentity();
            identity.AddClaims(new List<Claim>()
            {
                new Claim(ClaimTypes.Surname, "Ricardo"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            });

            var principal = new ClaimsPrincipal(identity);

            var list = new List<Event>();

            Evento evento = new()
            {
                Id = 1,
                Title = "evento 1",
                Notes = "descrip 1",
                Start = "1234",
                End = "1234",
                User = "ricardo",
                UserUid = 1,
                IdCre = 1
            };

            ClaimsIdent ident = new()
            {
                Id_Usuario = 1,
                Names = "Ricardo",
                Token = "token"
            };

            Relacion relacion = new()
            {
                Id_Nota = evento.Id,
                Id_User = ident.Id_Usuario
            };

            IToken.Setup(p => p.GetUser(principal.Claims)).Returns(ident);
            IEvento.Setup(p => p.INR_Buscar_Evento(1)).ReturnsAsync(evento);
            IEvento.Setup(r => r.INR_Eliminar_Evento_Usuario_Relacion(evento.Id, ident.Id_Usuario)).ReturnsAsync(new Resp() { Ok = true });

            var controller = new EventoController(validate.Object, IEvento.Object, IToken.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = principal
                    }
                }
            };

            var realcionEvent = await controller.DeleteRe(relacion);
            Assert.That(realcionEvent, Is.Not.Null);
            var view = realcionEvent.Result as ObjectResult;
            Assert.That(view, Is.Not.Null);
            Assert.That(view.StatusCode, Is.EqualTo(201));
        }
    }
}
