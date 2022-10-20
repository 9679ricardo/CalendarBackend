namespace Capa_Negocio
{
    public interface ICreateHash
    {
        string PasswordDecrypt(string password);
        string CreatePasswordEncrypt(string password);
    }
}
