using Capa_Datos;
using Capa_Entidad;
using System.Data;

namespace Capa_Negocio
{
    public class SNR_Evento : INR_Evento
    {
        private readonly IDB_Evento mEvento;

        public SNR_Evento(IDB_Evento mEvento)
        {
            this.mEvento = mEvento;
        }

        public async Task<object> INR_Editar_Evento(Evento evento)
        {
            var resp = await mEvento.BD_Editar_Evento(evento);
            if (resp) return new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" };

            return null;
        }

        public async Task<object> INR_Eliminar_Evento(int Id_Even, int uid)
        {
            var resp = await mEvento.BD_Eliminar_Evento(Id_Even, uid);

            if (resp) return new { ok = false, msg = "Por favor hable con el administrador, codigo de error: cod8" };

            return null;
        }

        public async Task<List<Evento>> INR_Mostar_Todos_Evento_Usuario(int uid)
        {
            DataTable eventos = await mEvento.DB_Mostar_Todos_Evento_Usuario(uid);

            var evaluar = EvaluarLista(eventos);

            return evaluar;
        }

        public async Task<int> INR_Registrar_Evento(Evento evento)
        {
            return await mEvento.BD_Registrar_Evento(evento);
        }
            
        private static List<Evento> EvaluarLista(DataTable eventos)
        {
            List<Evento> list = new();

            try
            {
                if (eventos.Rows.Count > 0)
                {
                    foreach (DataRow row in eventos.Rows)
                    {
                        Evento env = new()
                        {
                            Id = Convert.ToInt16(row["Id_Not"].ToString()),
                            UserUid = Convert.ToInt16(row["Id_Usu"].ToString()),
                            User = row["Usuario"].ToString(),
                            Title = row["title"].ToString(),
                            Notes = row["notes"].ToString(),
                            Start = row["start_note"].ToString(),
                            End = row["end_note"].ToString()
                        };

                        list.Add(env);
                    }
                    return list;
                }

                return list;
            }
            catch (Exception)
            {
                return list;
            }
        }

        public Evento RegisterData(ClaimsIdent identity, EventoRegister Request)
        {
            Evento evento = new()
            {
                Title = Request.Title,
                Notes = Request.Notes,
                Start = Request.Start,
                End = Request.End,
                User = identity.Names,
                UserUid = identity.Id_Usuario
            };

            return evento;
        }

        public Evento UpdateData(ClaimsIdent identity, EventoUpdate Request)
        {
            Evento evento = new()
            {
                Id = Request.Id,
                Title = Request.Title,
                Notes = Request.Notes,
                Start = Request.Start,
                End = Request.End,
                User = identity.Names,
                UserUid = identity.Id_Usuario
            };

            return evento;
        }

        public async Task<Evento> INR_Buscar_Evento(int Id_Even)
        {
            DataTable data = await mEvento.BD_Buscar_Evento(Id_Even);
            Evento evento = new();
            
            if (data.Rows.Count > 0)
            {
                evento.Id = Convert.ToInt16(data.Rows[0]["Id_Not"]);
                evento.UserUid = Convert.ToInt16(data.Rows[0]["Id_Usu"]);

                return evento;
            }

            return evento;
        }
    }
}
