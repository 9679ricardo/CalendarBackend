using Capa_Datos;
using Capa_Entidad;
using System.Data;

namespace Capa_Negocio
{
    public class SnrUsuario : INrUsuario
    {
        private readonly IBdUsuario mUsuario;

        public SnrUsuario(IBdUsuario mUsuario)
        {
            this.mUsuario = mUsuario;
        }

        public async Task<Resp> NR_Buscar_CorreoS(string email)
        {
            var resp = await mUsuario.BD_Buscar_Correo(email);

            if (resp) return new() { Ok = false, msg = "El correo ya esta registrado" };

            return new Resp() { Ok = true };
        }

        public async Task<Usuario> NR_Login(string email)
        {
            DataTable data = await mUsuario.DB_Login(email);

            if (data.Rows.Count > 0)
            {
                var name = data.Rows[0]["name"].ToString();
                if (string.IsNullOrEmpty(name)) return new();

                var password = data.Rows[0]["password"].ToString();
                if (string.IsNullOrEmpty(password)) return new();

                Usuario usuario = new()
                {
                    uid = Convert.ToInt16(data.Rows[0]["Id_Usu"]),
                    Name = name,
                    Password = password,
                    Email = email
                };

                return usuario;
            }

            return new();
        }

        public async Task<int> NR_Registrar_Usuario(UsuarioRegister usuario)
        {
            return await mUsuario.BD_Registrar_Usuario(usuario);
        }
    }
}
