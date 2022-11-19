using Capa_Entidad;

namespace Capa_Validacion
{
    public interface IValidarUsuario
    {
        Resp ValidarUsuario(UsuarioRegister usuario);
        Resp ValidarLogin(UsuarioLogin usuario);
    }
}
