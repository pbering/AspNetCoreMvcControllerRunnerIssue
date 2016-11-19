using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvcControllerRunnerIssue.Components
{
    public class TestOne : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int maxPriority)
        {
            return View();
        }
    }
}