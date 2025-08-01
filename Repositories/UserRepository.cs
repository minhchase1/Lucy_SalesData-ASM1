using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessObjects;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly SportsBookingDbContext _context;

        public UserRepository(SportsBookingDbContext context)
        {
            _context = context;
        }
        public bool AddUser(User user)
        {
            _context.Users.Add(user);
            return  _context.SaveChanges()>0;
        }
        public bool DeleteUser(int userid)
        {
            var user = _context.Users.Find(userid);
            if (user == null) return false;
            user.Status = "Inactive"; 
            return _context.SaveChanges() > 0;
        }
        public bool UpdateUser(User user)
        {
            _context.Users.Update(user);
            return _context.SaveChanges() > 0;
        }
        public User? GetUserByEmail(string email)
        {
           return  _context.Users.
                FirstOrDefault(u => u.Email == email);
        }

        public User? GetUserByEmailAndPassword(string email, string password)
        {
            return _context.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == email 
                && u.PasswordHash == password);
        }

        public List<User> GetUsers()
        {
            return _context.Users
                .Include(u => u.Role)
                .ToList();
        }

        public List<User> GetUserByKeyWord(string keyword)
        {
            return _context.Users
                .Include(u => u.Role)
                .Where(u => u.FullName.Contains(keyword))
                .ToList(); 
        }
    }
}
