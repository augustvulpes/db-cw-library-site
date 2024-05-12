using LibraryApp.Models;

namespace LibraryApp.Interfaces.ServiceInterfaces
{
    public interface ITokenService
    {
        string CreateToken(User user);
    }
}
