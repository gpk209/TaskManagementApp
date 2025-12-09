using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using TaskManagementApp.Core.Entities;
using TaskManagementApp.Core.Interfaces;
using TaskManagementApp.Services.Exceptions;

namespace TaskManagementApp.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskReadRepository _read;
        private readonly ITaskWriteRepository _write;
        private readonly ILogger<TaskService> _logger;

        public TaskService(
            ITaskReadRepository read, 
            ITaskWriteRepository write,
            ILogger<TaskService> logger)
        {
            _read = read ?? throw new ArgumentNullException(nameof(read));
            _write = write ?? throw new ArgumentNullException(nameof(write));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<TaskItem>> ListAsync(Status? status = null, Priority? priority = null)
        {
            _logger.LogDebug("Listing tasks with filters - Status: {Status}, Priority: {Priority}", 
                status, priority);
            
            var tasks = await _read.GetAllAsync(status, priority);
            
            _logger.LogInformation("Retrieved {Count} tasks", 
                tasks is ICollection<TaskItem> collection ? collection.Count : "unknown");
            
            return tasks;
        }

        public async Task<TaskItem?> GetAsync(int id)
        {
            if (id <= 0)
            {
                _logger.LogWarning("Invalid task ID requested: {Id}", id);
                return null;
            }

            _logger.LogDebug("Retrieving task with ID: {Id}", id);
            return await _read.GetByIdAsync(id);
        }

        public async Task<TaskItem> CreateAsync(TaskItem dto)
        {
            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            // Additional validation
            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Task title is required", nameof(dto));

            _logger.LogInformation("Creating new task: {Title}", dto.Title);
            
            var created = await _write.CreateAsync(dto);
            
            _logger.LogInformation("Task created successfully with ID: {Id}", created.Id);
            return created;
        }

        public async Task UpdateAsync(int id, TaskItem dto)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid task ID", nameof(id));

            if (dto == null)
                throw new ArgumentNullException(nameof(dto));

            if (string.IsNullOrWhiteSpace(dto.Title))
                throw new ArgumentException("Task title is required", nameof(dto));

            _logger.LogInformation("Updating task with ID: {Id}", id);

            var existing = await _read.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Update failed - task not found with ID: {Id}", id);
                throw new TaskNotFoundException(id);
            }

            // Update properties
            existing.Title = dto.Title;
            existing.Description = dto.Description;
            existing.DueDate = dto.DueDate;
            existing.Priority = dto.Priority;
            existing.Status = dto.Status;

            await _write.UpdateAsync(existing);
            _logger.LogInformation("Task updated successfully: {Id}", id);
        }

        public async Task DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("Invalid task ID", nameof(id));

            _logger.LogInformation("Deleting task with ID: {Id}", id);

            // Check if task exists before deleting
            var existing = await _read.GetByIdAsync(id);
            if (existing == null)
            {
                _logger.LogWarning("Delete failed - task not found with ID: {Id}", id);
                throw new TaskNotFoundException(id);
            }

            await _write.DeleteAsync(id);
            _logger.LogInformation("Task deleted successfully: {Id}", id);
        }
    }
}
