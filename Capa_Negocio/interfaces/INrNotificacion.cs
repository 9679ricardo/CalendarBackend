using Capa_Entidad;

namespace Capa_Negocio
{
    public interface INrNotificacion
    {
        Task<int> NR_Registrar_Notificacion(int id_Not, List<Guests> ListGue, int id_Usu);
        Task<bool> NR_Eliminar_Notificacion(int Id_Not, int Id_Usu);
        Task<List<Notificacion>> INR_Mostar_Todos_Notificacion_Usuario(int Id_Usu);
    } 
}
