using System.Threading.Tasks;
using TaskManagementApp.Core.Entities;

namespace TaskManagementApp.Core.Interfaces
{
    public interface IUserRepository
    {
        Task<AppUser?> GetByUsernameAsync(string username);
        Task<AppUser> CreateAsync(AppUser user);
    }
}
