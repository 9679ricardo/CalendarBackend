using Capa_Entidad;
using Capa_Negocio;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CalendarBackend.Controllers
{
    public class NotificationController : Controller
    {

        private readonly INR_Evento mEvento;
        readonly ITokenCreate mTokenCreate;
        private readonly INR_Notificacion mNotificacion;
        private readonly ISendEmail mSendEmail;
        public NotificationController(INR_Evento mEvento, ITokenCreate mTokenCreate, INR_Notificacion mNotificacion, ISendEmail mSendEmail)
        {
            this.mEvento = mEvento;
            this.mTokenCreate = mTokenCreate;
            this.mNotificacion = mNotificacion;
            this.mSendEmail = mSendEmail;
        }
        [HttpGet("/notification")]
        public async Task<ActionResult<Evento>> GetAllNotificationUsuario()
        {
            try
            {
                /** obtener id del usuario **/
                if (HttpContext.User.Identity is not ClaimsIdentity identity) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });
                ClaimsIdent user = mTokenCreate.GetUser(identity.Claims);
                if (user.Id_Usuario <= 0) StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });

                /** cargamos todos los eventos  **/
                var list = await mNotificacion.INR_Mostar_Todos_Notificacion_Usuario(user.Id_Usuario);
                return StatusCode(201, new { ok = true, notifications = list });

            }
            catch (Exception)
            {
                return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod6" });
            }
        }

        [HttpPost("/notification/accept")]
        public async Task<ActionResult<Notificacion>> Create([FromBody] NotificacionDelete Request)
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

                /** registrar evento relacion**/
                int relac = await mEvento.INR_Registrar_Evento_Relacion(DbProducto.Id, user.Id_Usuario);
                if (relac <= 0) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod77" });

                await mNotificacion.NR_Eliminar_Notificacion(Request.Id_Not, user.Id_Usuario);

                var listGuests = await mEvento.INR_Mostar_Todos_Usuarios_Evento(DbProducto.Id);

                return StatusCode(201, new { ok = true, evento = DbProducto, listGuests });
            }
            catch (Exception)
            {
                return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod6" });
            }
        }

        [HttpPost("/notification/decline")]
        public async Task<ActionResult<Notificacion>> Delete([FromBody] NotificacionDelete Request)
        {
            try
            {
                /** obtener id del usuario **/
                if (HttpContext.User.Identity is not ClaimsIdentity identity) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });
                ClaimsIdent user = mTokenCreate.GetUser(identity.Claims);
                if (user.Id_Usuario <= 0) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });

                // Eliminar notificacion
                await mNotificacion.NR_Eliminar_Notificacion(Request.Id_Not, user.Id_Usuario);

                return StatusCode(201, new { ok = true });
            }
            catch (Exception)
            {
                return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod6" });
            }
        }

        [HttpPost("/notificacion")]
        public async Task<ActionResult<ListGuest>> CreateGuests([FromBody] ListGuest Request)
        {
            try
            {
                /** obtener id del usuario **/
                if (HttpContext.User.Identity is not ClaimsIdentity identity) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });
                ClaimsIdent user = mTokenCreate.GetUser(identity.Claims);
                if (user.Id_Usuario <= 0) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });

                if(Request.ListGutes.Count > 0)
                {
                    await mNotificacion.NR_Registrar_Notificacion(Request.Lastinsert, Request.ListGutes, user.Id_Usuario);

                    foreach(var item in Request.ListGutes)
                    {
                        mSendEmail.sendEmailUsers(item.Email);
                    }
                }

                return StatusCode(201, new { ok = true });
            }
            catch (Exception)
            {
                return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod6" });
            }
        }
    }
}
