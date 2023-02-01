using System;
using System.Collections.Generic;
using System.Linq;

namespace Exam.TaskManager
{
    public class TaskManager : ITaskManager
    {
        private Dictionary<string, Task> tasksById = new Dictionary<string, Task>();
        private Dictionary<string, List<Task>> domainByName = new Dictionary<string, List<Task>>();
        private LinkedList<Task> waitingTasks = new LinkedList<Task>();
        private HashSet<Task> executedTasks = new HashSet<Task>();

        public TaskManager()
        {

        }
        public void AddTask(Task task)
        {
            if (this.tasksById.ContainsKey(task.Id))
            {
                throw new ArgumentException();
            }

            if (!domainByName.ContainsKey(task.Domain))
            {
                domainByName.Add(task.Domain, new List<Task>());
            }

            this.tasksById.Add(task.Id, task);
            this.waitingTasks.AddLast(task);

            this.domainByName[task.Domain].Add(task);

        }

        public bool Contains(Task task)
        {
            if (this.tasksById.ContainsKey(task.Id))
            {
                return true;
            }
            return false;
        }

        public void DeleteTask(string taskId)
        {
            if (!this.tasksById.ContainsKey(taskId))
            {
                throw new ArgumentException();
            }

            var currTask = this.tasksById[taskId];

            if (currTask.IsExecuted == false)
            {
                this.waitingTasks.Remove(currTask);    //waiting task deleted
            }
            else
            {
                this.executedTasks.Remove(currTask);  //executed task deleted
            }

            this.domainByName[currTask.Domain].Remove(currTask);   //domain deleted
            this.tasksById.Remove(taskId);                         // all task deleted
        }

        public Task ExecuteTask()
        {

            if (this.waitingTasks.Count == 0)
            {
                throw new ArgumentException();
            }

            var currTask = this.waitingTasks.First.Value;
            this.waitingTasks.RemoveFirst();

            currTask.IsExecuted = true;
            this.executedTasks.Add(currTask);

            return currTask;
        }

        public IEnumerable<Task> GetAllTasksOrderedByEETThenByName()
        {
            //EET in descending order, then by length of name in ascending order

            return this.tasksById.Values
                .OrderByDescending(x => x.EstimatedExecutionTime)
                .ThenBy(x => x.Name);

        }

        public IEnumerable<Task> GetDomainTasks(string domain)
        {
            if (!domainByName.ContainsKey(domain))
            {
                throw new ArgumentException();
            }

            var resultTasks = this.domainByName[domain].Where(x => x.IsExecuted == false).ToList();

            if (resultTasks.Count == 0)
            {
                throw new ArgumentException();

            }

            return resultTasks;
        }

        public Task GetTask(string taskId)
        {

            if (!this.tasksById.ContainsKey(taskId))
            {
                throw new ArgumentException();
            }
            return this.tasksById[taskId];
        }

        public IEnumerable<Task> GetTasksInEETRange(int lowerBound, int upperBound)
        {
            // returns all of the unexecuted tasks with an EET in the range specified with lower 
            // bound and upper bound.Both bounds are inclusive. The results should be ordered by 
            // position in the execution queue, in ascending order.
            //If there aren’t any tasks in the specified range – return an empty collection.

            return  this.waitingTasks             
                .Where(x => x.EstimatedExecutionTime >= lowerBound
            && x.EstimatedExecutionTime <= upperBound);

            
        }

        public void RescheduleTask(string taskId)
        {
            if (!this.tasksById.ContainsKey(taskId))
            {
                throw new ArgumentException();
            }

            var currTask = this.tasksById[taskId];

            if (currTask.IsExecuted == false)
            {
                throw new ArgumentException();
            }

            this.executedTasks.Remove(currTask);
            currTask.IsExecuted = false;

            this.waitingTasks.AddLast(currTask);

        }

        public int Size()
        {
            return this.waitingTasks.Count;
        }
    }
}
