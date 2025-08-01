using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public interface IUserService
    {
         User? Login(string email, string password);
        bool Register(User user);
        List<User> GetAllUsers();   
        bool UpdateUser(User user);
        bool DeleteUser(int userId);
        bool AddUser(User user);
        List<User> GetUserByKeyWord(string keyword);
    }
}
