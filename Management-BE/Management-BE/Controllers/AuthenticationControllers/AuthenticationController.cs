using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Management_BE.Models.DatabaseModels;
using Management_BE.Models.DataModels;
using Management_BE.Interfaces.Authentication;
using Management_BE.Services;
using Management_BE.Interfaces.Hasher;
using static Management_BE.Models.DataModels.AuthenticationRequest;

namespace Management_BE.Controllers.AuthenticationControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _iAuthenticationRepository;
        private readonly IPasswordHasher _iPasswordHasher;

        public AuthenticationController(IAuthenticationRepository iAuthenticationRepository,
                                        IPasswordHasher iPasswordHasher)
        {
            _iAuthenticationRepository = iAuthenticationRepository;
            _iPasswordHasher = iPasswordHasher;
        }

        // POST: api/Registration
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Registration")]
        public async Task<IActionResult> PostRegistration(AuthenticationRequest.RegistrationRequest registerRequest)
        {
            User user = new();

            LoginData loginDatavalue = new();

            if (registerRequest == null)
            {
                return BadRequest(new ErrorResponse("Register data not contains values"));
            }

            // Verifica email se presente nel DB
            User existingUserEmail = await _iAuthenticationRepository.GetByEmailAsync(registerRequest.Email);
            if (existingUserEmail != null)
            {
                return Conflict(new ErrorResponse("Email already exist."));
            }

            // Verifica username se presente nel DB
            User existingUserUsername = await _iAuthenticationRepository.GetByUsernameAsync(registerRequest.Username);
            if (existingUserUsername != null)
            {
                return Conflict(new ErrorResponse("Username already exist."));
            }

            // Criptazione password
            string passwordHash = _iPasswordHasher.EncryptUsersPassword(registerRequest.Password);

            // Inizializzo l'oggetto da inviare
            user.FirstName = registerRequest.FirstName;
            user.LastName = registerRequest.LastName;
            user.Email = registerRequest.Email;
            user.Username = registerRequest.Username;
            user.Password = passwordHash;
            user.Date = DateTime.Now;
            user.RoleId = registerRequest.RoleId;

            // Eseguo l'inserimento dei dati
            User valueInsertData = await _iAuthenticationRepository.CreateAsync(user);

            // Prendo tutte le informazioni dell'utente, compreso le informazioni del ruolo
            var valueQueryJoin = await _iAuthenticationRepository.GetUserWithRoleByIdAsync(valueInsertData.Id);

            // Inizializzo i dati da inviare al client
            loginDatavalue.Id = valueQueryJoin.FirstOrDefault().Id;
            loginDatavalue.Username = valueQueryJoin.FirstOrDefault().Username;
            loginDatavalue.RoleId = valueQueryJoin.FirstOrDefault().RoleId;
            loginDatavalue.nameRole = valueQueryJoin.FirstOrDefault().Role.Name;

            return Ok(loginDatavalue);
        }

        // POST: api/Login
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Login")]
        public async Task<IActionResult> PostLogin(AuthenticationRequest.LoginRequest userRequest)
        {
            //AuthenticationRequest.LoginData userData = new();
            LoginData loginDatavalue = new();

            if (userRequest == null)
            {
                return BadRequest(new ErrorResponse("Login request not contains values"));
            }

            // Verifico se l'utente è presente
            User userUsername = await _iAuthenticationRepository.GetByUsernameAsync(userRequest.Username);
            if (userUsername == null)
            {
                return Unauthorized();
            }

            // Verifica della password criptata
            bool isCorrectPassword = _iPasswordHasher.VerifyPasswordEncrypted(userRequest.Password, userRequest.Username);
            if (!isCorrectPassword)
            {
                return Unauthorized();
            }

            // Acquisisco l'id dell'utente
            User userId = await _iAuthenticationRepository.GetIdByUsernameAsync(userRequest.Username);

            // Acquisco la definizione del ruolo dell'utente
            //Role roleValue = await _iAuthenticationRepository.GetRoleByIdAsync(userUsername.RoleId);
            var valueQueryJoin = await _iAuthenticationRepository.GetUserWithRoleByIdAsync(userId.Id);

            // Inizializzo l'oggetto da inviare
            loginDatavalue.Id = valueQueryJoin.FirstOrDefault().Id;
            loginDatavalue.Username = valueQueryJoin.FirstOrDefault().Username;
            loginDatavalue.RoleId = valueQueryJoin.FirstOrDefault().RoleId;
            loginDatavalue.nameRole = valueQueryJoin.FirstOrDefault().Role.Name;

            return Ok(loginDatavalue);
        }

    }
}
