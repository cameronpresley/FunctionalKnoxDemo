using System;
using FunctionalKnoxDemo.DataAccess;
using FunctionalKnoxDemo.Models;
using Optionally;

namespace FunctionalKnoxDemo.Workflows
{
    public class ViewWorkItem
    {
        private readonly IRepository<WorkItem> _repo;

        public ViewWorkItem(IRepository<WorkItem> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public IOption<int> GetId(string idString)
        {
            return Int32.TryParse(idString, out int id)
                ? Option.Some(id)
                : Option.No<int>();
        }

        private string ConvertWorkItemToString(WorkItem item)
        {
            return $"{item.Id}, {item.Title}, {item.Description}, {item.Status}";
        }

        public void Workflow()
        {
            Console.WriteLine("What's the workitem's ID?");
            GetId(Console.ReadLine())
                .Map(_repo.GetById)
                .Do(
                    () => Console.WriteLine("Couldn't find record with a nonnumber ID"),
                    result =>
                    result.Do(
                            ex => Console.WriteLine("Failed to retrieve record : " + ex.Message),
                            option =>
                                option.Map(ConvertWorkItemToString)
                                .Do(
                                    () => Console.WriteLine("Couldn't find record with speicified id"),
                                    Console.WriteLine
                                )
                            )
                );
        }
    }
}
