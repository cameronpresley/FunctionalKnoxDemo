using System;
using System.Collections.Generic;
using FunctionalKnox.DataAccess;
using FunctionalKnox.Domain;
using FunctionalKnoxDemo.Workflows.AddWorkItem;
using FunctionalKnoxDemo.Workflows.PrintWorkItem;
using FunctionalKnoxDemo.Workflows.ViewWorkItem;
using Optionally;

namespace FunctionalKnoxDemo
{
    class Program
    {
        enum UserChoice
        {
            PrintAll,
            Add,
            ViewSingle,
            Quit
        }

        static void Main(string[] args)
        {
            var repo = new WorkItemRepository();
            var workflows = GetWorkflows();
            void MenuLoop()
            {
                PrintMenu();
                GetChoice()
                    .Do(onFailure: error =>
                        {
                            Console.WriteLine(error);
                            MenuLoop();
                        },
                        onSuccess: userChoice =>
                        {
                            if (workflows.TryGetValue(userChoice, out Action<IRepository<WorkItem>> action))
                            {
                                action(repo);
                                MenuLoop();
                            }
                        });
            }

            MenuLoop();
        }

        private static Dictionary<UserChoice, Action<IRepository<WorkItem>>> GetWorkflows()
        {
            return new Dictionary<UserChoice, Action<IRepository<WorkItem>>>
            {
                {UserChoice.PrintAll, PrintAllWorkItems},
                {UserChoice.Add, AddWorkItem },
                {UserChoice.ViewSingle, ViewSingleWorkItem },
            };
        }

        private static void PrintAllWorkItems(IRepository<WorkItem> repo)
        {
            new PrintWorkItems(repo).Run();
        }

        private static void AddWorkItem(IRepository<WorkItem> repo)
        {
            new AddWorkItemUsingResult(repo).Run();
        }

        private static void ViewSingleWorkItem(IRepository<WorkItem> repo)
        {
            new ViewWorkItem(repo).Run();
        }

        private static void PrintMenu()
        {
            List<string> CreateMenu()
            {
                return new List<string>
                {
                    "1). Print All Work Items",
                    "2). Add a Work Item",
                    "3). View a Work Item",
                    "4). Exit",
                };
            }
            CreateMenu().ForEach(Console.WriteLine);
        }

        private static IResult<string, UserChoice> GetChoice()
        {
            var input = Console.ReadLine();
            switch (input.Trim())
            {
                case "1": return Result.Success<string, UserChoice>(UserChoice.PrintAll);
                case "2": return Result.Success<string, UserChoice>(UserChoice.Add);
                case "3": return Result.Success<string, UserChoice>(UserChoice.ViewSingle);
                case "4": return Result.Success<string, UserChoice>(UserChoice.Quit);
                default: return Result.Failure<string, UserChoice>("Couldn't parse " + input + " as a command.");
            }
        }
    }
}