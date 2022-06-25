using Microsoft.AspNetCore.Authorization;
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

    [Authorize]
    public class PlayListController : Controller
    {
        private UserManager<IdentityUser> UserManager { get; set; }

        private IItemLibraryRepository ItemLibraryRepository { get; set; }

        private IYoutubeDataRepository YoutubeDataRepository { get; set; }

        private IPlayListRepository PlayListRepository { get; set; }

        private IConfiguration Configuration { get; set; }

        public PlayListController(UserManager<IdentityUser> userManager,
            IItemLibraryRepository itemLibraryRepository,
            IYoutubeDataRepository youtubeDataRepository,
            IPlayListRepository playListRepository,
            IConfiguration configurationManager)
        {
            UserManager = userManager;
            ItemLibraryRepository = itemLibraryRepository;
            YoutubeDataRepository = youtubeDataRepository;
            Configuration = configurationManager;
            PlayListRepository = playListRepository;
        }

        #region Index Action

        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var user = await UserManager.GetUserAsync(User);
            var model = new ManagePlayListViewModel(PlayListRepository,user);
            await model.Initialize();

            return View(model);
        }

        #endregion

        #region AddItem Action

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
                if (String.IsNullOrEmpty(model.Url))
                {
                    model.ErrorMessage = "URLを入力してください。";
                    return View(model);
                }

                YoutubeVideoRetrieveOperationResult? item;
                item = await YoutubeDataRepository.GetYoutubeVideoInfoAsync(model.Url);

                if (item.OperationResult == YoutubeAPIRetrieveOperationResultType.UnExpectedError)
                {
                    throw new ApplicationException("予期せぬエラーが発生しました。", item.Exception);
                }

                if (item.OperationResult != YoutubeAPIRetrieveOperationResultType.Success)
                {
                    model.ErrorMessage = item.Exception.Message;
                    return View(model);
                }

                //取得したデータをライブラリに追加
                var loginUserInfo = await UserManager.GetUserAsync(User);

                //入力されている場合、タイトルはそちらを使用
                await ItemLibraryRepository.InsertAsync(model.Platform,
                    item.RetrieveResult[0].ItemId,
                    !String.IsNullOrEmpty(model.TitleAlias) ? model.TitleAlias : item.RetrieveResult[0].Title,
                    loginUserInfo);

            }
            catch (Exception ex)
            {
                model.ErrorMessage = "追加に失敗しました。";
                return View(model);
            }

            return Redirect("PlayList");
        }

        #endregion

        #region CreatePlayList

        [HttpGet]
        public async Task<IActionResult> CreatePlayList()
        {
            var model = new CreateEditDeletePlayListViewModel();

            //プレイリスト一覧を取得
            var user = await UserManager.GetUserAsync(User);
            var playList = await ItemLibraryRepository.GetAllAsync(user);
            model.Libraries = playList.ToList();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePlayList(CreateEditDeletePlayListViewModel model)
        {
            if (ModelState.IsValid)
            {
                //URLが選択されているかどうかを確認
                if(model.SelectedLibraryInfo.Count == 0)
                {
                    model.ErrorMessage = "プレイリストに追加するアイテムを選択してください。";
                    return View(model);
                }

                //プレイリストを作成
                var user = await UserManager.GetUserAsync(User);
                var resultHeader = await PlayListRepository.CreateNewPlayListAsync(model.Title, user);

                if(resultHeader.OperationResult != PlayListOperationResultType.Success)
                {
                    //失敗
                }
                else
                {
                    //選択されているヘッダーIDから追加するライブラリを取得
                    var details = new List<ItemLibrariesEntity>();
                    foreach(var selected in model.SelectedLibraryInfo)
                    {
                        var libItem = model.Libraries.Where(lib => lib.ItemLibrariesEntityId == selected.ItemLibraryEntityId).First();
                        details.Add(libItem);
                    }

                    //プレイリストを作成
                    if(resultHeader.OperationResult != PlayListOperationResultType.Success)
                    {
                        //失敗
                    }
                    else
                    {
                        foreach (var detail in details)
                        {
                            var detailItem = new PlayListDetailsEntity()
                            {
                                PlayListHeadersEntityId = resultHeader.HeaderEntity!.PlayListHeadersEntityId,
                                ItemLibrariesEntityId = detail.ItemLibrariesEntityId,
                                ItemSeq = 0,
                            };
                            await PlayListRepository.AddItemToPlayListAsync(resultHeader.HeaderEntity.PlayListHeadersEntityId, detailItem, user);
                        }
                    }

                    return Redirect("PlayList");

                }

            }
            else
            {
                //入力不備あり
                if (String.IsNullOrEmpty(model.Title))
                {
                    model.ErrorMessage = "プレイリストのタイトルを入力してください。";
                    return View(model);
                }
            }

            return View(model);
        }


        #endregion

        #region EditPlayList

        [HttpGet]
        public async Task<IActionResult> EditPlayList(string playListHeaderId)
        {
            var model = new CreateEditDeletePlayListViewModel();

            var user = await UserManager.GetUserAsync(User);
            await model.SetPlayList(playListHeaderId,ItemLibraryRepository, PlayListRepository, user);

            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> EditPlayList(CreateEditDeletePlayListViewModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.SelectedLibraryInfo.Count() == 0)
                {
                    model.ErrorMessage = "プレイリストに追加するアイテムを選択してください。";
                    return View(model);
                }

                //プレイリストに登録


            }
            else
            {
                //入力不備あり
                if (String.IsNullOrEmpty(model.Title))
                {
                    model.ErrorMessage = "プレイリストのタイトルを入力してください。";
                    return View(model);
                }
            }



            return Redirect("PlayList");
        }


        #endregion

        [HttpGet]
        public IActionResult Create()
        {
            var model = new CreateEditDeletePlayListViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateEditDeletePlayListViewModel model)
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
        public async Task<IActionResult> Edit(CreateEditDeletePlayListViewModel model)
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
            var model = new CreateEditDeletePlayListViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddFromPlayList(CreateEditDeletePlayListViewModel model)
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

       


    }
}
