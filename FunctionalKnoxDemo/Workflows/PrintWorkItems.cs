using System;
using System.Collections.Generic;
using System.Linq;
using FunctionalKnox.DataAccess;
using FunctionalKnox.Domain;

namespace FunctionalKnoxDemo.Workflows
{
    public class PrintWorkItems
    {
        private readonly IRepository<WorkItem> _repo;

        public PrintWorkItems(IRepository<WorkItem> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        private string ConvertStatusToDisplay(Status status)
        {
            switch (status)
            {
                case Status.ToDo:
                    return "To Do";
                case Status.InProgress:
                    return "In Progress";
                case Status.Done:
                    return "Done";
                default:
                    throw new ArgumentOutOfRangeException(nameof(status), status, null);
            }
        }

        private string ConvertWorkItemToDisplay(WorkItem item)
        {
            return $"{item.Id}, {item.Title}, {item.Description}, {ConvertStatusToDisplay(item.Status)}";
        }

        private string CreateDisplay(IEnumerable<WorkItem> items)
        {
            var headerRow = "ID, Title, Description, Status";
            var rows = items.Select(ConvertWorkItemToDisplay).ToList();

            return headerRow + Environment.NewLine + String.Join(Environment.NewLine, rows);
        }

        public void Workflow()
        {
            _repo
                .GetAll()
                .Map(CreateDisplay)
                .Do(
                    ex => Console.WriteLine("Failed to retrieve records from the database."),
                    Console.WriteLine
                );
        }
    }
}
