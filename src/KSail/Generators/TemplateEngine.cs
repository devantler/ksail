using KSail.Generators.Models;

namespace KSail.Generators;

static class TemplateEngine
{
  internal static async Task<string> RenderAsync(string templatePath, object model)
  {
    string templateFile = await File.ReadAllTextAsync(templatePath);
    var template = Scriban.Template.Parse(templateFile);
    return await template.RenderAsync(model);
  }
}
