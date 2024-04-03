namespace KSail.Provisioners.ContainerOrchestrator;

interface IContainerOrchestratorProvisioner
{
  Task CreateNamespaceAsync(string context, string name);
  Task CreateSecretAsync(string context, string name, Dictionary<string, string> data, string @namespace = "default");
}
