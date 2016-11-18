using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvcControllerRunnerIssue.Controllers
{
    public class TestOneController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}