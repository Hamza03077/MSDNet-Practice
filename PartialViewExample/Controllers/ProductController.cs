using Microsoft.AspNetCore.Mvc;

namespace PartialViewExample.Controllers
{
    public class ProductController : Controller
    {
        [Route("/Product/Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
