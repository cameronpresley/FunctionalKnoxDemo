using System;
using System.Collections.Generic;
using System.Linq;
using FunctionalKnox.DataAccess;
using FunctionalKnox.Domain;
using Optionally;

namespace FunctionalKnoxDemo.Workflows.AddWorkItem
{
    public class AddWorkItemUsingResult : IWorkflow
    {
        private readonly IRepository<WorkItem> _repo;

        public AddWorkItemUsingResult(IRepository<WorkItem> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public void Run()
        {
            var titleResult = GetTitle();
            var descResult = GetDescription();

            void FailureAction(IEnumerable<string> failures)
            {
                Console.WriteLine("Failed to create work item due to validation errors.");
                failures.ToList().ForEach(Console.WriteLine);
            }

            void SuccessAction(IResult<Exception, WorkItem> result)
            {
                void OnFailure(Exception ex)
                {
                    Console.WriteLine("Failed to save workitem to the repostiory" + Environment.NewLine + ex.Message);
                }

                void OnSuccess(WorkItem item)
                {
                    Console.WriteLine($"Saved workitem #{item.Id} to the repository");
                }

                result.Do(OnFailure, OnSuccess);
            }

            Result.Apply(WorkItem.Create, titleResult, descResult)
                .Map(_repo.Add)
                .Do(FailureAction, SuccessAction);
        }

        private static IResult<string, string> GetTitle()
        {
            Console.WriteLine("Title");
            var titleResult =
                WorkItemValidationWithResults
                    .ValidateTitleResult(Console.ReadLine());
            return titleResult;
        }

        private static IResult<string, string> GetDescription()
        {
            Console.WriteLine("Description");
            var descResult = WorkItemValidationWithResults
                .ValidateDescriptionResult(Console.ReadLine());
            return descResult;
        }
    }
}
