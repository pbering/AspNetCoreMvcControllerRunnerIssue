using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreMvcControllerRunnerIssue.Test
{
    public static class HtmlHelperExtension
    {
        public static RenderViewComponentHtmlHelper Render(this IHtmlHelper helper)
        {
            return new RenderViewComponentHtmlHelper(helper);
        }
    }

    public class RenderViewComponentHtmlHelper
    {
        private readonly ViewComponentRunner _runner;
        private readonly ViewContext _viewContext;

        public RenderViewComponentHtmlHelper(IHtmlHelper htmlHelper)
        {
            _viewContext = htmlHelper.ViewContext;
            _runner = _viewContext.HttpContext.RequestServices.GetRequiredService<ViewComponentRunner>();
        }

        public async Task<HtmlString> RenderViewComponentAsync(string viewComponentName)
        {
            using (var writer = new StringWriter())
            {
                await _runner.Execute(viewComponentName, writer, _viewContext);

                return new HtmlString(writer.ToString());
            }
        }
    }
}