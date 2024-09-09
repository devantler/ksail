namespace KSail.Commands.Gen.Handlers;

class KSailGenCommandHandler : ICommandHandler
{
  public Task<int> HandleAsync(CancellationToken cancellationToken = default) => Task.FromResult(0);
}
