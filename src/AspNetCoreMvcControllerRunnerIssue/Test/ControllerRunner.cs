using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;

namespace AspNetCoreMvcControllerRunnerIssue.Test
{
    public class ControllerRunner
    {
        private readonly IActionSelector _selector;
        private readonly IActionInvokerFactory _invokerFactory;

        public ControllerRunner(IActionInvokerFactory invokerFactory, IActionSelector selector)
        {
            _invokerFactory = invokerFactory;
            _selector = selector;
        }

        public async Task Execute(HttpContext httpContext, string controller, string action, TextWriter writer)
        {
            var routeContext = new RouteContext(httpContext);

            routeContext.RouteData.Values["controller"] = controller;
            routeContext.RouteData.Values["action"] = action;

            var candidates = _selector.SelectCandidates(routeContext);

            if (candidates == null || candidates.Count == 0)
            {
                throw new Exception("Action selector did not find any candidates.");
            }

            var descriptor = _selector.SelectBestCandidate(routeContext, candidates);

            if (descriptor == null)
            {
                throw new Exception("Action selector dit not find any action descriptor.");
            }

            var invoker = _invokerFactory.CreateInvoker(new ActionContext(routeContext.HttpContext, routeContext.RouteData, descriptor));

            if (invoker == null)
            {
                throw new Exception("Action invoker was null.");
            }

            // Save current response body so we can restore it later
            var currentOutputStream = routeContext.HttpContext.Response.Body;

            using (var outputStream = new MemoryStream())
            {
                // Set our new stream as response body
                routeContext.HttpContext.Response.Body = outputStream;

                try
                {
                    // Invoke controller
                    await invoker.InvokeAsync();

                    // Write the body data to the text writer
                    outputStream.Position = 0;

                    using (var reader = new StreamReader(outputStream))
                    {
                        var output = await reader.ReadToEndAsync();

                        await writer.WriteLineAsync(output);
                    }
                }
                finally
                {
                    // Restore the current response body
                    routeContext.HttpContext.Response.Body = currentOutputStream;
                }
            }
        }
    }
}