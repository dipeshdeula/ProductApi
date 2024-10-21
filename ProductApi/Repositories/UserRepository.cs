using Microsoft.EntityFrameworkCore;
using ProductApi.Data;
using ProductApi.Models;

namespace ProductApi.Repositories
{
    public class UserRepository : IUserRepository
    {/*
        private List<User> _users = new List<User>
        { 
          new User{Id = 1, Username = "admin", Password="password",Role="Admin" },
          new User{Id = 2, Username = "user", Password="password",Role="User" }
            
        };*/

        //Get product data list from the database
        private readonly ProductDbContext _context;

        //for database
        public UserRepository(ProductDbContext context)
        {
            _context = context;

        }
       

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            
          return await _context.Users.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);

        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
