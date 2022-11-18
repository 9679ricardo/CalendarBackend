using Capa_Datos;
using Capa_Entidad;
using System.Data;

namespace Capa_Negocio
{
    public class SNR_Usuario : INR_Usuario
    {
        private readonly IBD_Usuario mUsuario;

        public SNR_Usuario(IBD_Usuario mUsuario)
        {
            this.mUsuario = mUsuario;
        }

        public async Task<object> NR_Buscar_CorreoS(string email)
        {
            var resp = await mUsuario.BD_Buscar_Correo(email); //debe retornar falso

            if (resp) return new { ok = false, msg = "El correo ya esta registrado" };

            return null;
        }

        public async Task<Usuario> NR_Login(string email)
        {
            DataTable data = await mUsuario.DB_Login(email);

            if (data.Rows.Count > 0)
            {
                Usuario usuario = new()
                {
                    uid = Convert.ToInt16(data.Rows[0]["Id_Usu"]),
                    Name = data.Rows[0]["name"].ToString(),
                    Password = data.Rows[0]["password"].ToString(),
                    Email = email
                };

                return usuario;
            }

            return new Usuario();
        }

        public async Task<int> NR_Registrar_Usuario(UsuarioRegister usuario)
        {
            return await mUsuario.BD_Registrar_Usuario(usuario); // tiene que devolver mayor que cero;
        }
    }
}
