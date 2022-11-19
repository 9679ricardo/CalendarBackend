namespace Capa_Entidad
{
    public class UsuarioLogin
    {
        private string _email = "";
        private string _password = "";
        public string Email { get => _email; set => _email = value; }
        public string Password { get => _password; set => _password = value; }
    }
}
