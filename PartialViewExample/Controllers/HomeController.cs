using Microsoft.AspNetCore.Mvc;

namespace PartialViewExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("/")]
        public IActionResult Index()
        {
            ViewData["ListTitle"] = "City";
            ViewData["ListItem"] = new List<string>()
            {
                "Paris",
                "New York",
                "Los Angles",
                "Palm Beach",
            };
            return View();
        }
        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [Route("ReturnPartial")]
        public IActionResult ReturnPartial()
        {
            ViewData["ListTitle"] = "City";
            ViewData["ListItem"] = new List<string>()
            {
                "Paris",
                "New York",
                "Los Angles",
                "Palm Beach",
            };
            return PartialView("_ListPartialView");
        }
    }
}
