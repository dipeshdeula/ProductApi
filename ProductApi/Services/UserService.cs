using ProductApi.Models;
using ProductApi.Repositories;

namespace ProductApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepostiory;
        public UserService(IUserRepository userRepository)
        {
            _userRepostiory = userRepository;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            return await _userRepostiory.AuthenticateAsync(username, password);
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _userRepostiory.GetByIdAsync(id);
        }
    }
}
