using KSail.Generators.Models;

namespace KSail.Generators;

static class Template
{
  internal static async Task<string> RenderAsync(string templatePath, IModel model)
  {
    string templateFile = await File.ReadAllTextAsync(templatePath);
    Console.WriteLine(templateFile);
    var template = Scriban.Template.Parse(templateFile);
    Console.WriteLine(template.Render(model as FluxKustomization));
    return await template.RenderAsync(model);
  }
}
