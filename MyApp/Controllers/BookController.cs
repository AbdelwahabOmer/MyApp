using Microsoft.AspNetCore.Mvc;

namespace MyApp.Controllers
{
    public class BookController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
