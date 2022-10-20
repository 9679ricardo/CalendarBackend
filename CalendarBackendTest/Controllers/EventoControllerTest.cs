﻿using CalendarBackend.Controllers;
using Capa_Entidad;
using Capa_Negocio;
using Capa_Validacion;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;
using NUnit.Framework.Internal.Execution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CalendarBackendTest.Controllers
{
    public class EventoControllerTest
    {
        [Test]
        public async Task DebePoderDevolverUnaLista()
        {
            var IEvento = new Mock<INR_Evento> ();
            var IToken = new Mock<ITokenCreate>();

            var principal = new ClaimsPrincipal(new ClaimsIdentity(new List<Claim>()
            {
                new Claim(ClaimTypes.Surname, "Ricardo"),
                new Claim(ClaimTypes.NameIdentifier, "1")
            }, "Bear Token"));

            var list = new List<Event>();

            IToken.Setup(p => p.GetUser(principal.Claims)).Returns(new ClaimsIdent() { Id_Usuario = 1, Names = "Ricardo" });
            IEvento.Setup(p => p.INR_Mostar_Todos_Evento_Usuario(1)).ReturnsAsync(new List<Evento>());

            var controller = new EventoController(null,IEvento.Object, IToken.Object)
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
            Assert.IsNotNull(events);
            var resul = events.Result as ObjectResult;
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task DebePoderRegistrarUnaNota()
        {
            var IEvento = new Mock<INR_Evento>();
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
            IEvento.Setup(p => p.RegisterData(ident, register)).Returns(evento);
            IEvento.Setup(p => p.INR_Registrar_Evento(evento)).ReturnsAsync(1);
            validate.Setup(p => p.ValidarEvento(evento)).Returns(null);

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
            Assert.IsNotNull(events);
            var resul = events.Result as ObjectResult;
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task DebePoderActualizarUnaNota()
        {
            var IEvento = new Mock<INR_Evento>();
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
                User = "ricardo",
                UserUid = 1
            };

            ClaimsIdent ident = new()
            {
                Id_Usuario = 1,
                Names = "Ricardo",
                Token = "token"
            };
             
            IToken.Setup(p => p.GetUser(principal.Claims)).Returns(ident);
            IEvento.Setup(p => p.UpdateData(ident, update)).Returns(evento);
            validate.Setup(p => p.ValidarEvento(evento)).Returns(null);
            validate.Setup(p => p.ValidarEventoId(1)).Returns(null);

            IEvento.Setup(p => p.INR_Buscar_Evento(1)).ReturnsAsync(evento);
            IEvento.Setup(p => p.INR_Editar_Evento(evento)).ReturnsAsync(null);

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
            Assert.IsNotNull(events);
            var resul = events.Result as ObjectResult;
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(201));
        }

        [Test]
        public async Task DebePoderEliminarUnaNota()
        {
            var IEvento = new Mock<INR_Evento>();
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
                UserUid = 1
            };

            ClaimsIdent ident = new()
            {
                Id_Usuario = 1,
                Names = "Ricardo",
                Token = "token"
            };

            IToken.Setup(p => p.GetUser(principal.Claims)).Returns(ident);
            validate.Setup(p => p.ValidarEventoId(1)).Returns(null);
            IEvento.Setup(p => p.INR_Buscar_Evento(1)).ReturnsAsync(evento);
            IEvento.Setup(p => p.INR_Eliminar_Evento(1,1)).ReturnsAsync(null);

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
            Assert.IsNotNull(events);
            var resul = events.Result as ObjectResult;
            Assert.That(resul, Is.Not.Null);
            Assert.That(resul.StatusCode, Is.EqualTo(201));
        }
    }
}