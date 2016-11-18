using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreMvcControllerRunnerIssue.Test
{
    public static class HtmlHelperExtension
    {
        public static RenderControllerHtmlHelper Render(this IHtmlHelper helper)
        {
            return new RenderControllerHtmlHelper(helper);
        }
    }

    public class RenderControllerHtmlHelper
    {
        private readonly HttpContext _context;
        private readonly ControllerRunner _runner;

        public RenderControllerHtmlHelper(IHtmlHelper htmlHelper)
        {
            _context = htmlHelper.ViewContext.HttpContext;
            _runner = _context.RequestServices.GetRequiredService<ControllerRunner>();
        }

        public async Task<HtmlString> RenderControllerAsync(string controller, string action)
        {
            using (var writer = new StringWriter())
            {
                await _runner.Execute(_context, controller, action, writer);

                return new HtmlString(writer.ToString());
            }
        }
    }
}