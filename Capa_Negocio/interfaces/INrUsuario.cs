using Capa_Entidad;

namespace Capa_Negocio
{
    public interface INrUsuario
    {
        Task<Resp> NR_Buscar_CorreoS(string email);
        Task<int> NR_Registrar_Usuario(UsuarioRegister usuario);
        Task<Usuario> NR_Login(string email);
    }
}
