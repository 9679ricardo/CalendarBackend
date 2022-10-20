using Capa_Entidad;
using System.Data;
using System.Data.SqlClient;

namespace Capa_Datos
{
    public class SDB_Evento : IDB_Evento
    {
        public async Task<bool> BD_Editar_Evento(Evento evento)
        {
            SqlConnection cn = new();
            bool respuesta = false;

            try
            {
                cn.ConnectionString = "Server=DESKTOP-7SCJT85;Database=Notas;Trusted_Connection=True;MultipleActiveResultSets=True";
                SqlCommand cmd = new("Sp_Editar_Nota", cn)
                {
                    CommandTimeout = 20,
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@idNot", evento.Id);
                cmd.Parameters.AddWithValue("@idUsu", evento.UserUid);
                cmd.Parameters.AddWithValue("@usuario", evento.User);
                cmd.Parameters.AddWithValue("@title", evento.Title);
                cmd.Parameters.AddWithValue("@notes", evento.Notes);
                cmd.Parameters.AddWithValue("@start", evento.Start);
                cmd.Parameters.AddWithValue("@end", evento.End);

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

        public async Task<bool> BD_Eliminar_Evento(int Id_Even, int uid)
        {
            SqlConnection cn = new();
            bool respuesta = true;

            try
            {
                cn.ConnectionString = "Server=DESKTOP-7SCJT85;Database=Notas;Trusted_Connection=True;MultipleActiveResultSets=True";

                SqlCommand cmd = new("Sp_Eliminar_Nota", cn)
                {
                    CommandTimeout = 20,
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@idNot", Id_Even);
                cmd.Parameters.AddWithValue("@idUsu", uid);

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

        public async Task<DataTable> DB_Mostar_Todos_Evento_Usuario(int uid)
        {
            SqlConnection cn = new();
            DataTable data = new();

            try
            {
                cn.ConnectionString = "Server=DESKTOP-7SCJT85;Database=Notas;Trusted_Connection=True;MultipleActiveResultSets=True";
                SqlDataAdapter da = new("Sp_Listar_Notas_Usuario", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@idUsu", uid);

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

        public async Task<int> BD_Registrar_Evento(Evento evento)
        {
            SqlConnection cn = new();

            int respuesta;

            try
            {
                cn.ConnectionString = "Server=DESKTOP-7SCJT85;Database=Notas;Trusted_Connection=True;MultipleActiveResultSets=True";

                SqlCommand cmd = new("Sp_Add_Nota", cn)
                {
                    CommandTimeout = 20,
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@idUsu", evento.UserUid);
                cmd.Parameters.AddWithValue("@usuario", evento.User);
                cmd.Parameters.AddWithValue("@title", evento.Title);
                cmd.Parameters.AddWithValue("@notes", evento.Notes);
                cmd.Parameters.AddWithValue("@start", evento.Start);
                cmd.Parameters.AddWithValue("@end", evento.End);

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

                cn.Close();

                return respuesta;
            }
            catch (Exception)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }

                return 0;
            }
        }

        public async Task<DataTable> BD_Buscar_Evento(int Id_Not)
        {
            SqlConnection cn = new();
            DataTable data = new();

            try
            {
                cn.ConnectionString = "Server=DESKTOP-7SCJT85;Database=Notas;Trusted_Connection=True;MultipleActiveResultSets=True";
                SqlDataAdapter da = new("Sp_Buscar_Notas", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@Id_Not", Id_Not);

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
