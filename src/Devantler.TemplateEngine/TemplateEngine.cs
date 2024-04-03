namespace KSail.TemplateEngine;

class TemplateEngine : ITemplateEngine
{
  public async Task<string> RenderFromPathAsync(string templatePath, object model)
  {
    string templateFile = await File.ReadAllTextAsync(templatePath);
    var parsedTemplate = Scriban.Template.Parse(templateFile);
    return await parsedTemplate.RenderAsync(model);
  }

  public async Task<string> RenderFromContentAsync(string templateContent, object model)
  {
    var parsedTemplate = Scriban.Template.Parse(templateContent);
    return await parsedTemplate.RenderAsync(model);
  }
}
