

using saikyo_playlist.Data.Video;
using saikyo_playlist.Helpers;
using saikyo_playlist.Models.PlayListManage;
using saikyo_playlist.Repository.Implements;

namespace saikyo_playListTest.Controllers
{

    
    public class PlayListControllerTest
    {

        public Mock<UserManager<IdentityUser>> userManagerMoq;

        public Mock<IConfiguration> configMoq;

        public Mock<IItemLibraryRepository> itemLibRepo;

        public Mock<IYoutubeDataRepository> youtubeRepo;

        public Mock<IPlayListRepository> playlistRepo;

        public PlayListController controller;

        public PlayListControllerTest()
        {
            //moqを利用してコントローラーのインスタンスを作成
            //Arrange
            var store = new Mock<IUserStore<IdentityUser>>();
            userManagerMoq = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            configMoq = new Mock<IConfiguration>();
            itemLibRepo = new Mock<IItemLibraryRepository>();
            youtubeRepo = new Mock<IYoutubeDataRepository>();
            playlistRepo = new Mock<IPlayListRepository>();

            controller = new PlayListController(
                userManagerMoq.Object,
                itemLibRepo.Object,
                youtubeRepo.Object,
                playlistRepo.Object,
                configMoq.Object);
        }

        #region "Index Action"

        /// <summary>
        /// プレイリスト一覧　成功
        /// </summary>
        [Fact]
        public void Index_ReturnAViewWithModel()
        {
            //Arrange
            playlistRepo.Setup(repo => repo.GetPlayListHeaderAll())
                .ReturnsAsync(new List<PlayListHeadersEntity>()
                {
                    new PlayListHeadersEntity()
                    {
                        Name = "test_header_name_1",
                        AspNetUserdId = "test_user_id_1"
                    },
                    new PlayListHeadersEntity()
                    {
                        Name = "test_header_name_2",
                        AspNetUserdId = "test_user_id_2"
                    },
                })
                .Verifiable();


            //Act
            var actResult = controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(actResult.Result);
            var model = Assert.IsAssignableFrom<ManagePlayListViewModel>(viewResult.Model);
            Assert.Equal(2, model.managePlayListItems.Count);
            playlistRepo.Verify();
        }

        #endregion

        #region "AddItem Action"

        /// <summary>
        /// アイテムライブラリへの追加画面：GET
        /// </summary>
        [Fact]
        public void AddItem_ReturnsAViewResult()
        {
            //Arrange

            //Act
            var actResult = controller.AddItem();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(actResult);
            var model = Assert.IsAssignableFrom<AddItemViewModel>(viewResult.Model);

        }

        /// <summary>
        /// アイテムライブラリへの追加画面：POST・成功
        /// </summary>
        [Fact]
        public async Task AddItem_RedirectToIndexWithSuccess()
        {

            //Arrange
            itemLibRepo.Setup(repo => repo.InsertAsync(It.IsAny<LibraryItemPlatform>(), It.IsAny<string>(), It.IsAny<string>()))
                .Verifiable();
            youtubeRepo.Setup(repo => repo.GetYoutubeVideoInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(new YoutubeVideoRetrieveOperationResult() { OperationResult = YoutubeAPIRetrieveOperationResultType.Success })
                .Verifiable();

            var model = new AddItemViewModel()
            {
                Url = "https://www.youtube.com?v=aaa",
                TitleAlias = "テスト",
                Platform = LibraryItemPlatform.Youtube,
                ErrorMessage = ""
            };

            //Act
            var actResult = await controller.AddItem(model);

            //Assert
            var viewResult = Assert.IsType<RedirectResult>(actResult);
            Assert.Equal("/PlayList", viewResult.Url);
            itemLibRepo.Verify();
            youtubeRepo.Verify();

        }

        /// <summary>
        /// アイテムライブラリへの追加画面：POST・失敗・URLなし
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddItem_ReturnAViewWithInvalidModelState_NoUrl()
        {
            //Arrange
            itemLibRepo.Setup(repo => repo.InsertAsync(It.IsAny<LibraryItemPlatform>(), It.IsAny<string>(), It.IsAny<string>()))
                .Verifiable();
            youtubeRepo.Setup(repo => repo.GetYoutubeVideoInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(new YoutubeVideoRetrieveOperationResult() { OperationResult = YoutubeAPIRetrieveOperationResultType.Success })
                .Verifiable();

            var model = new AddItemViewModel()
            {
                Url = "",
                TitleAlias = "テスト",
                Platform = LibraryItemPlatform.Youtube,
                ErrorMessage = ""
            };
            controller.ModelState.AddModelError("Url", "required");

            //Act
            var actResult = await controller.AddItem(model);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(actResult);
            var modelResult = Assert.IsAssignableFrom<AddItemViewModel>(viewResult.Model);
            Assert.Equal("URLを入力してください。", modelResult.ErrorMessage);
           
        }

        /// <summary>
        /// アイテムライブラリへの追加画面：POST・失敗・動画URLからYoutubeデータ取得不可
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddItem_ReturnAViewWithInvalidModelState_NoDataFromVideoUrl()
        {
            //Arrange
            youtubeRepo.Setup(repo => repo.GetYoutubeVideoInfoAsync(It.IsAny<string>()))
                .Verifiable();

            var model = new AddItemViewModel()
            {
                Url = "https://www.youtube.com?v=aaa",
                TitleAlias = "テスト",
                Platform = LibraryItemPlatform.Youtube,
                ErrorMessage = ""
            };

            //Act
            var actResult = await controller.AddItem(model);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(actResult);
            var modelResult = Assert.IsAssignableFrom<AddItemViewModel>(viewResult.Model);
            Assert.Equal($"URL:「{model.Url}」から動画情報が取得できませんでした。", modelResult.ErrorMessage);

        }

        /// <summary>
        /// アイテムライブラリへの追加画面：POST・失敗・動画URLのIDが不正
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddItem_ReturnAViewWithInvalidModelState_InvalidVideoUrl()
        {
            //Arrange
            youtubeRepo.Setup(repo => repo.GetYoutubeVideoInfoAsync(It.IsAny<string>()))
                .Verifiable();

            var model = new AddItemViewModel()
            {
                Url = "hogehoge",
                TitleAlias = "テスト",
                Platform = LibraryItemPlatform.Youtube,
                ErrorMessage = ""
            };

            //Act
            var actResult = await controller.AddItem(model);

            //Assert
            var viewResult = Assert.IsType<ViewResult>(actResult);
            var modelResult = Assert.IsAssignableFrom<AddItemViewModel>(viewResult.Model);
            Assert.Equal($"URL:「{model.Url}」から動画IDが取得できませんでした。", modelResult.ErrorMessage);

        }

        #endregion

    }


}
