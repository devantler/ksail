namespace KSail.TemplateEngine;

interface ITemplateEngine
{
  public Task<string> RenderAsync(FileInfo template, object model);
  public Task<string> RenderAsync(string template, object model);
}
