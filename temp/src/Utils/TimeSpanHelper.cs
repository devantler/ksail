using System.Globalization;
using System.Text.RegularExpressions;

namespace KSail.Utils;

/// <summary>
/// Helper class for <see cref="TimeSpan"/>.
/// </summary>
public static class TimeSpanHelper
{
  static readonly List<(string Abbreviation, TimeSpan InitialTimeSpan)> _timeSpanInfos =
  [
          ("y", TimeSpan.FromDays(365)), // Year
          ("M", TimeSpan.FromDays(30)), // Month
          ("w", TimeSpan.FromDays(7)), // Week
          ("d", TimeSpan.FromDays(1)), // Day
          ("h", TimeSpan.FromHours(1)), // Hour
          ("m", TimeSpan.FromMinutes(1)), // Minute
          ("s", TimeSpan.FromSeconds(1)), // Second
          ("t", TimeSpan.FromTicks(1)) // Tick
  ];

  /// <summary>
  /// Parse a duration string into a <see cref="TimeSpan"/>.
  /// </summary>
  /// <param name="format"></param>
  /// <returns></returns>
  public static TimeSpan ParseDuration(string format)
  {
    var result = _timeSpanInfos
        .Where(timeSpanInfo => format.Contains(timeSpanInfo.Abbreviation, StringComparison.Ordinal))
        .Select(timeSpanInfo => timeSpanInfo.InitialTimeSpan * int.Parse(new Regex(@$"(\d+){timeSpanInfo.Abbreviation}").Match(format).Groups[1].Value, CultureInfo.InvariantCulture))
        .Aggregate((accumulator, timeSpan) => accumulator + timeSpan);
    return result;
  }
}
