

namespace saikyo_playListTest.Controllers
{

    
    public class PlayListControllerTest
    {
        [Fact]
        public void AddItem_ReturnsAViewResult()
        {
            //Arrange
            var store = new Mock<IUserStore<IdentityUser>>();
            var userManagerMoq = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);



            var appContextMoq = new Mock<ApplicationDbContext>();
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

    }

   
}
