

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

            var controller = new PlayListController(
                userManagerMoq.Object, 
                itemLibRepo.Object,
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
            itemLibRepo.Setup(repo => repo.InsertAsync(It.IsAny<LibraryItemPlatform>(), It.IsAny<string>(), It.IsAny<string>()))
                .Verifiable();


            var controller = new PlayListController(
                userManagerMoq.Object,
                itemLibRepo.Object,
                configMoq.Object);

            var model = new AddItemViewModel()
            {
                Url = "www.testUrl.com",
                TitleAlias = "テスト",
                Platform = LibraryItemPlatform.Youtube,
                ErrorMessage = ""
            };

            //Act
            var actResult = await controller.AddItem(model);

            //Assert
            var viewResult = Assert.IsType<RedirectResult>(actResult);
            itemLibRepo.Verify();


        }

    }

   
}
