using System;
using System.Collections.Generic;
using System.Linq;
using FunctionalKnoxDemo.DataAccess;
using FunctionalKnoxDemo.Models;
using Optionally;

namespace FunctionalKnoxDemo.Workflows
{
    public class AddWorkItemUsingResult
    {
        private readonly IRepository<WorkItem> _repo;

        public AddWorkItemUsingResult(IRepository<WorkItem> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public IResult<string, string> ValidateTitleResult(string title)
        {
            if (title == null) return Result.Failure<string, string>("Title must be specified.");
            if (String.IsNullOrWhiteSpace(title)) return Result.Failure<string, string>("Title cannot be empty");
            if (title.Trim().Length > 50) return Result.Failure<string, string>("Title cannot be more than 50 characters.");
            return Result.Success<string, string>(title);
        }

        public IResult<string, string> ValidateDescriptionResult(string description)
        {
            if (description == null) return Result.Failure<string, string>("Description must be specified.");
            if (String.IsNullOrWhiteSpace(description)) return Result.Failure<string, string>("Description cannot be empty");
            if (description.Trim().Length > 100)
                return Result.Failure<string, string>("Description cannot be more than 100 characters.");

            return Result.Success<string, string>(description);
        }

        public void ResultWorkflow()
        {
            Console.WriteLine("Title");
            var titleResult = ValidateTitleResult(Console.ReadLine());
            Console.WriteLine("Description");
            var descResult = ValidateDescriptionResult(Console.ReadLine());

            void FailureAction(IEnumerable<string> failures)
            {
                Console.WriteLine("Failed to create work item due to validation errors.");
                failures.ToList().ForEach(Console.WriteLine);
            }

            void SuccessAction(IResult<Exception, WorkItem> result)
            {
                result.Do(ex => Console.WriteLine("Failed to save workitem to the repository"),
                          item => Console.WriteLine($"Saved workitem #{item.Id} to the repository"));
            }

            Result.Apply(WorkItem.Create, titleResult, descResult)
                    .Map(_repo.Add)
                    .Do(FailureAction, SuccessAction);
        }
    }
}
