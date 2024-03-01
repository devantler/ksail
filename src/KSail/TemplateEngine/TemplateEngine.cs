namespace KSail.TemplateEngine;

class TemplateEngine : ITemplateEngine
{
  public async Task<string> RenderAsync(FileInfo template, object model)
  {
    string templateFile = await File.ReadAllTextAsync(template.FullName);
    var parsedTemplate = Scriban.Template.Parse(templateFile);
    return await parsedTemplate.RenderAsync(model);
  }

  public async Task<string> RenderAsync(string template, object model)
  {
    var parsedTemplate = Scriban.Template.Parse(template);
    return await parsedTemplate.RenderAsync(model);
  }
}
