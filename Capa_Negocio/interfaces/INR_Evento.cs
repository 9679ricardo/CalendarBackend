using Capa_Entidad;

namespace Capa_Negocio
{
    public interface INR_Evento
    {
        Task<Resp> INR_Editar_Evento(Evento evento);
        Task<Resp> INR_Eliminar_Evento(int Id_Even, int uid);
        Task<Resp> INR_Eliminar_Evento_Usuario(int Id_Even);
        Task<Resp> INR_Eliminar_Evento_Usuario_Relacion(int Id_Even, int Id_User);
        Task<Resp> INR_Eliminar_All_Notificacion_Evento_Usuario(int uid);
        Task<List<EventoPart>> INR_Mostar_Todos_Evento_Usuario(int uid);
        Task<List<Guests>> INR_Mostar_Todos_Usuarios_Evento(int Id_Even);
        Task<int> INR_Registrar_Evento(Evento evento);
        Task<int> INR_Registrar_Evento_Relacion(int Id_Even, int uid);
        Evento RegisterData(ClaimsIdent identity, EventoRegister Request);
        Evento RegisterData(Guests guests, EventoRegister Request, ClaimsIdent ident);
        Evento UpdateData(ClaimsIdent identity, EventoUpdate Request);
        Task<Evento> INR_Buscar_Evento(int Id_Even);
    }
}
