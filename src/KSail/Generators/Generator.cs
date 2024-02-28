using System.Text;
using KSail.Generators.Models;

namespace KSail.Generators;

static class Generator
{
  internal static async Task GenerateAsync(string outputPath, string templatePath, IModel model)
  {
    string directoryName = Path.GetDirectoryName(outputPath) ?? throw new ArgumentNullException(nameof(outputPath));
    if (!Directory.Exists(directoryName))
    {
      _ = Directory.CreateDirectory(directoryName);
    }

    var fileStream = new FileStream(outputPath, FileMode.CreateNew, FileAccess.Write);
    await fileStream.WriteAsync(Encoding.UTF8.GetBytes(await Template.RenderAsync(templatePath, model)));
    await fileStream.FlushAsync();
    fileStream.Close();
  }
}
