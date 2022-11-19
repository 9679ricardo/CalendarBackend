using Capa_Entidad;

namespace Capa_Validacion
{
    public interface IValidarEvento
    {
        Resp ValidarEventoId(int uid);
        Resp ValidarEvento(Evento evento);
    }
}
