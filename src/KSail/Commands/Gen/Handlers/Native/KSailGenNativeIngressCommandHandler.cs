using Devantler.KubernetesGenerator.Native;
using k8s.Models;

namespace KSail.Commands.Gen.Handlers.Native;

class KSailGenNativeIngressCommandHandler
{
  readonly IngressGenerator _generator = new();
  internal async Task<int> HandleAsync(string outputFile, CancellationToken cancellationToken = default)
  {
    var model = new V1Ingress
    {
      ApiVersion = "networking.k8s.io/v1",
      Kind = "Ingress",
      Metadata = new V1ObjectMeta()
      {
        Name = "<name>",
      },
      Spec = new V1IngressSpec()
      {
        IngressClassName = "<ingressClassName>",
        Rules =
       [
         new V1IngressRule()
         {
           Host = "<host>",
           Http = new V1HTTPIngressRuleValue()
           {
             Paths =
             [
               new V1HTTPIngressPath()
               {
                 Path = "<path>",
                 PathType = "ImplementationSpecific",
                 Backend = new V1IngressBackend()
                 {
                   Service = new V1IngressServiceBackend()
                   {
                     Name = "<name>",
                     Port = new V1ServiceBackendPort()
                     {
                       Number = 0,
                     },
                   },
                 },
               },
             ],
           },
         },
       ],
      }
    };
    await _generator.GenerateAsync(model, outputFile, cancellationToken: cancellationToken).ConfigureAwait(false);
    return 0;
  }
}
