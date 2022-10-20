using Capa_Entidad;

namespace Capa_Negocio
{
    public interface INR_Evento
    {
        Task<object> INR_Editar_Evento(Evento evento);
        Task<object> INR_Eliminar_Evento(int Id_Even, int uid);
        Task<List<Evento>> INR_Mostar_Todos_Evento_Usuario(int uid);
        Task<int> INR_Registrar_Evento(Evento evento);
        Evento RegisterData(ClaimsIdent identity, EventoRegister Request);
        Evento UpdateData(ClaimsIdent identity, EventoUpdate Request);
        Task<Evento> INR_Buscar_Evento(int Id_Even);
    }
}
