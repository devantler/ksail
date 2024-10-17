using Devantler.KubernetesGenerator.KSail.Models;

static class KSailClusterExtensions
{
  internal static Task SetConfigValueAsync<T>(this KSailCluster config, string propertyPath, T value)
  {
    string[] properties = propertyPath.Split('.');

    object currentObject = config;

    for (int i = 0; i < properties.Length; i++)
    {
      string propertyName = properties[i];
      var propertyInfo = (currentObject?.GetType().GetProperty(propertyName)) ?? throw new ArgumentException($"Property '{propertyName}' not found on type '{currentObject?.GetType().FullName}'.");

      if (i == properties.Length - 1)
      {
        propertyInfo.SetValue(currentObject, value);
      }
      else
      {
        object? propertyValue = propertyInfo.GetValue(currentObject);
        if (propertyValue == null)
        {
          propertyValue = Activator.CreateInstance(propertyInfo.PropertyType);
          propertyInfo.SetValue(currentObject, propertyValue);
        }
        currentObject = propertyValue ?? throw new InvalidOperationException($"Failed to create an instance of '{propertyInfo.PropertyType.FullName}'.");
      }
    }

    return Task.CompletedTask;
  }
}
