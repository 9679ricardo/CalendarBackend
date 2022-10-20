using Capa_Entidad;

namespace Capa_Validacion
{
    public interface IValidarEvento
    {
        object ValidarEventoId(int uid);
        object ValidarEvento(Evento evento);
    }
}
