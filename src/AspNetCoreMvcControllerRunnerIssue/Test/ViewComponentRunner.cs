using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace AspNetCoreMvcControllerRunnerIssue.Test
{
    public class ViewComponentRunner
    {
        public async Task Execute(string viewComponentName, TextWriter writer, ViewContext viewContext)
        {
            var componentResult = new ViewComponentResult
            {
                ViewComponentName = viewComponentName,
                Arguments = new { maxPriority = 3 },
                ViewData = viewContext.ViewData,
                TempData = viewContext.TempData
            };

            // Save current response body so we can restore it later
            var originalResponseBody = viewContext.HttpContext.Response.Body;

            try
            {
                using (var outputStream = new MemoryStream())
                {
                    // Set our new stream as response body so we can read it
                    viewContext.HttpContext.Response.Body = outputStream;

                    // Invoke component
                    await componentResult.ExecuteResultAsync(viewContext);

                    outputStream.Position = 0;

                    // Write the body data to the text writer
                    using (var reader = new StreamReader(outputStream))
                    {
                        var output = await reader.ReadToEndAsync();

                        await writer.WriteAsync(output);
                    }
                }
            }
            finally
            {
                // Restore the current response body
                viewContext.HttpContext.Response.Body = originalResponseBody;
            }
        }
    }
}