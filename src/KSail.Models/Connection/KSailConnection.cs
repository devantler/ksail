using System.ComponentModel;

namespace KSail.Models.Connection;


public class KSailConnection
{

  [Description("The path to the kubeconfig file.")]
  public string Kubeconfig { get; set; } = "~/.kube/config";


  [Description("The kube context.")]
  public string Context { get; set; } = "kind-ksail-default";


  [Description("The timeout for operations (10s, 5m, 1h).")]
  public string Timeout { get; set; } = "5m";
}
