using Capa_Entidad;
using System.Data;

namespace Capa_Datos
{
    public interface IDB_Evento
    {
        Task<bool> BD_Editar_Evento(Evento evento);
        Task<bool> BD_Eliminar_Evento(int Id_Even, int uid);
        Task<bool> BD_Eliminar_Evento_Relacion(int Id_Even, int uid);
        Task<bool> BD_Eliminar_Evento_Usuario(int Id_Even);
        Task<DataTable> DB_Mostar_Todos_Evento_Usuario(int uid);
        Task<DataTable> DB_Mostar_Todos_Usuarios_Evento(int Id_Even);
        Task<int> BD_Registrar_Evento(Evento evento);
        Task<int> BD_Registrar_Evento_Relacion(int Id_Even, int uid);
        Task<DataTable> BD_Buscar_Evento(int Id_Not);

    }
}
