using Capa_Entidad;
using Capa_Negocio;
using Capa_Validacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CalendarBackend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EventoController : Controller
    {
        private readonly IValidarEvento mVEvento;
        private readonly INR_Evento mEvento;
        readonly ITokenCreate mTokenCreate;
        public EventoController(IValidarEvento mVEvento, INR_Evento mEvento, ITokenCreate mTokenCreate)
        {
            this.mVEvento = mVEvento;
            this.mEvento = mEvento;
            this.mTokenCreate = mTokenCreate;
        }

        [HttpGet("/events")]
        public async Task<ActionResult<EventoPart>> GetAllEventosUsuario()
        {
            try
            {
                /** obtener id del usuario **/
                if (HttpContext.User.Identity is not ClaimsIdentity identity) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });
                ClaimsIdent user = mTokenCreate.GetUser(identity.Claims);
                if (user.Id_Usuario <= 0) StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });

                /** cargamos todos los eventos  **/
                var list = await mEvento.INR_Mostar_Todos_Evento_Usuario(user.Id_Usuario);
                return StatusCode(201, new { ok = true, eventos = list });

            }
            catch (Exception)
            {
                return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod6" });
            }
        }

        [HttpPost("/events")]
        public async Task<ActionResult<EventoRegister>> Create([FromBody] EventoRegister Request)
        {
            try
            {
                /** obtener id del usuario **/
                if (HttpContext.User.Identity is not ClaimsIdentity identity) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });
                ClaimsIdent user = mTokenCreate.GetUser(identity.Claims);
                if (user.Id_Usuario <= 0) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });

                var even = mEvento.RegisterData(user, Request);

                /** validar evento **/
                var validar = mVEvento.ValidarEvento(even);
                if (validar != null) return StatusCode(500, validar);

                /** registrar evento **/
                int lastlast_insert = await mEvento.INR_Registrar_Evento(even);
                if (lastlast_insert <= 0) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod8" });
                even.Id = lastlast_insert;

                /** Registrar relacion **/

                int relac = await mEvento.INR_Registrar_Evento_Relacion(lastlast_insert, user.Id_Usuario);
                if (relac <= 0) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod887" });

                return StatusCode(201, new { ok = true, evento = even });
            }
            catch (Exception)
            {
                return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod6" });
            }
        }

        [HttpPut("/events")]
        public async Task<ActionResult<EventoUpdate>> Update([FromBody] EventoUpdate Request)
        {
            try
            {
                /** obtener id del usuario **/
                if (HttpContext.User.Identity is not ClaimsIdentity identity) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });
                ClaimsIdent user = mTokenCreate.GetUser(identity.Claims);
                if (user.Id_Usuario <= 0) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });

                var env = mEvento.UpdateData(user, Request);

                /** validar evento **/
                var validar = mVEvento.ValidarEvento(env);
                if (validar != null) return StatusCode(500, validar);
                var val = mVEvento.ValidarEventoId(Request.Id);
                if (val != null) return StatusCode(500, val);

                /** buscamos el evento **/
                Evento DbProducto = await mEvento.INR_Buscar_Evento(Request.Id);
                if (DbProducto == null) return StatusCode(500, new { ok = false, Request.Id, msg = "No existe una nota con ese id" });
                if (DbProducto.Id <= 0) return StatusCode(500, new { ok = false, Request.Id, msg = "No existe una nota con ese id" });

                /** validamos que el usuario a modificar se el mismo que lo creo **/
                if (DbProducto.UserUid != user.Id_Usuario) return StatusCode(500, new { ok = false, Request.Id, msg = "No tiene privilegio de editar este evento" });
                if(DbProducto.IdCre != user.Id_Usuario) return StatusCode(500, new { ok = false, Request.Id, msg = "No tiene privilegio de editar este evento" });

                env.UserUid = user.Id_Usuario;
                env.User = user.Names;

                /** editamos el evento en la base de datos **/
                var up = await mEvento.INR_Editar_Evento(env);
                if (up != null) return StatusCode(500, up);
                
                return StatusCode(201, new { ok = true, evento = env });
            }
            catch (Exception)
            {
                return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod6" });
            }
        }

        [HttpDelete("/events")]
        public async Task<ActionResult<Evento>> Delete(int Id)
        {
            try
            {
                /** obtener id del usuario **/
                if (HttpContext.User.Identity is not ClaimsIdentity identity) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });
                ClaimsIdent user = mTokenCreate.GetUser(identity.Claims);
                if (user.Id_Usuario <= 0) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });

                var val = mVEvento.ValidarEventoId(Id);
                if (val != null) return StatusCode(500, val);

                /** buscamos el evento **/
                Evento DbProducto = await mEvento.INR_Buscar_Evento(Id);
                if (DbProducto == null) return StatusCode(500, new { ok = false, msg = "No existe una nota con ese id" });
                if (DbProducto.Id <= 0) return StatusCode(500, new { ok = false, msg = "No existe una nota con ese id" });

                /** validamos que el usuario a modificar se el mismo que lo creo **/
                if (DbProducto.UserUid != user.Id_Usuario) return StatusCode(500, new { ok = false, msg = "No tiene privilegio de eliminar este evento" });
                if (DbProducto.IdCre != user.Id_Usuario) return StatusCode(500, new { ok = false,  msg = "No tiene privilegio de eliminar este evento" });

                /** eliminamos el evento relacion en la base de datos **/
                var delR = await mEvento.INR_Eliminar_Evento_Usuario(Id);
                if (delR != null) return StatusCode(500, delR);

                /** eliminamos el evento en la base de datos **/
                var del = await mEvento.INR_Eliminar_Evento(Id, user.Id_Usuario);
                if (del != null) return StatusCode(500, del);

                return StatusCode(201, new { ok = true });
            }
            catch (Exception)
            {
                return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod6" });
            }
        }

        [HttpPut("/event/delete")]
        public async Task<ActionResult<Notificacion>> DeleteRe([FromBody] Relacion Request)
        {
            try
            {
                /** obtener id del usuario **/
                if (HttpContext.User.Identity is not ClaimsIdentity identity) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });
                ClaimsIdent user = mTokenCreate.GetUser(identity.Claims);
                if (user.Id_Usuario <= 0) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });

                /** buscamos el evento **/
                Evento DbProducto = await mEvento.INR_Buscar_Evento(Request.Id_Nota);
                if (DbProducto == null) return StatusCode(500, new { ok = false, Request.Id_Nota, msg = "No existe una nota con ese id" });
                if (DbProducto.Id <= 0) return StatusCode(500, new { ok = false, Request.Id_Nota, msg = "No existe una nota con ese id" });

                /** validamos que el usuario a modificar se el mismo que lo creo **/
                if (DbProducto.UserUid != user.Id_Usuario) return StatusCode(500, new { ok = false, Request.Id_User, msg = "No tiene privilegio de editar este evento" });
                if (DbProducto.IdCre != user.Id_Usuario) return StatusCode(500, new { ok = false, Request.Id_User, msg = "No tiene privilegio de editar este evento" });

                /** eliminamos el evento relacion en la base de datos **/
                var delR = await mEvento.INR_Eliminar_Evento_Usuario_Relacion(DbProducto.Id, Request.Id_User);
                if (delR != null) return StatusCode(500, delR);

                return StatusCode(201, new { ok = true });
            }
            catch (Exception)
            {
                return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod6" });
            }
        }
    }
}
