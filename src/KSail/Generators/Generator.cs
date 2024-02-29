using System.Text;

namespace KSail.Generators;

static class Generator
{
  internal static async Task GenerateAsync(string outputPath, string templatePath, object model, FileMode fileMode = FileMode.CreateNew)
  {
    string directoryName = Path.GetDirectoryName(outputPath) ?? throw new ArgumentNullException(nameof(outputPath));
    if (!Directory.Exists(directoryName))
    {
      _ = Directory.CreateDirectory(directoryName);
    }

    var fileStream = new FileStream(outputPath, fileMode, FileAccess.Write);
    await fileStream.WriteAsync(Encoding.UTF8.GetBytes(await TemplateEngine.RenderAsync(templatePath, model)));
    await fileStream.FlushAsync();
    fileStream.Close();
  }
}
