using Management_BE.Models.DatabaseModels;

namespace Management_BE.Models.DataModels
{
    public class AuthenticationRequest
    {
        public class RegistrationRequest
        {
            public string FirstName { get; set; } = string.Empty;
            public string LastName { get; set; } = string.Empty;
            public string Email { get; set; } = string.Empty;
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;

            public int RoleId { get; set; }
        }

        public class LoginRequest 
        {
            public string Username { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;
        }

        public class LoginData
        {
            public int Id { get; set; }
            public string Username { get; set; } = string.Empty;

            public int RoleId { get; set; }
            public Role? Role { get; set; }
        }
    }
}
