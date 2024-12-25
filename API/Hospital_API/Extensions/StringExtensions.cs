using Hospital_API.Constants;

namespace Hospital_API.Extensions;

public static class StringExtensions
{
    /// <summary>
    /// Convert a string representation of DateTime to a time period.
    /// The period depends on the precision of the date.
    /// For example, 2024-04 would be equal to the period
    /// 2024-02-01T00:00:00 to 2024-04-29T23:59:59 if a leap year
    /// </summary>
    ///<returns>
    /// Returns: true if dateString was successfully; otherwise, false.
    /// Out defining a date and time period
    /// </returns>
    public static bool TryGetSearchingDateRange(this String dateString, out DateTime startDate, out DateTime endDate)
    {
        bool isSuccess = true;
        int[] dateIntStart = { DateOnly.MinValue.Year, DateOnly.MinValue.Month, DateOnly.MinValue.Day };
        int[] dateIntEnd = { DateOnly.MaxValue.Year, DateOnly.MaxValue.Month, DateOnly.MaxValue.Month };
        int[] timeIntStart = { TimeOnly.MinValue.Hour, TimeOnly.MinValue.Minute, TimeOnly.MinValue.Second };
        int[] timeIntEnd = { TimeOnly.MaxValue.Hour, TimeOnly.MaxValue.Minute, TimeOnly.MaxValue.Second };

        startDate = new();
        endDate = new();

        try
        {
            string[] dateSplit = dateString.Split('T');

            if (dateSplit.Any())
            {
                int counter = 0;

                //Date Parsing: Determine the maximum and minimum date based on the input data
                if (dateSplit.Length <= dateIntStart.Length)
                {
                    string[] dateStr = dateSplit[counter].Split('-');

                    for (int d = 0; d < dateStr.Length; d++)
                    {
                        if (!int.TryParse(dateStr[d], out int result))
                        {
                            isSuccess = false;
                            break;
                        } 
                        dateIntStart[d] = result;
                        dateIntEnd[d] = result;
                    }

                    //Calculate the maximum number of days in a month if the number of days is not provided
                    if (dateStr.Length < dateIntEnd.Length)
                        dateIntEnd[2] = DateTime.DaysInMonth(dateIntStart[0], dateIntStart[1]);

                    counter++;
                }

                //Time Parsing: Determine the maximum and minimum time based on the input data
                if (dateSplit.Length >= counter + 1)
                {
                    string[] timeStr = dateSplit[counter].Split(':');

                    for (int t = 0; t < timeStr.Length; t++)
                    {
                        if (!int.TryParse(timeStr[t], out int result))
                        {
                            isSuccess = false;
                            break;
                        }
                        timeIntStart[t] = result;
                        timeIntEnd[t] = result;
                    }
                }
            }
            startDate = new(
                dateIntStart[0], dateIntStart[1], dateIntStart[2],
                timeIntStart[0], timeIntStart[1], timeIntStart[2],
                DateTimeKind.Utc);

            endDate = new(
                dateIntEnd[0], dateIntEnd[1], dateIntEnd[2],
                timeIntEnd[0], timeIntEnd[1], timeIntEnd[2],
                DateTimeKind.Utc);
        }
        catch
        {
           isSuccess = false;
        }

        return isSuccess;
    }


    /// <summary>
    /// Convert a string representation of DateTime to a time period and SearchingPrefix.
    /// The period depends on the precision of the date.
    /// For example, 2024-04 would be equal to the period
    /// 2024-02-01T00:00:00 to 2024-04-29T23:59:59 if a leap year
    /// If the prefix code was not specified, the prefix code will be SearchingPrefix.Eq
    /// </summary>
    ///<returns>
    /// Returns: true if data was successfully; otherwise, false.
    /// Out defining a date and time period
    /// </returns>
    public static bool TryParseSearchingData(this String data, out SearchingPrefix prefix, out DateTime startDate, out DateTime endDate)
    {
        bool isSuccess;
        string prefixStr = data[..2];

        if (double.TryParse(prefixStr, out _))
        {
            prefix = SearchingPrefix.Eq;
            isSuccess = data.TryGetSearchingDateRange(out startDate, out endDate);
        }
        else
        {
            if (Enum.TryParse(prefixStr, true, out prefix))
            {
                isSuccess = data[2..].TryGetSearchingDateRange(out startDate, out endDate);
            }
            else
            {
                startDate = new();
                endDate = new();
                isSuccess = false;
            }                            
        }

        return isSuccess;
    }
}
