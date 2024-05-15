using Microsoft.AspNetCore.Mvc;

namespace Core_Prac_01.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
