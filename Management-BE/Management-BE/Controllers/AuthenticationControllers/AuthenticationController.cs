using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Management_BE.Models.DatabaseModels;
using Management_BE.Models.DataModels;
using Management_BE.Interfaces.Authentication;
using Management_BE.Services;
using Management_BE.Interfaces.Hasher;

namespace Management_BE.Controllers.AuthenticationControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationRepository _iAuthenticationRepository;
        private readonly IPasswordHasher _iPasswordHasher;

        // Riferimento alle tabelle 
        //private readonly AuthenticationDataContext _authenticationDataContext;

        // Costruttore di inizializzazione
        //public AuthenticationController(AuthenticationDataContext authenticationContext)
        //{
        //    _authenticationDataContext = authenticationContext;
        //}
        public AuthenticationController(IAuthenticationRepository iAuthenticationRepository,
                                        IPasswordHasher iPasswordHasher)
        {
            _iAuthenticationRepository = iAuthenticationRepository;
            _iPasswordHasher = iPasswordHasher;
        }

        //// POST: api/Registration
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost("Registration")]
        //public async Task<ActionResult<User>> PostRegistration(User registerRequest)
        //{
        //    if (_authenticationDataContext.Users == null)
        //    {
        //        return Problem("Entity set 'DataContext.Users' is null.");
        //    }

        //    // Verifica email se presente nel DB

        //    // Verifica username se presente nel DB

        //    // Criptazione password

        //    // Inizializzo l'oggetto da inviare

        //    // Aggiungo i dati ricevuti nel data context
        //    _authenticationDataContext.Users.Add(registerRequest);
        //    // Eseguo il salvataggio dei cambiamenti in modo asyncrono (attendo l'elaborazione)
        //    await _authenticationDataContext.SaveChangesAsync();

        //    return CreatedAtAction("Registration successfully !", new { id = registerRequest.Id }, registerRequest);
        //}

        // POST: api/Registration
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Registration")]
        public async Task<IActionResult> PostRegistration(AuthenticationRequest.RegistrationRequest registerRequest)
        {
            User user = new();

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

            var valueQueryJoin = await _iAuthenticationRepository.GetUserWithRoleByIdAsync(valueInsertData.Id);

            // Prendo il valore di riferimento per il ruolo
            //Role roleValue = await _iAuthenticationRepository.GetRoleByIdAsync(valueInsertData.RoleId);

            //user.Role = roleValue;

            return Ok(valueQueryJoin);
        }

        // POST: api/Login
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Login")]
        public async Task<IActionResult> PostLogin(AuthenticationRequest.LoginRequest userRequest)
        {
            //AuthenticationRequest.LoginData userData = new();

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
            //userData.Id = userId.Id;
            //userData.Username = userUsername.Username;
            ////userData.RoleId = userUsername.RoleId;
            //userData.Role = roleValue;

            return Ok(valueQueryJoin);
        }

    }
}
