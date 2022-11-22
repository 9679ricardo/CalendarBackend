using System.Data.SqlClient;
using System.Data;

namespace Capa_Datos
{
    public class SbdAction : IBdAction
    {
        private readonly string con = "Server=;Database=Notas;Trusted_Connection=True;MultipleActiveResultSets=True";
        public async Task<bool> BD_Eliminar_Notificacion(int uid)
        {
            bool delete = true;
            SqlConnection cn = new();

            try
            {
                cn.ConnectionString = con;

                SqlCommand cmd = new("Sp_Eliminar_Notificacion_All", cn)
                {
                    CommandTimeout = 20,
                    CommandType = CommandType.StoredProcedure
                };

                cmd.Parameters.AddWithValue("@idUsu", uid);

                await cn.OpenAsync();

                int getValue = Convert.ToInt32(await cmd.ExecuteNonQueryAsync());

                if (getValue > 0)
                {
                    delete = false;
                }
                else
                {
                    delete = true;
                }

                cmd.Parameters.Clear();
                cmd.Dispose();
                cn.Close();

                return delete;
            }
            catch (Exception)
            {
                if (cn.State == ConnectionState.Open)
                {
                    cn.Close();
                }

                return delete;
            }
        }
    }
}
