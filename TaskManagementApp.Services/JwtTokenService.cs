using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using TaskManagementApp.Core.Entities;

namespace TaskManagementApp.Services
{
    public class JwtTokenService : ITokenService
    {
        private readonly byte[] _key;
        private readonly TimeSpan _lifetime;

        public JwtTokenService(IConfiguration config)
        {
            var keyStr = config["Jwt:Key"] ?? "SuperSecretKeyForDev!";
            _key = Encoding.UTF8.GetBytes(keyStr);
            var hours = config.GetValue<int?>("Jwt:LifetimeHours") ?? 24;
            _lifetime = TimeSpan.FromHours(hours);
        }

        public string CreateToken(AppUser user, IDictionary<string, string>? extraClaims = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.Username)
            };
            if (extraClaims != null)
            {
                foreach (var kv in extraClaims)
                    claims.Add(new Claim(kv.Key, kv.Value));
            }

            var creds = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.Add(_lifetime),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}