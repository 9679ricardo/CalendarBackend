using Capa_Entidad;
using System.Data.SqlClient;
using System.Data;

namespace Capa_Datos
{
    public class SdbNotificacion : IDbNotificacion
    {
        private readonly string con = "Server=;Database=Notas;Trusted_Connection=True;MultipleActiveResultSets=True";
        public async Task<bool> BD_Eliminar_Notificacion(int Id_Not, int Id_Usu)
        {
            SqlConnection cn = new();
            bool respuesta = true;

            try
            {
                cn.ConnectionString = con;

                SqlCommand cmd = new("Sp_Eliminar_Notificacion", cn)
                {
                    CommandTimeout = 20,
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@idNoti", Id_Not);
                cmd.Parameters.AddWithValue("@idUsu", Id_Usu);

                await cn.OpenAsync();

                int getValue = Convert.ToInt32(await cmd.ExecuteNonQueryAsync());

                if (getValue > 0)
                {
                    respuesta = false;
                }
                else
                {
                    respuesta = true;
                }

                cmd.Parameters.Clear();
                cmd.Dispose();
                cn.Close();

                return respuesta;
            }
            catch (Exception)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }

                return respuesta;
            }
        }

        public async Task<int> BD_Registrar_Notificacion(NotificacionRegister notificacion)
        {
            SqlConnection cn = new();

            int respuesta;

            try
            {
                SqlCommand cmd = new();
                cn.ConnectionString = con;
                cmd.CommandText = "Sp_Add_Notificacion";
                cmd.Connection = cn;
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@idUsu", notificacion.Id_Usu);
                cmd.Parameters.AddWithValue("@idNot", notificacion.Id_Nota);
                cmd.Parameters.AddWithValue("@idCre", notificacion.Id_Cree);

                await cn.OpenAsync();

                int getValue = Convert.ToInt32(await cmd.ExecuteScalarAsync());

                if (getValue > 0)
                {
                    respuesta = getValue;
                }
                else
                {
                    respuesta = 0;
                }

                cmd.Parameters.Clear();
                cmd.Dispose();
                cn.Close();
            }
            catch (Exception)
            {

                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }

                return 0;
            }

            return respuesta;
        }

        public async Task<DataTable> DB_Mostar_Todas_Notificacion_Usuario(int Id_Usu)
        {
            SqlConnection cn = new();
            DataTable data = new();

            try
            {
                cn.ConnectionString = con;
                SqlDataAdapter da = new("Sp_Listar_Notificaciones", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@idUsu", Id_Usu);

                await cn.OpenAsync();

                da.Fill(data);
                cn.Close();

                return data;

            }
            catch (Exception)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }

                return data;
            }
        }
    }
}
