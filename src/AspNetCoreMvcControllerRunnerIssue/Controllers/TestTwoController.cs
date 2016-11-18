using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvcControllerRunnerIssue.Controllers
{
    public class TestTwoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}