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

        public Resp ValidarLogin(UsuarioLogin usuario)
        {
            if (mValidarCampos.ValidarEmail(usuario.Email)) return new() { Ok = false, msg = "Ingrese un correo valido" };
            if (string.IsNullOrEmpty(usuario.Password)) return new() { Ok = false, msg = "Ingrese un contraseña valida" };

            return new Resp() { Ok = true };
        }

        public Resp ValidarUsuario(UsuarioRegister usuario)
        {
            if (string.IsNullOrEmpty(usuario.Name)) return new() { Ok = false, msg = "El nombre es obligatorio" };

            if (usuario.Password.Length < 6) return new() { Ok = false, msg = "La contraseña debe de ser de 6 caracteres" };

            if (mValidarCampos.ValidarEmail(usuario.Email)) return new() { Ok = false, msg = "Ingrese un correo valido" };

            return new() { Ok = true };
        }
    }
}
