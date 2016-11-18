using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreMvcControllerRunnerIssue.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}