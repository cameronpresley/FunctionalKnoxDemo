using System;
using FunctionalKnox.DataAccess;
using FunctionalKnox.Domain;
using FunctionalKnox.Infrastructure;
using Optionally;

namespace FunctionalKnoxDemo.Workflows.ViewWorkItem
{
    public class ViewWorkItem : IWorkflow
    {
        private readonly IRepository<WorkItem> _repo;

        public ViewWorkItem(IRepository<WorkItem> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        private IOption<int> GetId(string idString)
        {
            return Converters.ParseInt(idString);
        }

        public void Run()
        {
            Console.WriteLine("What's the workitem's ID?");
            GetId(Console.ReadLine())
                .Map(_repo.GetById)
                .Do(
                    ifNone:FailedToParseIdAsInt,
                    ifSome: ParsedToInt);

            void FailedToParseIdAsInt()
            {
                Console.WriteLine("Couldn't find record with a nonnumber ID");
            }

            void ParsedToInt(IResult<Exception, IOption<WorkItem>> result)
            {
                void FailedToRetrieveRecord(Exception ex)
                {
                    Console.WriteLine("Failed to retrieve record : " + ex.Message);
                }

                void RetrievedRecord(IOption<WorkItem> workItemOption)
                {
                    workItemOption.Map(DisplayHelpers.ConvertWorkItemToDisplay)
                        .Do(
                            () => Console.WriteLine("Couldn't find record with speicified id"),
                            Console.WriteLine
                        );
                }
                result.Do(
                    onFailure:FailedToRetrieveRecord, 
                    onSuccess:RetrievedRecord
                );
            }
        }
    }
}
