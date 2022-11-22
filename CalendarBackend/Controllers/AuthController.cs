using Capa_Entidad;
using Capa_Negocio;
using Capa_Validacion;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CalendarBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        readonly INrUsuario mUsuario;
        readonly ICreateHash mCreateHash;
        readonly ITokenCreate mTokenCreate;
        readonly IValidarUsuario mVUsuario;

        public AuthController(INrUsuario mUsuario, ICreateHash mCreateHash, ITokenCreate mTokenCreate, IValidarUsuario mVUsuario)
        {
            this.mUsuario = mUsuario;
            this.mCreateHash = mCreateHash;
            this.mTokenCreate = mTokenCreate;
            this.mVUsuario = mVUsuario;
        }

        [HttpPost("/auth/new")]
        public async Task<ActionResult<UsuarioRegister>> Register(UsuarioRegister request)
        {
            try
            {
                var validar = mVUsuario.ValidarUsuario(request);
                if (!validar.Ok) return StatusCode(500, validar);

                var respuesta = await mUsuario.NR_Buscar_CorreoS(request.Email);
                if (!respuesta.Ok) return StatusCode(500, respuesta);

                string hsp = mCreateHash.CreatePasswordEncrypt(request.Password);
                if (string.IsNullOrEmpty(hsp)) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod4" });

                request.Password = hsp;
                var uid = await mUsuario.NR_Registrar_Usuario(request);
                if (uid <= 0) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod5" });

                var token = mTokenCreate.TokenCreate(uid, request.Name);
                if (string.IsNullOrEmpty(token)) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: token" });

                return StatusCode(201, new { ok = true, uid, name = request.Name, token, request.Email });
            }
            catch (Exception)
            {
                return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod6" });
            }
        }
        
        [HttpPost("/auth")]
        public async Task<ActionResult<UsuarioLogin>> Login(UsuarioLogin request)
        {
            try
            {
                var validar = mVUsuario.ValidarLogin(request);
                if (!validar.Ok) return StatusCode(500, validar);

                var login = await mUsuario.NR_Login(request.Email);
                if (login == null) return StatusCode(500, new { ok = false, msg = "Credenciales incorrectas" });
                if (login.Password == null) return StatusCode(500, new { ok = false, msg = "Credenciales incorrectas" });

                string hsp = mCreateHash.PasswordDecrypt(login.Password);
                if (!hsp.Equals(request.Password)) return StatusCode(500, new { ok = false, msg = "Credenciales incorrectas" });

                var token = mTokenCreate.TokenCreate(login.uid, login.Name);
                if (string.IsNullOrEmpty(token)) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: token" });

                return StatusCode(201, new { ok = true, login.uid, name = login.Name, token, request.Email });
            }
            catch
            {
                return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod6" });
            }

        }

        [Authorize]
        [HttpPost("/auth/renew")]
        public ActionResult<string> Renew(string token)
        {
            try
            {
                if (HttpContext.User.Identity is not ClaimsIdentity identity) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" });
                ClaimsIdent renew = mTokenCreate.ValidarToken(token, identity.Claims);
                if (renew == null) return StatusCode(500, new { ok = false, msg = "Token no valido" });
                if (string.IsNullOrEmpty(renew.Token)) return StatusCode(500, new { ok = false, msg = "Token no valido" });

                return StatusCode(201, new { ok = true, uid = renew.Id_Usuario, name = renew.Names, token = renew.Token });
            }
            catch
            {
                return StatusCode(500, new { ok = false, msg = "Token no valido" });
            }
        }

        [Authorize]
        [HttpPost("/user")]
        public async Task<ActionResult<Evento>> GetAsUsuario(string email)
        {
            try
            {
                var login = await mUsuario.NR_Login(email);
                if (login == null) return StatusCode(500, new { ok = false, msg = "No existe un usuario con este correo" });
                if (string.IsNullOrEmpty(login.Email)) return StatusCode(500, new { ok = false, msg = "No existe un usuario con este correo" });

                return StatusCode(200, new { ok = true, login.Email, id = login.uid, login.Name });

            }
            catch (Exception)
            {
                return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod6" });
            }
        }
    }
}
