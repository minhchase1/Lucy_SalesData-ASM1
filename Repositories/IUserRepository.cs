using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;

namespace Repositories
{
    public interface IUserRepository
    {
        User? GetUserByEmailAndPassword(string email, string password);
        bool AddUser(User user);
        bool UpdateUser(User user);
        bool DeleteUser(int userid);
        User? GetUserByEmail(string email);
        List<User> GetUserByKeyWord(string keyword);
        List<User> GetUsers();
    }
}
