using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Statistics
{
    public enum TimeStepType
    {
        Default,
        Hour,
        Day,
        Week,
        Month
    }

    public static class TimeStepUtils
    {

        public static IList<TimeStepType> GetOptionsForTimeSpan(TimeSpan ts)
        {
            var result = new List<TimeStepType>();

            result.Add(TimeStepType.Default);

            if (ts.TotalHours < 50.0)
            {
                result.Add(TimeStepType.Hour);
            }

            if (ts.TotalDays < 50.0 && ts.TotalDays > 1.5 )
            {
                result.Add(TimeStepType.Day);
            }

            if (ts.TotalDays < 370.0 && ts.TotalDays > 8.0)
            {
                result.Add(TimeStepType.Week);
            }

            if (ts.TotalDays > 33.0)
            {
                result.Add(TimeStepType.Month);
            }

            return result;
        }


    }
}
