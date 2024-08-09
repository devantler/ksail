using System.CommandLine;
using System.CommandLine.IO;
using System.Globalization;
using System.Reflection;
using System.Resources;
using KSail.Commands.Debug;

namespace KSail.Tests.Commands.Debug;

/// <summary>
/// Tests for the <see cref="KSailDebugCommand"/> class.
/// </summary>
[Collection("KSail.Tests")]
public class KSailDebugCommandTests : IAsyncLifetime
{
  readonly ResourceManager ResourceManager = new("KSail.Tests.Commands.Debug.Resources", Assembly.GetExecutingAssembly());
  /// <inheritdoc/>
  public Task DisposeAsync() => Task.CompletedTask;
  /// <inheritdoc/>
  public Task InitializeAsync() => Task.CompletedTask;

  /// <summary>
  /// Tests that the <c>ksail debug</c> command fails when given invalid kubeconfig path.
  /// </summary>
  [Fact]
  public async Task KSailDebug_GivenInvalidKubeconfigPath_Fails()
  {
    Console.WriteLine(ResourceManager.GetString(nameof(KSailDebug_GivenInvalidKubeconfigPath_Fails), CultureInfo.InvariantCulture));
    //Arrange
    var console = new TestConsole();
    var ksailDebugCommand = new KSailDebugCommand();

    //Act
    int exitCode = await ksailDebugCommand.InvokeAsync("--kubeconfig /path/to/invalid/kubeconfig", console);

    //Assert
    Assert.Equal(1, exitCode);
    _ = await Verify(console.Error.ToString() + console.Out);
  }

  /// <summary>
  /// Tests that the <c>ksail debug</c> command fails when given an empty kubeconfig path.
  /// </summary>
  [Fact]
  public async Task KSailDebug_GivenNoKubeconfigPath_Fails()
  {
    Console.WriteLine(ResourceManager.GetString(nameof(KSailDebug_GivenNoKubeconfigPath_Fails), CultureInfo.InvariantCulture));
    //Arrange
    var console = new TestConsole();
    var ksailDebugCommand = new KSailDebugCommand();

    //Act
    try
    {
      _ = await ksailDebugCommand.InvokeAsync("--kubeconfig ", console);
    }
    catch (InvalidOperationException exception)
    {
      //Assert
      _ = await Verify(exception.Message);
    }
  }

  /// <summary>
  /// Tests that the <c>ksail debug</c> command succeeds when given a valid kubeconfig path.
  /// </summary>
  [Fact]
  public async Task KSailDebug_GivenValidKubeconfigPathAndInvalidContext_Fails()
  {
    Console.WriteLine(ResourceManager.GetString(nameof(KSailDebug_GivenValidKubeconfigPathAndInvalidContext_Fails), CultureInfo.InvariantCulture));
    //Arrange
    var console = new TestConsole();
    var ksailDebugCommand = new KSailDebugCommand();

    //Act
    int exitCode = await ksailDebugCommand.InvokeAsync("--context ksail", console);

    //Assert
    Assert.Equal(1, exitCode);
  }
}
