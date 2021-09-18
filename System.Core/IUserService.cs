using System.Core.DTO;
using System.DB;
using System.Threading.Tasks;

namespace System.Core
{
    public interface IUserService
    {
        Task<AuthenticatedUser> SignUp(User user);
        Task<AuthenticatedUser> SignIn(User user);
        Task<AuthenticatedUser> ExternalSignIn(User user);
    }
}
