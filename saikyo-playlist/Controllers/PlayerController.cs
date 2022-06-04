using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using saikyo_playlist.Models;

namespace saikyo_playlist.Controllers
{

    [Authorize]
    public class PlayerController : Controller
    {
        public PlayerPlayListModel PlayListModel { get; set; }

        public PlayerController()
        {
            PlayListModel = new PlayerPlayListModel();
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(PlayListModel);
        }

        [HttpGet]
        public JsonResult GetNextPlayVideoId()
        {

            //再生中インデックスを加算し、次に再生する動作IDを返す
            var nextIndex = PlayListModel.GetNextVideoIndexAndIncrementIndex();
            return Json(new { nextId=PlayListModel.VideoId[nextIndex].Id });

        }


    }
}
