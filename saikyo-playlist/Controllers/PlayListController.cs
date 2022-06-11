using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using saikyo_playlist.Data;
using saikyo_playlist.Data.Video;
using saikyo_playlist.Helpers;
using saikyo_playlist.Models;
using saikyo_playlist.Models.PlayListManage;
using saikyo_playlist.Repository;
using saikyo_playlist.Repository.Implements;
using saikyo_playlist.Repository.Interfaces;

namespace saikyo_playlist.Controllers
{
    public class PlayListController : Controller
    {
        private UserManager<IdentityUser> UserManager { get; set; }

        private IItemLibraryRepository ItemLibraryRepository { get; set; }


        private IConfiguration Configuration { get; set; }

        public PlayListController(UserManager<IdentityUser> userManager,
            IItemLibraryRepository itemLibraryRepository,
            IConfiguration configurationManager)
        {
            UserManager = userManager;
            ItemLibraryRepository = itemLibraryRepository;
            Configuration = configurationManager;
        }

        [HttpGet]
        public IActionResult Index()
        {

            //var model = new ManagePlayListViewModel(ApplicationDbContext);

            //return View(model);
            return View();
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

            //var loginUserInfo = await UserManager.GetUserAsync(User);

            //var playListRepo = new PlayListRepository(ApplicationDbContext, loginUserInfo, Configuration["YoutubeAPIKey"]);
            //var createResult = await playListRepo.CreateNewPlayListAsync(model.Title, model.Urls);

            //if (createResult)
            //{
            //    //登録成功
            //    var indexModel = new ManagePlayListViewModel(ApplicationDbContext);
            //    return View("Index", indexModel);

            //}
            //else
            //{
            //    //登録失敗
            //    return View(model);
            //}
            return View();

        }

        [HttpGet]
        public IActionResult Edit(string playListHeaderId)
        {
            //TODO : ライブラリIDの管理を行うよう修正する


            ////対象IDのデータを取得
            //var playListData = ApplicationDbContext.PlayListHeaders
            //    .Where(item => item.PlayListHeadersEntityId == playListHeaderId)
            //    .Join(
            //        ApplicationDbContext.PlayListDetails,
            //        header => header.PlayListHeadersEntityId,
            //        detail => detail.PlayListHeadersEntityId,
            //        (header, detail) =>
            //            new
            //            {
            //                Name = header.Name,
            //                Details = header.Details
            //            }
            //    ).FirstOrDefault();

            //if (playListData == null)
            //{
            //    return NotFound();
            //}
            //else
            //{

            //    var model = new CreatePlayListViewModel();
            //    model.PlayListHeaderId = playListHeaderId;

            //    model.Title = playListData.Name;

            //    //model.Urls = String.Join("", playListData.Details.Select(item => @"https://www.youtube.com/watch?v=" + item.ItemId + "," + item.Title + "," + item.TitleAlias + Environment.NewLine));

            //    return View(model);

            //}
            return View();


        }

        [HttpPost]
        public async Task<IActionResult> Edit(CreatePlayListViewModel model)
        {
            //var loginUserInfo = await UserManager.GetUserAsync(User);

            //var playListRepo = new PlayListRepository(ApplicationDbContext, loginUserInfo, Configuration["YoutubeAPIKey"]);
            //var createResult = await playListRepo.UpdateExistPlayListAsync(model.PlayListHeaderId, model.Title, model.Urls);

            //if (createResult)
            //{
            //    //登録成功
            //    var indexModel = new ManagePlayListViewModel(ApplicationDbContext);
            //    return View("Index", indexModel);

            //}
            //else
            //{
            //    //登録失敗
            //    return View(model);
            //}
            return View();
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
            //var loginUserInfo = await UserManager.GetUserAsync(User);

            //var playListRepo = new PlayListRepository(ApplicationDbContext, loginUserInfo, Configuration["YoutubeAPIKey"]);
            //var createResult = await playListRepo.CreateNewPlayListFromPlayListUrlAsync(model.Title, model.PlayListUrl);

            //if (createResult)
            //{
            //    //登録成功
            //    var indexModel = new ManagePlayListViewModel(ApplicationDbContext);
            //    return View("Index", indexModel);

            //}
            //else
            //{
            //    //登録失敗
            //    return View(model);
            //}
            return View();
        }

        [HttpGet]
        public IActionResult AddItem()
        {
            return View(new AddItemViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddItem(AddItemViewModel model)
        {
            try
            {
                //プラットフォームを判別し、データを取得する
                var repo = new YoutubeDataRepository(Configuration["YoutubeAPIKey"]);

                var videoId = YoutubeHelpers.GetVideoIdFromUrl(model.Url);
                YoutubeVideoRetrieveResult? item;
                if (videoId != null)
                {
                    item = await repo.GetYoutubeVideoInfo(videoId);

                    if(item == null)
                    {
                        throw new ApplicationException($"URL:「{model.Url}」から動画情報が取得できませんでした。");
                    }
                }
                else
                {
                    throw new ApplicationException($"URL:「{model.Url}」から動画IDが取得できませんでした。");
                }

                //取得したデータをライブラリに追加
                var loginUserInfo = await UserManager.GetUserAsync(User);

                //入力されている場合、タイトルはそちらを使用
                await ItemLibraryRepository.InsertOrRetrieveAsync(model.Platform, item.ItemId, model.TitleAlias != "" ? model.TitleAlias : item.Title);

            }catch (Exception ex)
            {
                model.ErrorMessage = "追加に失敗しました。";
                return View(model);
            }

            return Redirect("./PlayList");
        }


    }
}
