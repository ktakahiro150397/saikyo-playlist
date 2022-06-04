using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using saikyo_playlist.Data;
using saikyo_playlist.Models;

namespace saikyo_playlist.Controllers
{

    [Authorize]
    public class PlayerController : Controller
    {

        private ApplicationDbContext ApplicationDbContext { get; set; }

        public PlayerController(ApplicationDbContext dbContext)
        {
            ApplicationDbContext = dbContext;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var PlayListModel = new PlayListModel();
            return View(PlayListModel);
        }

        [HttpGet]
        public IActionResult IdTest()
        {
            var playListModel = new PlayListModel(ApplicationDbContext, "1");
            return View("Index", playListModel);
        }
    }
}
