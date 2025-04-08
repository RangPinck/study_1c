using Study1CApi.Models;

namespace Study1CApi.Interfaces
{
    public interface ITokenService
    {
        public string CreateToken(AuthUser user, string role);
    }
}
