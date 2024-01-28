using KSail.Enums;

namespace KSail.Services.Provisioners.ContainerOrchestrator;

interface IContainerOrchestratorProvisioner
{
  Task<ContainerOrchestratorType> GetContainerOrchestratorTypeAsync();
  Task CreateNamespaceAsync(string context, string name);
  Task CreateSecretAsync(string context, string name, Dictionary<string, string> data, string @namespace = "default");
}
