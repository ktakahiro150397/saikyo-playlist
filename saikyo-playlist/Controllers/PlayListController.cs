using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using saikyo_playlist.Data;
using saikyo_playlist.Models.PlayListManage;
using saikyo_playlist.Repository;
using System.Security.Claims;

namespace saikyo_playlist.Controllers
{
    public class PlayListController : Controller
    {
        private ApplicationDbContext ApplicationDbContext { get; set; }

        private UserManager<IdentityUser> UserManager { get; set; }

        private SignInManager<IdentityUser> SignInManager { get; set; }



        public PlayListController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            ApplicationDbContext dbContext)
        {
            UserManager = userManager;
            SignInManager = signInManager;
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
        public async Task<IActionResult> Create(CreatePlayListViewModel model)
        {

            var loginUserInfo = await UserManager.GetUserAsync(User);

            var playListRepo = new PlayListRepository(ApplicationDbContext, loginUserInfo);
            var createResult = await playListRepo.CreateNewPlayListAsync(model.Title, model.Urls);

            if (createResult)
            {
                //登録成功
                var indexModel = new ManagePlayListViewModel(ApplicationDbContext);
                return View("Index", indexModel);

            }
            else
            {
                //登録失敗
                return View(model);
            }

        }

        [HttpGet]
        public IActionResult Edit(string playListHeaderId)
        {
            //対象IDのデータを取得
            var playListData = ApplicationDbContext.PlayListHeaders
                .Where(item => item.PlayListHeadersEntityId == playListHeaderId)
                .Join(
                    ApplicationDbContext.PlayListDetails,
                    header => header.PlayListHeadersEntityId,
                    detail => detail.PlayListHeadersEntityId,
                    (header, detail) =>
                        new
                        {
                            Name = header.Name,
                            Details = header.Details
                        }
                ).FirstOrDefault();

            if (playListData == null)
            {
                return NotFound();
            }
            else
            {

                var model = new CreatePlayListViewModel();
                model.PlayListHeaderId = playListHeaderId;

                model.Title = playListData.Name;

                model.Urls = String.Join("", playListData.Details.Select(item => @"https://www.youtube.com/watch?v=" + item.ItemId + "," + item.Title + "," + item.TitleAlias + Environment.NewLine));

                return View(model);

            }


        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreatePlayListViewModel model)
        {
            var loginUserInfo = await UserManager.GetUserAsync(User);

            var playListRepo = new PlayListRepository(ApplicationDbContext, loginUserInfo);
            var createResult = await playListRepo.UpdateExistPlayListAsync(model.PlayListHeaderId, model.Title, model.Urls);

            if (createResult)
            {
                //登録成功
                var indexModel = new ManagePlayListViewModel(ApplicationDbContext);
                return View("Index", indexModel);

            }
            else
            {
                //登録失敗
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult AddFromPlayList()
        {
            var model = new CreatePlayListViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddFromPlayList(CreatePlayListViewModel model)
        {
            var loginUserInfo = await UserManager.GetUserAsync(User);

            var playListRepo = new PlayListRepository(ApplicationDbContext, loginUserInfo);
            var createResult = await playListRepo.CreateNewPlayListFromPlayListUrlAsync(model.Title, model.PlayListUrl);

            if (createResult)
            {
                //登録成功
                var indexModel = new ManagePlayListViewModel(ApplicationDbContext);
                return View("Index", indexModel);

            }
            else
            {
                //登録失敗
                return View(model);
            }
        }


    }
}
