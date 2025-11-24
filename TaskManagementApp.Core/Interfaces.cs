using System.Collections.Generic;
using System.Threading.Tasks;
using TaskManagementApp.Core.Entities;

namespace TaskManagementApp.Core.Interfaces
{
    public interface ITaskReadRepository
    {
        Task<IEnumerable<TaskItem>> GetAllAsync(Status? status = null, Priority? priority = null);
        Task<TaskItem?> GetByIdAsync(int id);
    }

    public interface ITaskWriteRepository
    {
        Task<TaskItem> CreateAsync(TaskItem item);
        Task UpdateAsync(TaskItem item);
        Task DeleteAsync(int id);
    }

    public interface IUserRepository
    {
        Task<AppUser?> GetByUsernameAsync(string username);
        Task<AppUser> CreateAsync(AppUser user);
    }
}
