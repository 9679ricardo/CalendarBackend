using Capa_Entidad;
using System.Security.Claims;

namespace Capa_Negocio
{
    public interface ITokenCreate
    {
        ClaimsIdent GetUser(IEnumerable<Claim> identity);
        string TokenCreate(int id, string nombre);
        ClaimsIdent ValidarToken(string token, IEnumerable<Claim> identity);
    }
}
