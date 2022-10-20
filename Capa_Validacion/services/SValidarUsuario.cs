using Capa_Entidad;

namespace Capa_Validacion
{
    public class SValidarUsuario : IValidarUsuario
    {
        readonly IValidarCampos mValidarCampos;
        public SValidarUsuario(IValidarCampos mValidarCampos)
        {
            this.mValidarCampos = mValidarCampos;
        }

        public object ValidarLogin(UsuarioLogin usuario)
        {
            if (mValidarCampos.ValidarEmail(usuario.Email)) return new { ok = false, msg = "Ingrese un correo valido" };
            if (string.IsNullOrEmpty(usuario.Password)) return new { ok = false, msg = "Ingrese un contraseña valida" };

            return null;
        }

        public object ValidarUsuario(UsuarioRegister usuario)
        {
            if (string.IsNullOrEmpty(usuario.Name)) return new { ok = false, msg = "El nombre es obligatorio" };

            if (usuario.Password.Length < 6) return new { ok = false, msg = "La contraseña debe de ser de 6 caracteres" };

            if (mValidarCampos.ValidarEmail(usuario.Email)) return new { ok = false, msg = "Ingrese un correo valido" };

            return null;
        }
    }
}
