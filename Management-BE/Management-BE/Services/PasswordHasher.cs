using Management_BE.Data.AuthenticationData;
using Management_BE.Interfaces.Hasher;
using System.Security.Cryptography;
using System.Text;
using Microsoft.EntityFrameworkCore;


namespace Management_BE.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        private readonly ApplicationDataContext _authenticationDataContext;

        public PasswordHasher(ApplicationDataContext authenticationDataContext)
        {
            _authenticationDataContext = authenticationDataContext;
        }

        public string EncryptUsersPassword(string password)
        {
            StringBuilder Sb = new StringBuilder();

            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(password));

                foreach (byte b in result)
                    // Conversion hex (Esadecimale x2)
                    Sb.Append(b.ToString("x2"));
            }

            return Sb.ToString();

            //using (SHA256Managed sha256 = new SHA256Managed { })
            //{
            //    // Codifico la password in byte
            //    byte[] passwordByte = Encoding.ASCII.GetBytes(password);
            //    // Trasformo la stringa in un altra cryptata
            //    byte[] encryptedPassword = sha256.ComputeHash(passwordByte);
            //    // Converto in string i byte trasformati
            //    string pwdEcryptedString = Convert.ToBase64String(encryptedPassword);

            //    return pwdEcryptedString;
            //}
        }

        public bool VerifyPasswordEncrypted(string password, string userUsername)
        {
            string passwordHash = EncryptUsersPassword(password);

            int countValue = _authenticationDataContext.User
                                    .Where(res => res.Username.Equals(userUsername) &&
                                            res.Password.Equals(passwordHash))
                                    .Count();

            return countValue == 1 ? true : false;
        }
    }
}
