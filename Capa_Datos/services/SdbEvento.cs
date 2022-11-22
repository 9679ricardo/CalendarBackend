using Capa_Entidad;
using System.Data;
using System.Data.SqlClient;

namespace Capa_Datos
{
    public class SdbEvento : IDbEvento
    {
        private readonly string con = "Server=;Database=Notas;Trusted_Connection=True;MultipleActiveResultSets=True";
        public async Task<bool> BD_Editar_Evento(Evento evento)
        {
            bool respuesta = false;
            SqlConnection cn = new();
            try
            {
               
                cn.ConnectionString = con;
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
            bool respuesta = true;
            SqlConnection cn = new();
            try
            {
                
                cn.ConnectionString = con;

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

        public async Task<bool> BD_Eliminar_Evento_Usuario(int Id_Not)
        {
            bool respuesta = true;
            SqlConnection cn = new();
            try
            {
               
                cn.ConnectionString = con;

                SqlCommand cmd = new("Sp_Eliminar_Nota_Usuario", cn)
                {
                    CommandTimeout = 20,
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@idNota", Id_Not);

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
            DataTable data = new();
            SqlConnection cn = new();
            try
            {
                cn.ConnectionString = con;
                SqlDataAdapter da = new("Sp_Notas_Usuario", cn);
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
            int respuesta;
            SqlConnection cn = new();
            try
            {
                cn.ConnectionString = con;

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
                cmd.Parameters.AddWithValue("@idCre", evento.IdCre);
                
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
            DataTable data = new();
            SqlConnection cn = new();
            try
            {
                cn.ConnectionString = con;
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

        public async Task<int> BD_Registrar_Evento_Relacion(int Id_Even, int uid)
        {
            int respuesta;
            SqlConnection cn = new();

            try
            {
                cn.ConnectionString = con;

                SqlCommand cmd = new("Sp_Add_Nota_Usuario", cn)
                {
                    CommandTimeout = 20,
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@idUsu", uid);
                cmd.Parameters.AddWithValue("@idNot", Id_Even);

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

        public async Task<DataTable> DB_Mostar_Todos_Usuarios_Evento(int Id_Even)
        {
            DataTable data = new();
            SqlConnection cn = new();

            try
            {
                cn.ConnectionString = con;
                SqlDataAdapter da = new("Sp_Usuarios_Nota", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@idNot", Id_Even);

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

        public async Task<bool> BD_Eliminar_Evento_Relacion(int Id_Even, int uid)
        {
            bool respuesta = true;
            SqlConnection cn = new();

            try
            {
                cn.ConnectionString = con;

                SqlCommand cmd = new("Sp_Eliminar_Nota_Usuario_Uno", cn)
                {
                    CommandTimeout = 20,
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@idNota", Id_Even);
                cmd.Parameters.AddWithValue("@idUser", uid);

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
    }
}
