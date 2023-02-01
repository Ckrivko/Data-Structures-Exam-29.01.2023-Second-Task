namespace Exam.TaskManager
{
    public class Task
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public int EstimatedExecutionTime { get; set; }

        public string Domain { get; set; }

        public bool IsExecuted { get; set; }

        public Task(string id, string name, int estimatedExecutionTime, string domain)
        {
            Id = id;
            Name = name;
            EstimatedExecutionTime = estimatedExecutionTime;
            Domain = domain;
            IsExecuted = false;
        }

        //public override bool Equals(object obj)
        //{
        //    return Id.Equals(obj);
        //}

        //public override int GetHashCode()
        //{
        //    return Id.GetHashCode();
        //}

    }
}
