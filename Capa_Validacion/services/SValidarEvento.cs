using Capa_Entidad;

namespace Capa_Validacion
{
    public class SValidarEvento : IValidarEvento
    {
        public object ValidarEventoId(int uid)
        {
            if (uid <= 0) return new { ok = false, msg = "Id no válido" };

            return null;
        }

        public object ValidarEvento(Evento evento)
        {
            if (string.IsNullOrEmpty(evento.Title)) return new { ok = false, msg = "El título es obligatorio" };
            if (string.IsNullOrEmpty(evento.Start)) return new { ok = false, msg = "La fecha inicial es obligatoria" };
            if (string.IsNullOrEmpty(evento.End)) return new { ok = false, msg = "La fecha final es obligatoria" };
            return null;
        }
    }
}
