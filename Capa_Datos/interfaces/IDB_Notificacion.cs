using Capa_Entidad;
using System.Data;

namespace Capa_Datos
{
    public interface IDB_Notificacion
    {
        Task<int> BD_Registrar_Notificacion(NotificacionRegister notificacion);
        Task<bool> BD_Eliminar_Notificacion(int Id_Not, int Id_Usu);
        Task<DataTable> DB_Mostar_Todas_Notificacion_Usuario(int Id_Usu);
    }
}
