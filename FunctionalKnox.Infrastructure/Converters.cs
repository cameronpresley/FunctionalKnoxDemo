using System;
using Optionally;

namespace FunctionalKnox.Infrastructure
{
    public static class Converters
    {
        public static IOption<int> ParseInt(string s)
        {
            return Int32.TryParse(s, out int num)
                ? Option.Some(num)
                : Option.No<int>();
        }
    }
}
