using CalendarBackend;
using CalendarBackend.Controllers;
using Capa_Entidad;
using Capa_Negocio;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework.Internal.Execution;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace CalendarBackendTest.Controllers
{
    public class NotificationControllerTest
    {
        [Test]
        public async Task DebePoderDevolverUnaListaDeNotificaciones()
        {
            var IToken = new Mock<ITokenCreate>();
            var INotificacion = new Mock<INR_Notificacion>();
            var IEvento = new Mock<INR_Evento>();
            var ISend = new Mock<ISendEmail>();

            var identity = new ClaimsIdentity();
            identity.AddClaims(new List<Claim>()
            {
                new Claim(ClaimTypes.Surname, "Ricardo"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            });

            var principal = new ClaimsPrincipal(identity);

            ClaimsIdent ident = new()
            {
                Id_Usuario = 1,
                Names = "Ricardo",
                Token = "token"
            };

            IToken.Setup(p => p.GetUser(principal.Claims)).Returns(ident);
            INotificacion.Setup(p => p.INR_Mostar_Todos_Notificacion_Usuario(ident.Id_Usuario)).ReturnsAsync(new List<Notificacion>());

            var controller = new NotificationController(IEvento.Object, IToken.Object, INotificacion.Object, ISend.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = principal
                    }
                }
            };

            var resul = await controller.GetAllNotificationUsuario();
            Assert.IsNotNull(resul);
            var view = resul.Result as ObjectResult;
            Assert.That(view, Is.Not.Null);
            Assert.That(view.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task DebePoderGuardarUnaNotificacion()
        { 
            var IToken = new Mock<ITokenCreate>();
            var INotificacion = new Mock<INR_Notificacion>();
            var IEvento = new Mock<INR_Evento>();
            var ISend = new Mock<ISendEmail>();

            var identity = new ClaimsIdentity();
            identity.AddClaims(new List<Claim>()
            {
                new Claim(ClaimTypes.Surname, "Ricardo"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            });

            var principal = new ClaimsPrincipal(identity);

            ClaimsIdent ident = new()
            {
                Id_Usuario = 1,
                Names = "Ricardo",
                Token = "token"
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
                State = "Activo",
            };

            NotificacionDelete noti = new()
            {
                Id_Not = 1,
                Id_Nota = evento.Id
            };

            IToken.Setup(p => p.GetUser(principal.Claims)).Returns(ident);
            IEvento.Setup(p => p.INR_Buscar_Evento(noti.Id_Nota)).ReturnsAsync(evento);
            IEvento.Setup(p => p.INR_Registrar_Evento_Relacion(evento.Id, ident.Id_Usuario)).ReturnsAsync(1);
            INotificacion.Setup(p => p.NR_Eliminar_Notificacion(noti.Id_Not, ident.Id_Usuario)).ReturnsAsync(true);
            IEvento.Setup(p => p.INR_Mostar_Todos_Usuarios_Evento(evento.Id)).ReturnsAsync(new List<Guests>());

            var controller = new NotificationController(IEvento.Object, IToken.Object, INotificacion.Object, ISend.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = principal
                    }
                }
            };

            var resul = await controller.Create(noti);
            Assert.IsNotNull(resul);
            var view = resul.Result as ObjectResult;
            Assert.That(view, Is.Not.Null);
            Assert.That(view.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task DebePoderEliminarUnaNotificacion()
        {
            var IToken = new Mock<ITokenCreate>();
            var INotificacion = new Mock<INR_Notificacion>();
            var IEvento = new Mock<INR_Evento>();
            var ISend = new Mock<ISendEmail>();

            var identity = new ClaimsIdentity();
            identity.AddClaims(new List<Claim>()
            {
                new Claim(ClaimTypes.Surname, "Ricardo"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            });

            var principal = new ClaimsPrincipal(identity);

            ClaimsIdent ident = new()
            {
                Id_Usuario = 1,
                Names = "Ricardo",
                Token = "token"
            };

            NotificacionDelete noti = new()
            {
                Id_Not = 1,
                Id_Nota = 1
            };

            IToken.Setup(p => p.GetUser(principal.Claims)).Returns(ident);
            INotificacion.Setup(p => p.NR_Eliminar_Notificacion(noti.Id_Not, ident.Id_Usuario)).ReturnsAsync(true);

            var controller = new NotificationController(IEvento.Object, IToken.Object, INotificacion.Object, ISend.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = principal
                    }
                }
            };

            var resul = await controller.Delete(noti);
            Assert.IsNotNull(resul);
            var view = resul.Result as ObjectResult;
            Assert.That(view, Is.Not.Null);
            Assert.That(view.StatusCode, Is.EqualTo(201));
        }
        [Test]
        public async Task DebePoderEnviarUnaNotificacion()
        {
            var IToken = new Mock<ITokenCreate>();
            var INotificacion = new Mock<INR_Notificacion>();
            var ISend = new Mock<ISendEmail>();
            var IEvento = new Mock<INR_Evento>();

            var identity = new ClaimsIdentity();
            identity.AddClaims(new List<Claim>()
            {
                new Claim(ClaimTypes.Surname, "Ricardo"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            });

            var principal = new ClaimsPrincipal(identity);

            ClaimsIdent ident = new()
            {
                Id_Usuario = 1,
                Names = "Ricardo",
                Token = "token"
            };

            NotificacionDelete noti = new()
            {
                Id_Not = 1,
                Id_Nota = 1
            };

            List<Guests> list = new()
            {
                new Guests(){ id = 1, Email = "user1@gmail.com", Name = "user1"},
                new Guests(){ id = 2, Email = "user2@gmail.com", Name = "user2"}
            };

            ListGuest Request = new()
            {
                Lastinsert = 1,
                ListGutes = list,
            };

            IToken.Setup(p => p.GetUser(principal.Claims)).Returns(ident);
            INotificacion.Setup(p => p.NR_Registrar_Notificacion(Request.Lastinsert,Request.ListGutes, ident.Id_Usuario)).ReturnsAsync(1);
            ISend.Setup(s => s.sendEmailUsers("user1@gmail.com")).Returns(true);

            var controller = new NotificationController(IEvento.Object, IToken.Object, INotificacion.Object, ISend.Object)
            {
                ControllerContext = new ControllerContext()
                {
                    HttpContext = new DefaultHttpContext()
                    {
                        User = principal
                    }
                }
            };
            
            var resul = await controller.CreateGuests(Request);
            Assert.IsNotNull(resul);
            var view = resul.Result as ObjectResult;
            Assert.That(view, Is.Not.Null);
            Assert.That(view.StatusCode, Is.EqualTo(201));
        }
    }
}
