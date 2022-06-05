using Microsoft.AspNetCore.Mvc;
using saikyo_playlist.Data;
using saikyo_playlist.Models.PlayListManage;

namespace saikyo_playlist.Controllers
{
    public class PlayListController : Controller
    {
        private ApplicationDbContext ApplicationDbContext { get; set; }

        public PlayListController(ApplicationDbContext dbContext)
        {
            ApplicationDbContext = dbContext;
        }

        [HttpGet]
        public IActionResult Index()
        {

            var model = new ManagePlayListViewModel(ApplicationDbContext);

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreatePlayListViewModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Create(CreatePlayListViewModel model)
        {




            return View("Index");
        }
    }
}
