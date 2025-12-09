using System;

namespace TaskManagementApp.Services.Exceptions
{
    public class TaskNotFoundException : Exception
    {
        public TaskNotFoundException(int taskId) 
            : base($"Task with ID {taskId} not found") 
        {
        }
    }
}
