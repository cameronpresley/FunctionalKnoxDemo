using System;
using FunctionalKnox.DataAccess;
using FunctionalKnox.Domain;
using Optionally;

namespace FunctionalKnoxDemo.Workflows
{
    public class AddWorkItemUsingOption
    {
        private readonly IRepository<WorkItem> _repo;

        public AddWorkItemUsingOption(IRepository<WorkItem> repo)
        {
            _repo = repo ?? throw new ArgumentNullException(nameof(repo));
        }

        public IOption<string> ValidateTitleOption(string title)
        {
            if (title == null) return Option.No<string>();
            if (String.IsNullOrWhiteSpace(title)) return Option.No<string>();
            if (title.Trim().Length > 50) return Option.No<string>();

            return Option.Some(title);
        }

        public IOption<string> ValidateDescriptionOption(string description)
        {
            if (description == null) return Option.No<string>();
            if (String.IsNullOrWhiteSpace(description)) return Option.No<string>();
            if (description.Trim().Length > 100) return Option.No<string>();

            return Option.Some(description);
        }

        public void OptionWorkflow()
        {
            Console.WriteLine("Title:");
            var titleOption = ValidateTitleOption(Console.ReadLine());
            Console.WriteLine("Description:");
            var descOption = ValidateDescriptionOption(Console.ReadLine());

            Option.Apply(WorkItem.Create, titleOption, descOption)
                .Map(_repo.Add)
                .Do(
                    () => Console.WriteLine("Failed to create the work item due to validation errors."),
                    result => result.Do(
                        ex => Console.WriteLine("Failed to save to the repository " + ex.Message),
                        item => Console.WriteLine("Saved workitem #" + item.Id + " to the repository.")));
        }
    }
}
