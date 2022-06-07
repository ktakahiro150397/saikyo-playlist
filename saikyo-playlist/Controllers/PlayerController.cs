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
        public IActionResult Play(string playListHeaderId)
        {
            var playListModel = new PlayListModel(ApplicationDbContext, playListHeaderId);
            return View("Index", playListModel);
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult AddPlayCount(string playListHeaderId,int itemSeq)
        {

            try
            {
                var detail = ApplicationDbContext.PlayListDetails
                        .Where(item => item.PlayListHeadersEntityId == playListHeaderId && item.ItemSeq == itemSeq)
                        .FirstOrDefault();

                if(detail == null)
                {
                    //データが見つからなかった
                    return NotFound();
                }

                //データの再生回数を1つ増やす
                detail.PlayCount += 1;
                ApplicationDbContext.SaveChanges();

            }catch(Exception ex)
            {
                //内部エラー
                return BadRequest();
            }

            return Ok();

        }

    }
}
