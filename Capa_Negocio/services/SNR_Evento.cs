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

        public async Task<Resp> INR_Editar_Evento(Evento evento)
        {
            var resp = await mEvento.BD_Editar_Evento(evento);
            if (resp) return new() { Ok = false, msg = "Por favor hable con el administrador, codigo de error: cod7" };

            return new() { Ok = true };
        }

        public async Task<Resp> INR_Eliminar_Evento(int Id_Even, int uid)
        {
            var resp = await mEvento.BD_Eliminar_Evento(Id_Even, uid);
            if (resp) return new() { Ok = false, msg = "Por favor hable con el administrador, codigo de error: cod8" };
            return new() { Ok = true };
        }

        public async Task<List<EventoPart>> INR_Mostar_Todos_Evento_Usuario(int uid)
        {
            DataTable eventos = await mEvento.DB_Mostar_Todos_Evento_Usuario(uid);

            var evaluar = await EvaluarLista(eventos);

            return evaluar;
        }
        public async Task<List<Guests>> INR_Mostar_Todos_Usuarios_Evento(int Id_Even)
        {
            DataTable guests = await mEvento.DB_Mostar_Todos_Usuarios_Evento(Id_Even);

            var evaluar = EvaluarListaGuest(guests);

            return evaluar;
        }
        public async Task<int> INR_Registrar_Evento(Evento evento)
        {
            return await mEvento.BD_Registrar_Evento(evento);
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
                UserUid = identity.Id_Usuario,
                IdCre = identity.Id_Usuario
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

            if (data.Rows.Count > 0)
            {
                var Usuario = data.Rows[0]["Usuario"].ToString();
                if (string.IsNullOrEmpty(Usuario)) return new();

                var title = data.Rows[0]["title"].ToString();
                if (string.IsNullOrEmpty(title)) return new();

                var notes = data.Rows[0]["notes"].ToString();
                if (string.IsNullOrEmpty(notes)) return new();

                var start = data.Rows[0]["start_note"].ToString();
                if (string.IsNullOrEmpty(start)) return new();

                var end = data.Rows[0]["end_note"].ToString();
                if (string.IsNullOrEmpty(end)) return new();

                Evento evento = new()
                {
                    Id = Convert.ToInt16(data.Rows[0]["Id_Not"]),
                    UserUid = Convert.ToInt16(data.Rows[0]["Id_Usu"]),
                    User = Usuario,
                    Title = title,
                    Notes = notes,
                    Start = start,
                    End = end,
                    IdCre = Convert.ToInt16(data.Rows[0]["Id_Cre"])
                };

                return evento;
            }

            return new();
        }

        public Evento RegisterData(Guests guests, EventoRegister Request, ClaimsIdent ident)
        {
            Evento evento = new()
            {
                Title = Request.Title,
                Notes = Request.Notes,
                Start = Request.Start,
                End = Request.End,
                User = ident.Names,
                UserUid = guests.id,
                IdCre = ident.Id_Usuario
            };

            return evento;
        }

        public async Task<Resp> INR_Eliminar_Evento_Usuario(int Id_Even)
        {
            var notUs = await mEvento.BD_Eliminar_Evento_Usuario(Id_Even);
            if (notUs) return new() { Ok = false, msg = "Por favor hable con el administrador, codigo de error: cod88" };

            return new() { Ok = true };
        }

        public async Task<int> INR_Registrar_Evento_Relacion(int Id_Even, int uid)
        {
            return await mEvento.BD_Registrar_Evento_Relacion(Id_Even, uid);
        }

        private async Task<List<EventoPart>> EvaluarLista(DataTable eventos)
        {
            List<EventoPart> list = new();

            try
            {
                if (eventos.Rows.Count > 0)
                {
                    foreach (DataRow row in eventos.Rows)
                    {
                        var user = row["Usuario"].ToString();
                        if (string.IsNullOrEmpty(user)) break;

                        var title = row["title"].ToString();
                        if (string.IsNullOrEmpty(title)) break;

                        var notes = row["notes"].ToString();
                        if (string.IsNullOrEmpty(notes)) break;

                        var start = row["start_note"].ToString();
                        if (string.IsNullOrEmpty(start)) break;

                        var end = row["end_note"].ToString();
                        if (string.IsNullOrEmpty(end)) break;

                        EventoPart env = new()
                        {
                            Id = Convert.ToInt16(row["Id_Not"].ToString()),
                            UserUid = Convert.ToInt16(row["Id_Usu"].ToString()),
                            User = user,
                            Title = title,
                            Notes = notes,
                            Start = start,
                            End = end,
                            IdCre = Convert.ToInt16(row["Id_Cre"].ToString())
                        };

                        var resp = await INR_Mostar_Todos_Usuarios_Evento(env.Id);
                        env.listGuests = resp;

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

        private static List<Guests> EvaluarListaGuest(DataTable eventos)
        {
            List<Guests> list = new();

            try
            {
                if (eventos.Rows.Count > 0)
                {
                    foreach (DataRow row in eventos.Rows)
                    {
                        var name = row["name"].ToString();
                        if (string.IsNullOrEmpty(name)) break;

                        var email = row["email"].ToString();
                        if (string.IsNullOrEmpty(email)) break;

                        Guests env = new()
                        {
                            id = Convert.ToInt16(row["Id_Usu"].ToString()),
                            Name = name,
                            Email = email,
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

        public async Task<Resp> INR_Eliminar_Evento_Usuario_Relacion(int Id_Even, int Id_User)
        {
            var resp = await mEvento.BD_Eliminar_Evento_Relacion(Id_Even, Id_User);
            if (resp) return new() { Ok = false, msg = "Por favor hable con el administrador, codigo de error: cod88" };

            return new() { Ok = true };
        }
    }
}
