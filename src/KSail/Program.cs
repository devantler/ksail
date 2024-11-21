# pragma warning disable CA1031
using System.CommandLine;
using System.Runtime.InteropServices;
using KSail.Commands.Root;


if (!RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
{
  string runtime = RuntimeInformation.RuntimeIdentifier;
  string ageKeygenBinary = Path.Combine(AppContext.BaseDirectory, "age-keygen-" + runtime);
  string fluxBinary = Path.Combine(AppContext.BaseDirectory, "flux-" + runtime);
  string k3dBinary = Path.Combine(AppContext.BaseDirectory, "k3d-" + runtime);
  string k9sBinary = Path.Combine(AppContext.BaseDirectory, "k9s-" + runtime);
  string kindBinary = Path.Combine(AppContext.BaseDirectory, "kind-" + runtime);
  string kubeconformBinary = Path.Combine(AppContext.BaseDirectory, "kubeconform-" + runtime);
  string kustomizeBinary = Path.Combine(AppContext.BaseDirectory, "kustomize-" + runtime);
  //string sopsBinary = Path.Combine(AppContext.BaseDirectory, "sops-" + runtime);
  File.SetUnixFileMode(ageKeygenBinary, UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute);
  File.SetUnixFileMode(fluxBinary, UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute);
  File.SetUnixFileMode(k3dBinary, UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute);
  File.SetUnixFileMode(k9sBinary, UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute);
  File.SetUnixFileMode(kindBinary, UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute);
  File.SetUnixFileMode(kubeconformBinary, UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute);
  File.SetUnixFileMode(kustomizeBinary, UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute);
  //File.SetUnixFileMode(sopsBinary, UnixFileMode.UserExecute | UnixFileMode.GroupExecute | UnixFileMode.OtherExecute);
}
var ksailCommand = new KSailRootCommand();
return await ksailCommand.InvokeAsync(args);
