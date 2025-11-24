using Microsoft.EntityFrameworkCore;
using TaskManagementApp.Core.Entities;
using TaskManagementApp.Core.Interfaces;
using TaskManagementApp.Infrastructure.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace TaskManagementApp.Infrastructure.Repositories
{
    public class TaskReadRepository : ITaskReadRepository
    {
        private readonly AppDbContext _db;
        public TaskReadRepository(AppDbContext db) { _db = db; }

        public async Task<IEnumerable<TaskItem>> GetAllAsync(Status? status = null, Priority? priority = null)
        {
            var q = _db.Tasks.AsQueryable();
            if (status.HasValue) q = q.Where(t => t.Status == status.Value);
            if (priority.HasValue) q = q.Where(t => t.Priority == priority.Value);
            return await q.OrderBy(t => t.DueDate).ToListAsync();
        }

        public async Task<TaskItem?> GetByIdAsync(int id) => await _db.Tasks.FindAsync(id);
    }

    public class TaskWriteRepository : ITaskWriteRepository
    {
        private readonly AppDbContext _db;
        public TaskWriteRepository(AppDbContext db) { _db = db; }

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
            var t = await _db.Tasks.FindAsync(id);
            if (t == null) return;
            _db.Tasks.Remove(t);
            await _db.SaveChangesAsync();
        }
    }

    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _db;
        public UserRepository(AppDbContext db) { _db = db; }

        public async Task<AppUser?> GetByUsernameAsync(string username)
            => await _db.Users.FirstOrDefaultAsync(u => u.Username == username);

        public async Task<AppUser> CreateAsync(AppUser user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }
    }
}
