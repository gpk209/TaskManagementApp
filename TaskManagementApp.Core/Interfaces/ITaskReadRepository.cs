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
}
