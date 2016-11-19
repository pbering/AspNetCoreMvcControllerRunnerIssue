using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        private static readonly ViewComponentRunner _runner = new ViewComponentRunner();
        private readonly ViewContext _viewContext;

        public RenderViewComponentHtmlHelper(IHtmlHelper htmlHelper)
        {
            _viewContext = htmlHelper.ViewContext;
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