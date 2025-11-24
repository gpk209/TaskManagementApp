using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApp.Core.Entities;

namespace TaskManagementApp.Services
{
    public interface IAuthService
    {
        Task<string?> LoginAsync(string username, string password);
        Task RegisterAsync(string username, string password);
    }

    public interface ITaskService
    {
        Task<IEnumerable<TaskItem>> ListAsync(Status? status = null, Priority? priority = null);
        Task<TaskItem?> GetAsync(int id);
        Task<TaskItem> CreateAsync(TaskItem dto);
        Task UpdateAsync(int id, TaskItem dto);
        Task DeleteAsync(int id);
    }
}
