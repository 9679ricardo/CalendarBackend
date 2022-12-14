using Capa_Datos;
using Capa_Entidad;
using System.Data;

namespace Capa_Negocio
{
    public class SnrNotificacion : INrNotificacion
    {
        private readonly IDbNotificacion mNotificacion;

        public SnrNotificacion(IDbNotificacion mNotificacion)
        {
            this.mNotificacion = mNotificacion;
        }

        public async Task<List<Notificacion>> INR_Mostar_Todos_Notificacion_Usuario(int Id_Usu)
        {
            var resp = await mNotificacion.DB_Mostar_Todas_Notificacion_Usuario(Id_Usu);

            return EvaluarLista(resp);
        }
         
        public async Task<bool> NR_Eliminar_Notificacion(int Id_Not, int Id_Usu)
        {
            return await mNotificacion.BD_Eliminar_Notificacion(Id_Not, Id_Usu);
        }

        public async Task<int> NR_Registrar_Notificacion(int id_Not, List<Guests> ListGue, int id_Usu)
        {
            foreach(Guests guests in ListGue)
            {
                var notification = RegistarData(id_Not, guests, id_Usu);
                await mNotificacion.BD_Registrar_Notificacion(notification);
            }

            return 1;
        }

        private static List<Notificacion> EvaluarLista(DataTable eventos)
        {
            List<Notificacion> list = new();

            try
            {
                if (eventos.Rows.Count > 0)
                {
                    foreach (DataRow row in eventos.Rows)
                    {
                        var email = row["email"].ToString();
                        if (string.IsNullOrEmpty(email)) break;

                        var name = row["name"].ToString();
                        if (string.IsNullOrEmpty(name)) break;

                        Notificacion not = new()
                        {
                            Id_Not = Convert.ToInt16(row["Id_Noti"].ToString()),
                            Email = email,
                            Id_Nota = Convert.ToInt16(row["Id_Nota"].ToString()),
                            Id_Usu = Convert.ToInt16(row["Id_Usu"].ToString()),
                            Name = name
                        };

                        list.Add(not);
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
        
        private static NotificacionRegister RegistarData(int Id_Not, Guests ListGue, int id_Usu)
        {
            NotificacionRegister notification = new()
            {
               Id_Nota = Id_Not,
               Id_Usu = ListGue.id,
               Id_Cree = id_Usu
            };

            return notification;
        }
    }
}
