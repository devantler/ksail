using System.Text.RegularExpressions;

namespace KSail.Utils;

static partial class RegexFilters
{
  [GeneratedRegex("^.+/[^/]*$")]
  internal static partial Regex PathFilter();

  [GeneratedRegex("^(y|n|yes|no|true|false)$")]
  internal static partial Regex YesNoFilter();
}
