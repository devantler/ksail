using System.ComponentModel;

namespace KSail.Models.Template;


public class KSailTemplateKustomize
{

  [Description("The root directory.")]
  public string Root { get; set; } = "k8s/clusters/ksail-default/flux-system";
}
