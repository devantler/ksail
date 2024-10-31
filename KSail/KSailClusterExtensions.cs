using KSail.Models;

namespace KSail;

/// <summary>
/// Extension methods for the <see cref="KSailCluster"/> class.
/// </summary>
public static class KSailClusterExtensions
{
  /// <summary>
  /// Updates a property on the <see cref="KSailCluster"/> object.
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="config"></param>
  /// <param name="propertyPath"></param>
  /// <param name="value"></param>
  /// <exception cref="ArgumentException"></exception>
  public static void UpdateConfig<T>(this KSailCluster config, string propertyPath, T value)
  {
    ArgumentNullException.ThrowIfNull(config);
    if (string.IsNullOrEmpty(propertyPath)) throw new ArgumentException("Property path cannot be null or empty.", nameof(propertyPath));

    string[] properties = propertyPath.Split('.');
    object? currentObject = config;

    for (int i = 0; i < properties.Length; i++)
    {
      string propertyName = properties[i];
      var property = (currentObject?.GetType().GetProperty(propertyName)) ?? throw new ArgumentException($"Property '{propertyName}' not found on type '{currentObject?.GetType().FullName}'.");
      if (i == properties.Length - 1)
      {
        // If it's the last property in the path, set the value
        object? currentValue = property.GetValue(currentObject);
        if (!Equals(currentValue, value))
        {
          property.SetValue(currentObject, value);
        }
      }
      else
      {
        // Traverse to the next object in the path
        object? nextObject = property.GetValue(currentObject);
        if (nextObject == null)
        {
          // Initialize the property if it's null
          nextObject = Activator.CreateInstance(property.PropertyType);
          property.SetValue(currentObject, nextObject);
        }
        currentObject = nextObject;
      }
    }
  }
}
