using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApp.Core.Entities;
using TaskManagementApp.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using BCryptNet = BCrypt.Net.BCrypt;

namespace TaskManagementApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _users;
        private readonly IConfiguration _config;
        public AuthService(IUserRepository users, IConfiguration config) { _users = users; _config = config; }

        public async Task<string?> LoginAsync(string username, string password)
        {
            var user = await _users.GetByUsernameAsync(username);
            if (user == null) return null;
            if (!BCryptNet.Verify(password, user.PasswordHash)) return null;

            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"] ?? "SuperSecretKeyForDev!");
            var claims = new[] { new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()), new Claim(ClaimTypes.Name, user.Username) };
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddHours(24), signingCredentials: new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256));
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task RegisterAsync(string username, string password)
        {
            var existing = await _users.GetByUsernameAsync(username);
            if (existing != null) throw new Exception("Username taken");

            var hashed = BCryptNet.HashPassword(password);
            var user = new AppUser { Username = username, PasswordHash = hashed };
            await _users.CreateAsync(user);
        }
    }

    public class TaskService : ITaskService
    {
        private readonly ITaskReadRepository _read;
        private readonly ITaskWriteRepository _write;
        public TaskService(ITaskReadRepository read, ITaskWriteRepository write) { _read = read; _write = write; }

        public async Task<IEnumerable<TaskItem>> ListAsync(Status? status = null, Priority? priority = null)
            => await _read.GetAllAsync(status, priority);

        public async Task<TaskItem?> GetAsync(int id) => await _read.GetByIdAsync(id);

        public async Task<TaskItem> CreateAsync(TaskItem dto) => await _write.CreateAsync(dto);

        public async Task UpdateAsync(int id, TaskItem dto)
        {
            var existing = await _read.GetByIdAsync(id);
            if (existing == null) throw new Exception("Not found");
            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.DueDate = dto.DueDate;
            existing.Priority = dto.Priority;
            existing.Status = dto.Status;
            await _write.UpdateAsync(existing);
        }

        public async Task DeleteAsync(int id) => await _write.DeleteAsync(id);
    }
}
