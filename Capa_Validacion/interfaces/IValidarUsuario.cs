using Capa_Entidad;

namespace Capa_Validacion
{
    public interface IValidarUsuario
    {
        object ValidarUsuario(UsuarioRegister usuario);
        object ValidarLogin(UsuarioLogin usuario);
    }
}
