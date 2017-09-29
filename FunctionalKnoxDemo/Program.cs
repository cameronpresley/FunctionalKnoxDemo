using System;
using FunctionalKnox.DataAccess;
using FunctionalKnox.Domain;
using FunctionalKnoxDemo.Workflows;

namespace FunctionalKnoxDemo
{
    class Program
    {
        enum UserChoice
        {
            Unknown,
            PrintAll,
            Add,
            ViewSingle,
            Quit
        }

        static void Main(string[] args)
        {
            var repo = new WorkItemRepository();

            UserChoice choice;
            do
            {
                PrintMenu();
                choice = GetChoice();
                switch (choice)
                {
                    case UserChoice.PrintAll:
                        PrintAllWorkItems(repo);
                        break;
                    case UserChoice.Add:
                        AddWorkItem(repo);
                        break;
                    case UserChoice.ViewSingle:
                        ViewSingleWorkItem(repo);
                        break;
                }
            } while (choice != UserChoice.Quit);
        }

        private static void ViewSingleWorkItem(WorkItemRepository repo)
        {
            new ViewWorkItem(repo).Workflow();
        }

        private static void AddWorkItem(IRepository<WorkItem> repo)
        {
            new AddWorkItemUsingResult(repo).ResultWorkflow();
            //new AddWorkItemUsingOption(repo).OptionWorkflow();
        }

        private static void PrintAllWorkItems(WorkItemRepository repo)
        {
            new PrintWorkItems(repo).Workflow();
        }

        private static UserChoice GetChoice()
        {
            var input = Console.ReadLine();
            switch (input.Trim())
            {
                case "1": return UserChoice.PrintAll;
                case "2": return UserChoice.Add;
                case "3": return UserChoice.ViewSingle;
                case "4": return UserChoice.Quit;
                default: return UserChoice.Unknown;
            }
        }

        private static void PrintMenu()
        {
            Console.WriteLine("1). Print All Work Items");
            Console.WriteLine("2). Add a Work Item");
            Console.WriteLine("3). View a Work Item");
            Console.WriteLine("4). Exit");
        }
    }
}
