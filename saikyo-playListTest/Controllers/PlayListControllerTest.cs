﻿

using saikyo_playlist.Data.Video;
using saikyo_playlist.Helpers;
using saikyo_playlist.Repository.Implements;

namespace saikyo_playListTest.Controllers
{

    
    public class PlayListControllerTest
    {

        /// <summary>
        /// アイテムライブラリへの追加画面：GET
        /// </summary>
        [Fact]
        public void AddItem_ReturnsAViewResult()
        {
            //Arrange
            var store = new Mock<IUserStore<IdentityUser>>();
            var userManagerMoq = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            var configMoq = new Mock<IConfiguration>();
            var itemLibRepo = new Mock<IItemLibraryRepository>();
            var youtubeRepo = new Mock<IYoutubeDataRepository>();

            var controller = new PlayListController(
                userManagerMoq.Object, 
                itemLibRepo.Object,
                youtubeRepo.Object,
                configMoq.Object);

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
            var store = new Mock<IUserStore<IdentityUser>>();
            var userManagerMoq = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            var configMoq = new Mock<IConfiguration>();
            var itemLibRepo = new Mock<IItemLibraryRepository>();
            itemLibRepo.Setup(repo => repo.InsertAsync(It.IsAny<LibraryItemPlatform>(), It.IsAny<string>(), It.IsAny<string>(),It.IsAny<IdentityUser>()))
                .Verifiable();
            var youtubeRepo = new Mock<IYoutubeDataRepository>();
            youtubeRepo.Setup(repo => repo.GetYoutubeVideoInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(new YoutubeVideoRetrieveOperationResult() { OperationResult = YoutubeAPIRetrieveOperationResultType.Success })
                .Verifiable();

            var controller = new PlayListController(
                userManagerMoq.Object,
                itemLibRepo.Object,
                youtubeRepo.Object,
                configMoq.Object);

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
            var store = new Mock<IUserStore<IdentityUser>>();
            var userManagerMoq = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            var configMoq = new Mock<IConfiguration>();
            var itemLibRepo = new Mock<IItemLibraryRepository>();
            itemLibRepo.Setup(repo => repo.InsertAsync(It.IsAny<LibraryItemPlatform>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IdentityUser>()))
                .Verifiable();
            var youtubeRepo = new Mock<IYoutubeDataRepository>();
            youtubeRepo.Setup(repo => repo.GetYoutubeVideoInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(new YoutubeVideoRetrieveOperationResult() { OperationResult = YoutubeAPIRetrieveOperationResultType.Success })
                .Verifiable();

            var controller = new PlayListController(
                userManagerMoq.Object,
                itemLibRepo.Object,
                youtubeRepo.Object,
                configMoq.Object);

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
            var store = new Mock<IUserStore<IdentityUser>>();
            var userManagerMoq = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            var configMoq = new Mock<IConfiguration>();
            var itemLibRepo = new Mock<IItemLibraryRepository>();
            var youtubeRepo = new Mock<IYoutubeDataRepository>();
            youtubeRepo.Setup(repo => repo.GetYoutubeVideoInfoAsync(It.IsAny<string>()))
                .Verifiable();

            var controller = new PlayListController(
                userManagerMoq.Object,
                itemLibRepo.Object,
                youtubeRepo.Object,
                configMoq.Object);

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
            var store = new Mock<IUserStore<IdentityUser>>();
            var userManagerMoq = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            var configMoq = new Mock<IConfiguration>();
            var itemLibRepo = new Mock<IItemLibraryRepository>();
            var youtubeRepo = new Mock<IYoutubeDataRepository>();
            youtubeRepo.Setup(repo => repo.GetYoutubeVideoInfoAsync(It.IsAny<string>()))
                .Verifiable();

            var controller = new PlayListController(
                userManagerMoq.Object,
                itemLibRepo.Object,
                youtubeRepo.Object,
                configMoq.Object);

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

    }

   
}
