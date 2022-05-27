using Management_BE.Models.DatabaseModels;

namespace Management_BE.Interfaces.Authentication
{
    public interface IAuthenticationRepository
    {
        Task<User> GetByEmailAsync(string email);

        Task<User> GetByUsernameAsync(string username);

        Task<User> CreateAsync(User user);

        Task<User> GetIdByUsernameAsync(string username);

        Task<User> GetUsernameByIdAsync(int idUser);

        Task<Role> GetRoleByIdAsync(int idRole);

        Task<List<User>> GetUserWithRoleByIdAsync(int idUser);
    }
}
