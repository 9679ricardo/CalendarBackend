using System.Security.Cryptography;
using System.Text;

namespace Capa_Negocio
{
    public class SCreateHash : ICreateHash
    {
        public string CreatePasswordEncrypt(string password)
        {
            byte[] iv = new byte[16];
            byte[] array;

            try
            {
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes("mysmallkey12345512987651");
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using MemoryStream ms = new();
                    using CryptoStream cryptoStream = new(ms, encryptor, CryptoStreamMode.Write);
                    using (StreamWriter streamWriter = new(cryptoStream))
                    {
                        streamWriter.Write(password);
                    }

                    array = ms.ToArray();
                }

                return Convert.ToBase64String(array);
            }
            catch (Exception)
            {
                return "";
            }
        }

        public string PasswordDecrypt(string password)
        {
            byte[] iv = new byte[16];

            try
            {
                byte[] buffer = Convert.FromBase64String(password);
                using Aes aes = Aes.Create();
                aes.Key = Encoding.UTF8.GetBytes("mysmallkey12345512987651");
                aes.IV = iv;

                ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using MemoryStream ms = new(buffer);
                using CryptoStream cryptoStream = new(ms, decryptor, CryptoStreamMode.Read);
                using StreamReader sr = new(cryptoStream);

                return sr.ReadToEnd();
            }
            catch (Exception)
            {
                return "";
            }
        }
    }
}
