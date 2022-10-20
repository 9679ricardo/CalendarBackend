using Capa_Entidad;
using System.Data;

namespace Capa_Datos
{
    public interface IBD_Usuario
    {
        Task<bool> BD_Buscar_Correo(string email);
        Task<int> BD_Registrar_Usuario(UsuarioRegister usuario);
        Task<DataTable> DB_Login(string email);
    }
}
