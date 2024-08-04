using System.CommandLine;
using System.CommandLine.IO;
using System.Globalization;
using System.Reflection;
using System.Resources;
using KSail.Commands.Check;

namespace KSail.Tests.Commands.Check;

/// <summary>
/// Tests for the <see cref="KSailCheckCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailCheckCommandTests : IAsyncLifetime
{
  readonly ResourceManager ResourceManager = new("KSail.Tests.Commands.Check.Resources", Assembly.GetExecutingAssembly());
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the <c>ksail check</c> command fails when given invalid kubeconfig path.
  /// </summary>
  [Fact]
  public async Task KSailCheck_GivenInvalidKubeconfigPath_Fails()
  {
    Console.WriteLine(ResourceManager.GetString(nameof(KSailCheck_GivenInvalidKubeconfigPath_Fails), CultureInfo.InvariantCulture));
    //Arrange
    var console = new TestConsole();
    var ksailCheckCommand = new KSailCheckCommand();

    //Act
    int exitCode = await ksailCheckCommand.InvokeAsync("--kubeconfig /path/to/invalid/kubeconfig", console);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the <c>ksail check</c> command fails when given an empty kubeconfig path.
  /// </summary>
  [Fact]
  public async Task KSailCheck_GivenNoKubeconfigPath_Fails()
  {
    Console.WriteLine(ResourceManager.GetString(nameof(KSailCheck_GivenInvalidKubeconfigPath_Fails), CultureInfo.InvariantCulture));
    //Arrange
    var console = new TestConsole();
    var ksailCheckCommand = new KSailCheckCommand();

    //Act
    try
    {
      _ = await ksailCheckCommand.InvokeAsync("--kubeconfig ", console);
    }
    catch (InvalidOperationException exception)
    {
      //Assert
      _ = await Verify(exception.Message);
    }
  }

  /// <summary>
  /// Tests that the <c>ksail check</c> command succeeds when given a valid kubeconfig path.
  /// </summary>
  [Fact]
  public async Task KSailCheck_GivenValidKubeconfigPathAndInvalidContext_Fails()
  {
    Console.WriteLine(ResourceManager.GetString(nameof(KSailCheck_GivenValidKubeconfigPathAndInvalidContext_Fails), CultureInfo.InvariantCulture));
    //Arrange
    var console = new TestConsole();
    var ksailCheckCommand = new KSailCheckCommand();

    //Act
    int exitCode = await ksailCheckCommand.InvokeAsync("--context ksail", console);

    //Assert
    Assert.Equal(1, exitCode);
  }
}
