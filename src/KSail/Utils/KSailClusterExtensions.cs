using System.Linq.Expressions;
using KSail.Models;

namespace KSail.Utils;


static class KSailClusterExtensions
{
  public static void UpdateConfig<T>(this KSailCluster config, Expression<Func<KSailCluster, T>> propertyExpression, T value)
  {
    var memberExpression = propertyExpression.Body as MemberExpression ?? throw new ArgumentException("Expression must be a member expression");
    var propertyPath = new Stack<string>();

    while (memberExpression != null)
    {
      propertyPath.Push(memberExpression.Member.Name);
      memberExpression = memberExpression.Expression as MemberExpression;
    }

    object? currentObject = config;
    object? defaultObject = new KSailCluster();

    while (propertyPath.Count > 0)
    {
      string propertyName = propertyPath.Pop();
      var property = (currentObject?.GetType().GetProperty(propertyName)) ?? throw new ArgumentException($"Property '{propertyName}' not found on type '{currentObject?.GetType().FullName}'.");
      var defaultProperty = (defaultObject?.GetType().GetProperty(propertyName)) ?? throw new ArgumentException($"Property '{propertyName}' not found on type '{defaultObject?.GetType().FullName}'.");

      if (propertyPath.Count == 0)
      {
        // If it's the last property in the path, set the value
        object? currentValue = property.GetValue(currentObject);
        object? defaultValue = defaultProperty.GetValue(defaultObject);

        if (value != null && !Equals(currentValue, value) && !Equals(value, defaultValue))
        {
          if (value is IEnumerable<string> enumerableValue && !enumerableValue.Any())
            continue;
          property.SetValue(currentObject, value);
        }
      }
      else
      {
        // Traverse to the next object in the path
        object? nextObject = property.GetValue(currentObject);
        object? nextDefaultObject = defaultProperty.GetValue(defaultObject);
        nextObject ??= Activator.CreateInstance(property.PropertyType);
        nextDefaultObject ??= Activator.CreateInstance(defaultProperty.PropertyType);
        property.SetValue(currentObject, nextObject);
        property.SetValue(defaultObject, nextDefaultObject);
        currentObject = nextObject;
        defaultObject = nextDefaultObject;
      }
    }
  }
}
