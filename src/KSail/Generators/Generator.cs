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

    string renderedFile = await TemplateEngine.RenderAsync(templatePath, model);
    Console.WriteLine(renderedFile);
    var fileStream = new FileStream(outputPath, FileMode.CreateNew, FileAccess.Write);
    await fileStream.WriteAsync(Encoding.UTF8.GetBytes(renderedFile));
    await fileStream.FlushAsync();
    fileStream.Close();
  }
}
