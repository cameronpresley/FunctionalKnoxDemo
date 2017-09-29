using System;
using System.Collections.Generic;
using System.Linq;
using FunctionalKnox.DataAccess;
using FunctionalKnox.Domain;

namespace FunctionalKnoxDemo.Workflows.PrintWorkItem
{
    public class PrintWorkItems : IWorkflow
    {
        private readonly IRepository<WorkItem> _repo;

        public PrintWorkItems(IRepository<WorkItem> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }
        
        private string CreateDisplay(IEnumerable<WorkItem> items)
        {
            var headerRow = "ID, Title, Description, Status";
            var rows = items
                .Select(DisplayHelpers.ConvertWorkItemToDisplay)
                .ToList();

            return headerRow + Environment.NewLine + String.Join(Environment.NewLine, rows);
        }

        public void Run()
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
