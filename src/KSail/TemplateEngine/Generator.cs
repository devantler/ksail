using System.Text;

namespace KSail.TemplateEngine;

class Generator(ITemplateEngine templateEngine) : IGenerator
{
  readonly ITemplateEngine _templateEngine = templateEngine;

  public Task<string> GenerateAsync(string templatePath, object model) =>
    _templateEngine.RenderAsync(new FileInfo(templatePath), model);

  public async Task GenerateAsync(
    string outputPath,
    string templatePath,
    object model,
    FileMode fileMode = FileMode.CreateNew
  )
  {
    string directoryName = Path.GetDirectoryName(outputPath) ?? throw new ArgumentNullException(nameof(outputPath));
    if (!Directory.Exists(directoryName))
      _ = Directory.CreateDirectory(directoryName);

    var fileStream = new FileStream(outputPath, fileMode, FileAccess.Write);
    await fileStream.WriteAsync(Encoding.UTF8.GetBytes(await _templateEngine.RenderAsync(templatePath, model)));
    await fileStream.FlushAsync();
    fileStream.Close();
  }
}
