using TaskManagementApp.Core.Entities;
using System.Collections.Generic;

namespace TaskManagementApp.Services
{
    public interface ITokenService
    {
        string CreateToken(AppUser user, IDictionary<string, string>? extraClaims = null);
    }
}