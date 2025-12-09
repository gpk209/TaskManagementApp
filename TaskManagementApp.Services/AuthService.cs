using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TaskManagementApp.Core.Entities;
using TaskManagementApp.Core.Interfaces;
using TaskManagementApp.Services.Exceptions;

namespace TaskManagementApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IPasswordHasher _hasher;
        private readonly ITokenService _tokens;
        private readonly ILogger<AuthService> _logger;

        public AuthService(
            IUserRepository users, 
            IPasswordHasher hasher, 
            ITokenService tokens,
            ILogger<AuthService> logger)
        {
            _users = users ?? throw new ArgumentNullException(nameof(users));
            _hasher = hasher ?? throw new ArgumentNullException(nameof(hasher));
            _tokens = tokens ?? throw new ArgumentNullException(nameof(tokens));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string?> LoginAsync(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username))
            {
                _logger.LogWarning("Login attempt with empty username");
                return null;
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                _logger.LogWarning("Login attempt with empty password for username: {Username}", username);
                return null;
            }

            _logger.LogInformation("Login attempt for user: {Username}", username);

            var user = await _users.GetByUsernameAsync(username);
            if (user == null)
            {
                _logger.LogWarning("Login failed - user not found: {Username}", username);
                return null;
            }

            if (!_hasher.Verify(password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed - invalid password for user: {Username}", username);
                return null;
            }

            _logger.LogInformation("Login successful for user: {Username}", username);
            return _tokens.CreateToken(user);
        }

        public async Task RegisterAsync(string username, string password)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException("Username cannot be empty", nameof(username));

            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Password cannot be empty", nameof(password));

            _logger.LogInformation("Registration attempt for username: {Username}", username);

            var existing = await _users.GetByUsernameAsync(username);
            if (existing != null)
            {
                _logger.LogWarning("Registration failed - username already exists: {Username}", username);
                throw new UsernameTakenException(username);
            }

            var hashed = _hasher.Hash(password);
            var user = new AppUser { Username = username, PasswordHash = hashed };
            
            await _users.CreateAsync(user);
            _logger.LogInformation("User registered successfully: {Username}", username);
        }
    }
}
