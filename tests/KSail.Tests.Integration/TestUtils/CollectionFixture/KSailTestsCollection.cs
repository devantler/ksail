#pragma warning disable CA1711
namespace KSail.Tests.Integration.TestUtils.CollectionFixture;

/// <summary>
/// A collection definition for KSail tests.
/// </summary>
[CollectionDefinition("KSail Tests Collection")]
public class KSailTestsCollection : ICollectionFixture<KSailClassFixture>
{
  // This class has no code, and is never created. Its purpose is simply
  // to be the place to apply [CollectionDefinition] and all the
  // ICollectionFixture<> interfaces.
}
