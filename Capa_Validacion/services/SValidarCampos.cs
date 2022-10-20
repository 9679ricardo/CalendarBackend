using System.Text.RegularExpressions;

namespace Capa_Validacion
{
    public class SValidarCampos : IValidarCampos
    {
        public bool ValidarEmail(string correo)
        {
            string expresion;

            expresion = "\\w+([-+.']\\w+)*@\\w+([-.])*\\.\\w+([-.]\\w+)*";
            if (Regex.IsMatch(correo, expresion))
            {
                if (Regex.Replace(correo, expresion, string.Empty).Length == 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                return true;
            }
        }
    }
}
