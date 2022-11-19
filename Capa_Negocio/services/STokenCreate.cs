using Capa_Entidad;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Capa_Negocio
{
    public class STokenCreate : ITokenCreate
    {
        public ClaimsIdent GetUser(IEnumerable<Claim> identity)
        {
            if (identity != null)
            {

                var id = identity.Where(c => c.Type == ClaimTypes.NameIdentifier)
                   .Select(c => c.Value).SingleOrDefault();

                var name = identity.Where(c => c.Type == ClaimTypes.Surname)
                  .Select(c => c.Value).SingleOrDefault();

                if(id is null) return new ClaimsIdent();
                if (name is null) return new ClaimsIdent();

                ClaimsIdent user = new()
                {
                    Id_Usuario = int.Parse(id),
                    Names = name
                };

                return user;
            }

            return new ClaimsIdent();
        }

        public string TokenCreate(int id, string nombre)
        {
            try
            {
                if (string.IsNullOrEmpty(nombre)) return "";
                if(id == 0) return "";

                var keyBytes = Encoding.UTF8.GetBytes("12751421523ekmedjriccoutr8344ids38754d");
                var clains = new ClaimsIdentity();

                clains.AddClaim(new Claim(ClaimTypes.NameIdentifier, id.ToString()));
                clains.AddClaim(new Claim(ClaimTypes.Surname, nombre));

                var tokenDescr = new SecurityTokenDescriptor
                {
                    Subject = clains,
                    Expires = DateTime.UtcNow.AddMinutes(120),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes),
                    SecurityAlgorithms.HmacSha512Signature)
                };

                var tokenHan = new JwtSecurityTokenHandler();
                var tokenC = tokenHan.CreateToken(tokenDescr);
                string token = tokenHan.WriteToken(tokenC);
                return token;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public ClaimsIdent ValidarToken(string token, IEnumerable<Claim> identity)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.UTF8.GetBytes("12751421523ekmedjriccoutr8344ids38754d");

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                ClaimsIdent user = GetUser(identity);

                string newToken = TokenCreate(user.Id_Usuario, user.Names);
                user.Token = newToken;

                return user;
            }
            catch (Exception)
            {
                return new ClaimsIdent();
            }
        }
    }
}
