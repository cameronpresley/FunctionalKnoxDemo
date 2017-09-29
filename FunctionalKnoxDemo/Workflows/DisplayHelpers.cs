using FunctionalKnox.Domain;
using Optionally;

namespace FunctionalKnoxDemo.Workflows
{
    public static class DisplayHelpers
    {
        public static IOption<string> ConvertStatusToDisplay(Status status)
        {
            switch (status)
            {
                case Status.ToDo:
                    return Option.Some("To Do");
                case Status.InProgress:
                    return Option.Some("In Progress");
                case Status.Done:
                    return Option.Some("Done");
                default:
                    return Option.No<string>();
            }
        }

        public static string ConvertWorkItemToDisplay(WorkItem item)
        {
            var formattedStatus =
                ConvertStatusToDisplay(item.Status)
                    .Match(() => "None", x => x);

            return $"{item.Id}, {item.Title}, {item.Description}, {formattedStatus}";
        }

    }
}
