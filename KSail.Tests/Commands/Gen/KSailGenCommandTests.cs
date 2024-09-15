using System.CommandLine;
using System.CommandLine.IO;
using KSail.Commands.Gen;

namespace KSail.Tests.Commands.Gen;

/// <summary>
/// Tests for the <see cref="KSailGenCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailGenCommandTests : IAsyncLifetime
{
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the 'ksail gen' command succeeds and returns the help text.
  /// </summary>
  [Theory]
  [MemberData(nameof(TheoryData.HelpTheoryData), MemberType = typeof(TheoryData))]
  public async Task KSailGen_SucceedsAndPrintsHelp(string command)
  {
    //Arrange
    var console = new TestConsole();
    var ksailCommand = new KSailGenCommand(console);

    //Act
    int exitCode = await ksailCommand.InvokeAsync(command, console);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the 'ksail gen' command generates a resource.
  /// </summary>
  /// <returns></returns>
  [Theory]
  [MemberData(nameof(TheoryData.GenerateNativeResourceTheoryData), MemberType = typeof(TheoryData))]
  public async Task KSailGen_SucceedsAndGeneratesAResource(string command, string fileName)
  {
    //Arrange
    var ksailCommand = new KSailGenCommand();

    //Act
    string outputPath = Path.Combine(Path.GetTempPath(), fileName);
    if (File.Exists(outputPath))
    {
      File.Delete(outputPath);
    }
    int exitCode = await ksailCommand.InvokeAsync(command + $" --output {outputPath}");
    string fileContents = await File.ReadAllTextAsync(outputPath);

    //Assert
    Assert.Equal(0, exitCode);
    _ = await Verify(fileContents, extension: "yaml").UseFileName(fileName);

    //Cleanup
    File.Delete(outputPath);
  }
}
