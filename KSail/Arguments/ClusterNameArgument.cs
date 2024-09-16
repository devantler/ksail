using System.CommandLine;

namespace KSail.Arguments;

sealed class ClusterNameArgument() : Argument<string>("name", "The name of the cluster.")
{
}
