using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Optionally;

namespace FunctionalKnoxDemo.Workflows.AddWorkItem
{
    public static class WorkItemValidationWithResults
    {
        public static IResult<string, string> ValidateTitleResult(string title)
        {
            if (title == null) return Result.Failure<string, string>("Title must be specified.");
            if (String.IsNullOrWhiteSpace(title)) return Result.Failure<string, string>("Title cannot be empty");
            if (title.Trim().Length > 50) return Result.Failure<string, string>("Title cannot be more than 50 characters.");
            return Result.Success<string, string>(title);
        }

        public static IResult<string, string> ValidateDescriptionResult(string description)
        {
            if (description == null) return Result.Failure<string, string>("Description must be specified.");
            if (String.IsNullOrWhiteSpace(description)) return Result.Failure<string, string>("Description cannot be empty");
            if (description.Trim().Length > 100)
                return Result.Failure<string, string>("Description cannot be more than 100 characters.");

            return Result.Success<string, string>(description);
        }
    }
}
