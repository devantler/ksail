using KSail.Generators.Models;

namespace KSail.Generators;

static class Template
{
  internal static async Task<string> RenderAsync(string templatePath, IModel model)
  {
    string templateFile = await File.ReadAllTextAsync(templatePath);
    var template = Scriban.Template.Parse(templateFile);
    return await template.RenderAsync(model);
  }
}
