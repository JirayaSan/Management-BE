namespace Management_BE.Interfaces.Hasher
{
    public interface IPasswordHasher
    {
        string EncryptUsersPassword(string password);

        bool VerifyPasswordEncrypted(string password, string passwordHash);
    }
}
