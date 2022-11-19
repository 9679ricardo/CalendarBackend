using Capa_Entidad;

namespace Capa_Validacion
{
    public class SValidarEvento : IValidarEvento
    {
        public Resp ValidarEventoId(int uid)
        {
            if (uid <= 0) return new() { Ok = false, msg = "Id no válido" };

            return new() { Ok = true };
        }

        public Resp ValidarEvento(Evento evento)
        {
            if (string.IsNullOrEmpty(evento.Title)) return new() { Ok = false, msg = "El título es obligatorio" };
            if (string.IsNullOrEmpty(evento.Start)) return new() { Ok = false, msg = "La fecha inicial es obligatoria" };
            if (string.IsNullOrEmpty(evento.End)) return new() { Ok = false, msg = "La fecha final es obligatoria" };
            
            return new() { Ok = true };
        }
    }
}
