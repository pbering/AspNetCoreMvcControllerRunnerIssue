using System.Threading.Tasks;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewComponents;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetCoreMvcControllerRunnerIssue.Test
{
    public static class HtmlHelperExtension
    {
        public static async Task<IHtmlContent> RenderPlaceholderAsync(this IHtmlHelper htmlHelper)
        {
            var helper = (DefaultViewComponentHelper)htmlHelper.ViewContext.HttpContext.RequestServices.GetRequiredService<IViewComponentHelper>();

            helper.Contextualize(htmlHelper.ViewContext);

            var builder = new HtmlContentBuilder();

            foreach (var component in new[] {"TestOne", "TestTwo"})
            {
                builder.AppendHtml(await helper.InvokeAsync(component));
            }

            return builder;
        }
    }
}