namespace KSail.TemplateEngine;

interface ITemplateEngine
{
  public Task<string> RenderFromPathAsync(string templatePath, object model);
  public Task<string> RenderFromContentAsync(string templateContent, object model);
}
