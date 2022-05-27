using Management_BE.Data.AuthenticationData;
using Management_BE.Interfaces.Authentication;
using Management_BE.Models.DatabaseModels;
using Management_BE.Models.DataModels;
using Microsoft.EntityFrameworkCore;

namespace Management_BE.Repositories.Authentication
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ApplicationDataContext _applicationDataContext;

        public AuthenticationRepository(ApplicationDataContext applicationDataContext)
        {
            _applicationDataContext = applicationDataContext;
        }

        public async Task<User> CreateAsync(User registerRequest)
        {
            // Aggiungo i dati ricevuti nel data context
            _applicationDataContext.User.Add(registerRequest);
            // Eseguo il salvataggio dei cambiamenti in modo asyncrono (attendo l'elaborazione)
            await _applicationDataContext.SaveChangesAsync();

            // Restituisco i valori inseriti
            return registerRequest;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            User userData = new();

            userData = await _applicationDataContext.User.Where(e => e.Email.Equals(email))
                                                                .FirstOrDefaultAsync();

            return userData;
                                            
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            User userData = new();

            userData = await _applicationDataContext.User.Where(u => u.Username.Equals(username))
                                                                .FirstOrDefaultAsync();

            return userData;
        }

        public async Task<User> GetIdByUsernameAsync(string username)
        {
            User userData = new();

            userData = await (from u in _applicationDataContext.User
                                where u.Username.Equals(username)
                                select new User()
                                {
                                    Id = u.Id,
                                })
                            .FirstOrDefaultAsync();

            return userData;
        }

        public async Task<User> GetUsernameByIdAsync(int idUser)
        {
            User userData = new();

            userData = await (from u in _applicationDataContext.User
                                where u.Id == idUser
                                select new User()
                                {
                                    Username = u.Username,
                                })
                            .FirstOrDefaultAsync();

            return userData;
        }

        public async Task<Role> GetRoleByIdAsync(int idRole)
        {
            Role roleValue = new();

            //roleValue = await _authenticationDataContext.Roles.Where(r => r.Id == idRole)
            //                                                        .FirstOrDefaultAsync();

            //return await (from user in _authenticationDataContext.Users
            //              join role in _authenticationDataContext.Roles
            //              on user.RoleId equals idRole
            //              select new { role, user })
            //                        .FirstOrDefaultAsync();

            roleValue = await (from user in _applicationDataContext.User
                            join role in _applicationDataContext.Role
                            on user.RoleId equals idRole
                            select new Role()
                            { 
                                Id = role.Id,
                                Name = role.Name,
                                Description = role.Description
                            })
                            .FirstOrDefaultAsync();

            return roleValue;
        }

        public async Task<List<User>> GetUserWithRoleByIdAsync(int idUser)
        {
            List<User> userData = await _applicationDataContext.User
                                .Where(u => u.Id == idUser)
                                .Include(r => r.Role)
                                .ToListAsync();
            return userData;
        }
    }

}
