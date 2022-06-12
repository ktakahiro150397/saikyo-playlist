using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Infrastructure;
using System.Security.Claims;
//using System.Data.Entity;

namespace saikyo_playListTest.Repository
{
    public class ItemLibraryRepositoryTest
    {

        public Mock<DbSet<ItemLibrariesEntity>> ItemLibraryDbSetMoq;

        public Mock<ApplicationDbContext> ApplicationDbContextMoq;

        public Mock<UserManager<IdentityUser>> userManagerMoq;

        public Mock<IConfiguration> configMoq;

        public ItemLibraryRepository _repo;

        public ItemLibraryRepositoryTest()
        {

            var store = new Mock<IUserStore<IdentityUser>>();
            userManagerMoq = new Mock<UserManager<IdentityUser>>(store.Object, null, null, null, null, null, null, null, null);
            //var loginUserInfo = await UserManager.GetUserAsync(User);
            userManagerMoq.Setup(userManager => userManager.GetUserAsync(It.IsAny<ClaimsPrincipal>()))
                .ReturnsAsync(new IdentityUser { Id = "test_user_id" });

            configMoq = new Mock<IConfiguration>();


            var testData = GetTestData();
            ItemLibraryDbSetMoq = new Mock<DbSet<ItemLibrariesEntity>>();
            ItemLibraryDbSetMoq.As<IDbAsyncEnumerable<ItemLibrariesEntity>>()
                .Setup(m => m.GetAsyncEnumerator())
                .Returns(new TestDbAsyncEnumerator<ItemLibrariesEntity>(testData.GetEnumerator()));
            ItemLibraryDbSetMoq.As<IQueryable<ItemLibrariesEntity>>().Setup(m => m.Provider).Returns(testData.Provider);
            ItemLibraryDbSetMoq.As<IQueryable<ItemLibrariesEntity>>().Setup(m => m.Expression).Returns(testData.Expression);
            ItemLibraryDbSetMoq.As<IQueryable<ItemLibrariesEntity>>().Setup(m => m.ElementType).Returns(testData.ElementType);
            ItemLibraryDbSetMoq.As<IQueryable<ItemLibrariesEntity>>().Setup(m => m.GetEnumerator()).Returns(testData.GetEnumerator());

            ApplicationDbContextMoq = new Mock<ApplicationDbContext>();
            ApplicationDbContextMoq.Setup(context => context.ItemLibraries).Returns(ItemLibraryDbSetMoq.Object);

            _repo = new ItemLibraryRepository(ApplicationDbContextMoq.Object, userManagerMoq.Object);

        }

        private IQueryable<ItemLibrariesEntity> GetTestData()
        {
            var data = new List<ItemLibrariesEntity>
            {
                new ItemLibrariesEntity()
                {
                    ItemLibrariesEntityId = "entity_id_1",
                    Title = "lib_item_1",
                    ItemId = "item_id_xxx_1",
                    Platform = LibraryItemPlatform.Youtube,
                    PlayCount = 0,
                    AspNetUserdId = "test_user_id"
                },
                new ItemLibrariesEntity()
                {
                    ItemLibrariesEntityId = "entity_id_2",
                    Title = "lib_item_2",
                    ItemId = "item_id_xxx_2",
                    Platform = LibraryItemPlatform.Youtube,
                    PlayCount = 0,
                    AspNetUserdId = "test_user_id"
                },
                new ItemLibrariesEntity()
                {
                    ItemLibrariesEntityId = "entity_id_3",
                    Title = "lib_item_3",
                    ItemId = "item_id_xxx_3",
                    Platform = LibraryItemPlatform.Youtube,
                    PlayCount = 0,
                    AspNetUserdId = "test_user_id_other"
                },
                new ItemLibrariesEntity()
                {
                    ItemLibrariesEntityId = "entity_id_4",
                    Title = "lib_item_4",
                    ItemId = "item_id_xxx_4",
                    Platform = LibraryItemPlatform.Youtube,
                    PlayCount = 0,
                    AspNetUserdId = "test_user_id"
                },
                new ItemLibrariesEntity()
                {
                    ItemLibrariesEntityId = "entity_id_5",
                    Title = "lib_item_5",
                    ItemId = "item_id_xxx_5",
                    Platform = LibraryItemPlatform.Youtube,
                    PlayCount = 0,
                    AspNetUserdId = "test_user_id_other_2"
                },

            }.AsQueryable();

            return data;
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllAsync_Success()
        {

            //Arrange
            var data = GetTestData();

            //Act
            var actResult = await _repo.GetAllAsync();

            //Assert
            Assert.NotNull(actResult);
            Assert.Equal(3, actResult.Count());
            
        }




    }
}
