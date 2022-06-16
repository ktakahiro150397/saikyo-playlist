

using saikyo_playlist.Data.Video;
using saikyo_playlist.Helpers;
using saikyo_playlist.Models.PlayListManage;
using saikyo_playlist.Repository.Implements;
using System.Security.Claims;

namespace saikyo_playListTest.Controllers
{


    public class PlayListControllerTest
    {

        public Mock<UserManager<IdentityUser>> userManagerMoq;

        public Mock<IdentityUser> userMoq;

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
            userManagerMoq.Setup(manager => manager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new IdentityUser() { Id = "test_user_id" });
            userMoq = new Mock<IdentityUser>();
            userMoq.Setup(moq => moq.Id).Returns("test_user_id");
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

        #region Helper method

        /// <summary>
        /// GetAllAsync()から返却されるアイテムライブラリ一覧
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<ItemLibrariesEntity> ItemLibraryRepo_GetAllResult()
        {
            var itemLibRepoRetValue = new List<ItemLibrariesEntity>()
            {
                new ItemLibrariesEntity()
                {
                    ItemLibrariesEntityId = "ItemLibrariesEntityId_1",
                    AspNetUserdId = "test_user_id",
                    ItemId = "itemid_1",
                    PlayCount = 0,
                    Platform = LibraryItemPlatform.Youtube,
                    Title = "title_1",
                    TitleAlias = "titleAlias_1",
                },
                new ItemLibrariesEntity()
                {
                    ItemLibrariesEntityId = "ItemLibrariesEntityId_2",
                    AspNetUserdId = "test_user_id",
                    ItemId = "itemid_2",
                    PlayCount = 0,
                    Platform = LibraryItemPlatform.Youtube,
                    Title = "title_2",
                    TitleAlias = "titleAlias_2",
                },
                new ItemLibrariesEntity()
                {
                    ItemLibrariesEntityId = "ItemLibrariesEntityId_3",
                    AspNetUserdId = "test_user_id",
                    ItemId = "itemid_3",
                    PlayCount = 0,
                    Platform = LibraryItemPlatform.Youtube,
                    Title = "title_3",
                    TitleAlias = "titleAlias_3",
                },
                new ItemLibrariesEntity()
                {
                    ItemLibrariesEntityId = "ItemLibrariesEntityId_4",
                    AspNetUserdId = "test_user_id",
                    ItemId = "itemid_4",
                    PlayCount = 0,
                    Platform = LibraryItemPlatform.Youtube,
                    Title = "title_3",
                    TitleAlias = "titleAlias_4",
                },
                new ItemLibrariesEntity()
                {
                    ItemLibrariesEntityId = "ItemLibrariesEntityId_5",
                    AspNetUserdId = "AspNetUserId_5",
                    ItemId = "itemid_5",
                    PlayCount = 0,
                    Platform = LibraryItemPlatform.Youtube,
                    Title = "title_3",
                    TitleAlias = "titleAlias_5",
                },
                new ItemLibrariesEntity()
                {
                    ItemLibrariesEntityId = "ItemLibrariesEntityId_6",
                    AspNetUserdId = "AspNetUserId_6",
                    ItemId = "itemid_6",
                    PlayCount = 0,
                    Platform = LibraryItemPlatform.Youtube,
                    Title = "title_6",
                    TitleAlias = "titleAlias_6",
                },

            };

            return itemLibRepoRetValue;
        }

        /// <summary>
        /// テスト用のプレイリスト情報
        /// </summary>
        /// <returns></returns>
        internal PlayList PlayListRepo_GetResult()
        {
            var playList = new PlayList()
            {

                Header = new PlayListHeadersEntity()
                {
                    PlayListHeadersEntityId = "header_id",
                    Name = "playlist_name",
                    Details = new List<PlayListDetailsEntity>()
                    {
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "detail_id_1",
                            ItemSeq = 0,
                        },
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "detail_id_2",
                            ItemSeq = 1,
                        },
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "detail_id_3",
                            ItemSeq = 2,
                        },
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "detail_id_4",
                            ItemSeq = 3,
                        },
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "detail_id_5",
                            ItemSeq = 4,
                        },
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "detail_id_6",
                            ItemSeq = 5,
                        },
                    }

                }
            };

            return playList;
        }

        #endregion

        #region "Index Action"

        /// <summary>
        /// プレイリスト一覧　成功
        /// </summary>
        [Fact]
        public async Task Index_ReturnAViewWithModel()
        {
            //Arrange
            playlistRepo.Setup(repo => repo.GetPlayListHeaderAll(It.IsAny<IdentityUser>()))
                .Returns(new List<PlayListHeadersEntity>()
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
            var actResult = await controller.Index();

            //Assert
            var viewResult = Assert.IsType<ViewResult>(actResult);
            var model = Assert.IsAssignableFrom<ManagePlayListViewModel>(viewResult.Model);
            Assert.Equal(2, model.managePlayListItems.Count);
            playlistRepo.Verify(repo => repo.GetPlayListHeaderAll(It.IsAny<IdentityUser>()), Times.Once());
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
            itemLibRepo.Setup(repo => repo.InsertAsync(It.IsAny<LibraryItemPlatform>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IdentityUser>()))
                .Verifiable();
            youtubeRepo.Setup(repo => repo.GetYoutubeVideoInfoAsync(It.IsAny<string>()))
                .ReturnsAsync(new YoutubeVideoRetrieveOperationResult()
                {
                    OperationResult = YoutubeAPIRetrieveOperationResultType.Success,
                    RetrieveResult = new List<YoutubeVideoRetrieveResult>()
                    {
                        new YoutubeVideoRetrieveResult()
                        {
                            ItemId = "moq_itemId",
                            ItemSeq = 0,
                            Title = "moq_Title",
                            Url = "moq_url"
                        }
                    }
                })
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
            Assert.Equal("./PlayList", viewResult.Url);
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
            itemLibRepo.Setup(repo => repo.InsertAsync(It.IsAny<LibraryItemPlatform>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<IdentityUser>()))
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
                .ReturnsAsync(new YoutubeVideoRetrieveOperationResult()
                {
                    OperationResult = YoutubeAPIRetrieveOperationResultType.InvalidUrl,
                    Exception = new ApplicationException("指定されたURLの動画は存在しません。"),
                })
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
            Assert.Equal($"指定されたURLの動画は存在しません。", modelResult.ErrorMessage);

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
                .ReturnsAsync(new YoutubeVideoRetrieveOperationResult()
                {
                    OperationResult = YoutubeAPIRetrieveOperationResultType.InvalidUrl,
                    Exception = new ApplicationException("URLが正しくありません。Youtubeの動画URLを指定してください。"),
                })
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
            Assert.Equal($"URLが正しくありません。Youtubeの動画URLを指定してください。", modelResult.ErrorMessage);

        }

        #endregion

        #region "CreatePlayList Action"

        /// <summary>
        /// プレイリスト作成　GET 成功
        /// </summary>
        [Fact]
        public async Task CreatePlayList_ReturnAViewWithResult()
        {
            //Arrange
            var itemLibRepoRetValue = ItemLibraryRepo_GetAllResult();
            itemLibRepo.Setup(repo => repo.GetAllAsync(It.IsAny<IdentityUser>()))
                .ReturnsAsync(itemLibRepoRetValue.Where(item => item.AspNetUserdId == userMoq.Object.Id))
                .Verifiable();

            //Act
            var actResult = await controller.CreatePlayList();

            //Assert
            var view = Assert.IsType<ViewResult>(actResult);
            var model = Assert.IsType<CreateEditDeletePlayListViewModel>(view.Model);
            Assert.NotNull(model);
            Assert.NotNull(model.Libraries);
            Assert.Equal(4, model.Libraries.Count);
            itemLibRepo.Verify(repo => repo.GetAllAsync(It.IsAny<IdentityUser>()), Times.Once);

        }

        /// <summary>
        /// プレイリスト作成　POST 成功
        /// </summary>
        [Fact]
        public async Task CreatePlayList_RedirectToIndexWithSuccess()
        {

            //Arrange
            var itemLibRepoRetValue = ItemLibraryRepo_GetAllResult();
            var vm = new CreateEditDeletePlayListViewModel()
            {
                Libraries = itemLibRepoRetValue.ToList(),
                PlayListHeaderId = "",
                Title = "CreatePlayList_RedirectToIndexWithSucces_Title",
                SelectedLibraryHeaderIdList = new List<string>()
                {
                    "ItemLibrariesEntityId_1",
                    "ItemLibrariesEntityId_3",
                    "ItemLibrariesEntityId_5",
                }
            };
            playlistRepo.Setup(repo => repo.CreateNewPlayListAsync(It.IsAny<string>(), It.IsAny<IdentityUser>()))
                .ReturnsAsync(new PlayListOperationResult()
                {
                     OperationResult = PlayListOperationResultType.Success,
                     HeaderEntity = new PlayListHeadersEntity()
                     {
                         PlayListHeadersEntityId = "playlistheaderentityId",
                         Name = "test_header_name"
                     }
                })
                .Verifiable();
            playlistRepo.Setup(repo => repo.AddItemToPlayListAsync(It.IsAny<string>(), It.IsAny<PlayListDetailsEntity>(), It.IsAny<IdentityUser>()))
                .Verifiable();

            //Act
            var actResult = await controller.CreatePlayList(vm);

            //Assert
            var view = Assert.IsType<RedirectResult>(actResult);
            Assert.Equal("./PlayList", view.Url);
            playlistRepo.Verify(repo => repo.CreateNewPlayListAsync(It.IsAny<string>(), It.IsAny<IdentityUser>()), Times.Once);
            playlistRepo.Verify(repo => repo.AddItemToPlayListAsync(It.IsAny<string>(), It.IsAny<PlayListDetailsEntity>(), It.IsAny<IdentityUser>()), Times.Exactly(3));

        }

        /// <summary>
        /// プレイリスト作成　POST 失敗・選択されたURLなし
        /// </summary>
        [Fact]
        public async Task CreatePlayList_NoUrlSelected()
        {

            //Arrange
            var itemLibRepoRetValue = ItemLibraryRepo_GetAllResult();
            var vm = new CreateEditDeletePlayListViewModel()
            {
                Libraries = itemLibRepoRetValue.ToList(),
                PlayListHeaderId = "",
                Title = "CreatePlayList_NoUrlSelected_Title",
                SelectedLibraryHeaderIdList = new List<string>(),
                ErrorMessage = ""
            };
            playlistRepo.Setup(repo => repo.CreateNewPlayListAsync(It.IsAny<string>(), It.IsAny<IdentityUser>()))
                .Verifiable();
            playlistRepo.Setup(repo => repo.AddItemToPlayListAsync(It.IsAny<string>(), It.IsAny<PlayListDetailsEntity>(), It.IsAny<IdentityUser>()))
                .Verifiable();

            //Act
            var actResult = await controller.CreatePlayList(vm);

            //Assert
            var view = Assert.IsType<ViewResult>(actResult);
            var model = Assert.IsAssignableFrom<CreateEditDeletePlayListViewModel>(view.Model);

            Assert.Equal(vm.Title, model.Title);
            Assert.Equal(vm.Libraries.Count, model.Libraries.Count);
            Assert.Equal("プレイリストに追加するアイテムを選択してください。", model.ErrorMessage);

            playlistRepo.Verify(repo => repo.CreateNewPlayListAsync(It.IsAny<string>(), It.IsAny<IdentityUser>()), Times.Never);
            playlistRepo.Verify(repo => repo.AddItemToPlayListAsync(It.IsAny<string>(), It.IsAny<PlayListDetailsEntity>(), It.IsAny<IdentityUser>()), Times.Never);

        }

        /// <summary>
        /// プレイリスト作成　POST 失敗・タイトル入力なし
        /// </summary>
        [Fact]
        public async Task CreatePlayList_NoTitleInput()
        {

            //Arrange
            var itemLibRepoRetValue = ItemLibraryRepo_GetAllResult();
            var vm = new CreateEditDeletePlayListViewModel()
            {
                Libraries = itemLibRepoRetValue.ToList(),
                PlayListHeaderId = "",
                Title = "",
                SelectedLibraryHeaderIdList = new List<string>()
                {
                    "ItemLibrariesEntityId_1",
                    "ItemLibrariesEntityId_3",
                    "ItemLibrariesEntityId_5",
                }
            };
            controller.ModelState.AddModelError("Title", "Title is required");
            playlistRepo.Setup(repo => repo.CreateNewPlayListAsync(It.IsAny<string>(), It.IsAny<IdentityUser>()))
                .Verifiable();
            playlistRepo.Setup(repo => repo.AddItemToPlayListAsync(It.IsAny<string>(), It.IsAny<PlayListDetailsEntity>(), It.IsAny<IdentityUser>()))
                .Verifiable();

            //Act
            var actResult = await controller.CreatePlayList(vm);

            var view = Assert.IsType<ViewResult>(actResult);
            var model = Assert.IsAssignableFrom<CreateEditDeletePlayListViewModel>(view.Model);

            Assert.Equal(vm.Title, model.Title);
            Assert.Equal(vm.Libraries.Count, model.Libraries.Count);
            Assert.Equal("プレイリストのタイトルを入力してください。", model.ErrorMessage);

            playlistRepo.Verify(repo => repo.CreateNewPlayListAsync(It.IsAny<string>(), It.IsAny<IdentityUser>()), Times.Never);
            playlistRepo.Verify(repo => repo.AddItemToPlayListAsync(It.IsAny<string>(), It.IsAny<PlayListDetailsEntity>(), It.IsAny<IdentityUser>()), Times.Never);

        }

        #endregion

        #region EditPlayList Action

        /// <summary>
        /// プレイリスト編集　GET 成功
        /// </summary>
        [Fact]
        public void EditPlayList_ReturnAViewWithResult()
        {
            //Arrange
            var playList = PlayListRepo_GetResult();
            playlistRepo.Setup(repo => repo.GetPlayListAsync(It.IsAny<string>(), It.IsAny<IdentityUser>()))
                .ReturnsAsync(new GetPlayListOperationResult()
                {
                    OperationResult = PlayListOperationResultType.Success,
                    PlayList = playList,
                })
                .Verifiable();

            ////Act
            //var actResult = controller.CreatePlayList();

            ////Assert
            //var view = Assert.IsType<ViewResult>(actResult);
            //var model = Assert.IsType<CreateEditDeletePlayListViewModel>(view.Model);
            //Assert.NotNull(model);
            //Assert.NotNull(model.Libraries);
            //Assert.Equal(3, model.Libraries.Count);

        }


        #endregion

    }


}
