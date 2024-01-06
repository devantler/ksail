using System.Text.RegularExpressions;

namespace KSail.Utils;

/// <summary>
/// A class containing regex filters.
/// </summary>
public static partial class RegexFilters
{
  [GeneratedRegex("^.+/[^/]*$")]
  public static partial Regex PathFilter();

  [GeneratedRegex("^(y|n|yes|no|true|false)$")]
  public static partial Regex YesNoFilter();
}
