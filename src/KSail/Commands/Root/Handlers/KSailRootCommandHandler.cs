using System.CommandLine;

namespace KSail.Commands.Root.Handlers;

/// <summary>
/// Handles the KSail root command.
/// </summary>
class KSailRootCommandHandler
{
  /// <summary>
  /// Handles the KSail root command.
  /// </summary>
  /// <param name="console"></param>
  internal bool Handle(IConsole console)
  {
    PrintIntroduction(console);
    return true;
  }

  void PrintIntroduction(IConsole console) => console.WriteLine(Introduction);

  const string Introduction = @"
    🛥️ 🐳    Welcome to KSail!    🛥️ 🐳
                                         . . .
                    __/___                 :
              _____/______|             ___|____     |""\/""|
      _______/_____\_______\_____     ,'        `.    \  /
      \               KSail      |    |  ^        \___/  |
    ~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^~^
";
}
