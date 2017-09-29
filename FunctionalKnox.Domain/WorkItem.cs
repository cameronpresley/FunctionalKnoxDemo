namespace FunctionalKnox.Domain
{
    public class WorkItem
    {
        public int Id;
        public readonly string Title;
        public readonly string Description;
        public Status Status;

        private WorkItem(int id, string title, string description, Status status)
        {
            Id = id;
            Title = title;
            Description = description;
            Status = status;
        }

        public static WorkItem Create(string title, string description)
        {
            return new WorkItem(-1, title, description, Status.ToDo);
        }
    }
}