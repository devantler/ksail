namespace KSail.TemplateEngine;

interface IGenerator
{
  Task GenerateAsync(string outputPath, string templatePath, object model, FileMode fileMode = FileMode.CreateNew);
  Task<string> GenerateAsync(string templatePath, object model);
}
