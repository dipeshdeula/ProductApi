using ProductApi.Models;

namespace ProductApi.Services
{
    public interface IUserService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<User> GetByIdAsync(int id);

    }
}
