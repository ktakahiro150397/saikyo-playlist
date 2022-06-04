using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using saikyo_playlist.Models;

namespace saikyo_playlist.Controllers
{

    [Authorize]
    public class PlayerController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            var PlayListModel = new PlayListModel();
            return View(PlayListModel);
        }
    }
}
