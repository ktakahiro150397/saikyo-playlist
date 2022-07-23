using Microsoft.AspNetCore.Mvc;

namespace saikyo_playlist.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
