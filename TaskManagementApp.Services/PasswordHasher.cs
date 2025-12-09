using BCryptNet = BCrypt.Net.BCrypt;

namespace TaskManagementApp.Services
{
    public class BcryptPasswordHasher : IPasswordHasher
    {
        public string Hash(string password) => BCryptNet.HashPassword(password);
        public bool Verify(string password, string passwordHash) => BCryptNet.Verify(password, passwordHash);
    }
}