using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using Repositories;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public bool AddUser(User user)
        {
            return _userRepository.AddUser(user);
        }

        public bool DeleteUser(int userId)
        {
            return _userRepository.DeleteUser(userId);
        }

        public List<User> GetAllUsers()
        {
            return _userRepository.GetUsers();
        }

        public List<User> GetUserByKeyWord(string keyword)
        {
            return _userRepository.GetUserByKeyWord(keyword);
        }

        public User? Login(string email, string password)
        {
            var user = _userRepository.GetUserByEmailAndPassword(email, password);
            return user;
        }

        public bool Register(User user)
        {
            var existed = _userRepository.GetUserByEmail(user.Email);
            if (existed != null) return false;

            _userRepository.AddUser(user);
            return true;
        }

        public bool UpdateUser(User user)
        {
            return _userRepository.UpdateUser(user);
        }
    }
}
