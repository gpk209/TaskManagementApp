using System.Threading.Tasks;

namespace TaskManagementApp.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(string username, string password);
        Task RegisterAsync(string username, string password);
    }
}
