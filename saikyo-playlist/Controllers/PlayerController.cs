using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using saikyo_playlist.Data;
using saikyo_playlist.Models;
using saikyo_playlist.Repository.Interfaces;

namespace saikyo_playlist.Controllers
{

    [Authorize]
    public class PlayerController : Controller
    {

        private ApplicationDbContext ApplicationDbContext { get; set; }

        private IPlayListRepository PlayListRepository { get; set; }

        private IItemLibraryRepository ItemLibraryRepository { get; set; }

        public PlayerController(ApplicationDbContext dbContext,
            IPlayListRepository playListRepository,
            IItemLibraryRepository itemLibraryRepository)
        {
            ApplicationDbContext = dbContext;
            PlayListRepository = playListRepository;
            ItemLibraryRepository = itemLibraryRepository;
        }


        [HttpGet]
        public IActionResult Index()
        {
            var PlayListModel = new PlayListModel();
            return View(PlayListModel);
        }

        [HttpGet]
        public async Task<IActionResult> Play(string playListHeaderId)
        {
            //アクセス時、現在時刻を最終再生時間として更新
            await PlayListRepository.UpdatePlayListPlayTime(playListHeaderId, DateTime.Now);

            var playListModel = new PlayListModel(ApplicationDbContext, playListHeaderId);
            return View("Index", playListModel);
        }

        /// <summary>
        /// 指定したIDの再生回数を加算します。
        /// </summary>
        /// <param name="itemLibrariesEntityId"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> AddPlayCount(string itemLibrariesEntityId,int addCount = 1)
        {
            var ret = 0;
            try
            {
                var result = await ItemLibraryRepository.AddPlayCount(itemLibrariesEntityId, addCount);
            
                if(result.OperationResult != Repository.Implements.ItemLibraryOperationResultType.Success)
                {
                    return BadRequest();
                }
                else
                {
                    ret = result.PlayCount!.Value;
                }
                
            }
            catch (Exception ex)
            {
                //内部エラー
                return BadRequest();
            }

            // カウント後の再生回数を返す
            return Ok(new { playCount = ret });

        }

    }
}
