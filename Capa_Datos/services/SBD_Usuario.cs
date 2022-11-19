using Capa_Entidad;
using System.Data;
using System.Data.SqlClient;

namespace Capa_Datos
{
    public class SBD_Usuario : IBD_Usuario
    {
        private readonly string con = "Server=;Database=Notas;Trusted_Connection=True;MultipleActiveResultSets=True";

        public async Task<bool> BD_Buscar_Correo(string email)
        {
            SqlConnection cn = new();

            bool respuesta = false;

            try
            {

                SqlCommand cmd = new();
                cn.ConnectionString = con;
                cmd.CommandText = "Sp_Validar_Correo";
                cmd.Connection = cn;
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@email", email);

                await cn.OpenAsync();

                int getValue = Convert.ToInt32(cmd.ExecuteScalar());

                if (getValue > 0)
                {
                    respuesta = true;
                }
                else
                {
                    respuesta = false;
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

        public async Task<int> BD_Registrar_Usuario(UsuarioRegister usuario)
        {
            SqlConnection cn = new();

            int respuesta;

            try
            {
                SqlCommand cmd = new();
                cn.ConnectionString = con;
                cmd.CommandText = "Sp_Add_Usuario";
                cmd.Connection = cn;
                cmd.CommandTimeout = 20;
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@name", usuario.Name.Trim());
                cmd.Parameters.AddWithValue("@password", usuario.Password.Trim());
                cmd.Parameters.AddWithValue("@email", usuario.Email.Trim());

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

        public async Task<DataTable> DB_Login(string email)
        {
            SqlConnection cn = new();
            DataTable data = new();

            try
            {
                cn.ConnectionString = con;
                SqlDataAdapter da = new("Sp_Usuario_Login", cn);
                da.SelectCommand.CommandType = CommandType.StoredProcedure;
                da.SelectCommand.Parameters.AddWithValue("@Email", email);

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
