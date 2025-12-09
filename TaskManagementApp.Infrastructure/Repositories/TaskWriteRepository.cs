using System.Threading.Tasks;
using TaskManagementApp.Core.Entities;
using TaskManagementApp.Core.Interfaces;
using TaskManagementApp.Infrastructure.Data;

namespace TaskManagementApp.Infrastructure.Repositories
{
    public class TaskWriteRepository : ITaskWriteRepository
    {
        private readonly AppDbContext _db;

        public TaskWriteRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task<TaskItem> CreateAsync(TaskItem item)
        {
            _db.Tasks.Add(item);
            await _db.SaveChangesAsync();
            return item;
        }

        public async Task UpdateAsync(TaskItem item)
        {
            _db.Tasks.Update(item);
            await _db.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var task = await _db.Tasks.FindAsync(id);
            if (task == null)
                return;

            _db.Tasks.Remove(task);
            await _db.SaveChangesAsync();
        }
    }
}
