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
        readonly INR_Usuario mUsuario;
        readonly ICreateHash mCreateHash;
        readonly ITokenCreate mTokenCreate;
        readonly IValidarUsuario mVUsuario;

        public AuthController(INR_Usuario mUsuario, ICreateHash mCreateHash, ITokenCreate mTokenCreate, IValidarUsuario mVUsuario)
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
                /** validar usuario **/
                var validar = mVUsuario.ValidarUsuario(request);
                if (validar != null) return StatusCode(500, validar);

                /** validar que en la base de datos no exista el correo a registrar **/
                var respuesta = await mUsuario.NR_Buscar_CorreoS(request.Email);
                if (respuesta != null) return StatusCode(500, respuesta);

                /** encriptación de contraseña **/
                string hsp = mCreateHash.CreatePasswordEncrypt(request.Password);
                if (string.IsNullOrEmpty(hsp)) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod4" });

                /** registrar usuario en la base de datos **/
                request.Password = hsp;
                var uid = await mUsuario.NR_Registrar_Usuario(request);
                if (uid <= 0) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod5" });

                /** crear token **/
                var token = mTokenCreate.TokenCreate(uid, request.Name);
                if (string.IsNullOrEmpty(token)) return StatusCode(500, new { ok = false, msg = "Por favor hable con el administrador, codigo de error: token" });
                /** retornamos un nuevo objeto con la respuesta **/
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
                /** validar login **/
                var validar = mVUsuario.ValidarLogin(request);
                if (validar != null) return StatusCode(500, validar);

                /** consultamos en la base de datos si existe el email **/
                var login = await mUsuario.NR_Login(request.Email);
                if (login == null) return StatusCode(500, new { ok = false, msg = "Credenciales incorrectas" });
                if (login.Password == null) return StatusCode(500, new { ok = false, msg = "Credenciales incorrectas" });

                /** desencriptar de contraseña **/
                string hsp = mCreateHash.PasswordDecrypt(login.Password);
                if (!hsp.Equals(request.Password)) return StatusCode(500, new { ok = false, msg = "Credenciales incorrectas" });

                /** crear token **/
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
                ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;

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
