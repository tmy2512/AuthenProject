using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.UserRepository
{
    public interface IUserRepository
    {
        Task RegisterUser(User user);
        Task<LoginResult> ValidateUser(string username, string password);
    }
}
