namespace KSail;

/// <summary>
/// A handler for a command.
/// </summary>
public interface ICommandHandler
{
  /// <summary>
  /// Handle the command.
  /// </summary>
  /// <param name="cancellationToken"></param>
  Task<int> HandleAsync(CancellationToken cancellationToken = default);
}
