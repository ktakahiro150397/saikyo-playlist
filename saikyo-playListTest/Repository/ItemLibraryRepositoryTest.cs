using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Infrastructure;
using System.Security.Claims;
//using System.Data.Entity;

namespace saikyo_playListTest.Repository
{
    public class ItemLibraryRepositoryTest
    {
        public IItemLibraryRepository _repo;
        public Mock<IdentityUser> user;

        public ApplicationDbContext ApplicationDbContext;

        private static readonly object LockObject = new object();
        private static bool isDatabaseInitialized = false;

        public ItemLibraryRepositoryTest()
        {
            var Configuration = new ConfigurationBuilder().AddUserSecrets<ItemLibraryRepositoryTest>().Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(Configuration.GetConnectionString("ApplicationDbContextConnection_Test"));
            var options = builder.Options;
            ApplicationDbContext = new ApplicationDbContext(options);

            user = new Mock<IdentityUser>();
            user.Setup(user => user.Id).Returns("test_user_id");

            _repo = new ItemLibraryRepository(ApplicationDbContext, user.Object);

            //テスト用インメモリDBの構成
            SeedTestData();
        }

        internal void SeedTestData()
        {

            lock (LockObject)
            {
                if (isDatabaseInitialized) return;

                ApplicationDbContext.Database.EnsureDeleted();
                ApplicationDbContext.Database.Migrate();

                //テスト用データの追加
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

            };

                ApplicationDbContext.ItemLibraries.AddRange(data);
                ApplicationDbContext.SaveChanges();

                isDatabaseInitialized = true;
            }
        }

        /// <summary>
        /// 成功
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllAsync()
        {
            //Act
            var actResult = await _repo.GetAllAsync();

            //Assert
            Assert.NotNull(actResult);
            Assert.Equal(3, actResult.Count());
        }

        /// <summary>
        /// 成功・データ0件
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetAllAsync_NoResult()
        {
            //Arrange
            user.Setup(user => user.Id).Returns("test_user_id_hogehoge");

            //Act
            var actResult = await _repo.GetAllAsync();

            //Moq戻す
            user.Setup(user => user.Id).Returns("test_user_id");

            //Assert
            Assert.NotNull(actResult);
            Assert.Empty(actResult);

        }

        [Fact]
        public async Task InsertAsync_Success()
        {
            //Arrange
            var platform = LibraryItemPlatform.Youtube;
            var itemId = "insertasync_success_id";
            var title = "insertasync_testTitle";

            //Act
            var actResult = await _repo.InsertAsync(platform, itemId, title);

            //Assert
            //インサートされているはずのデータを取得する
            var insertData = ApplicationDbContext.ItemLibraries
                .Where(item => item.ItemId == itemId)
                .Where(item => item.AspNetUserdId == user.Object.Id)
                .FirstOrDefault();

            //結果
            Assert.NotNull(actResult);
            Assert.Equal(ItemLibraryOperationResultType.Success, actResult.OperationResult);
            Assert.Null(actResult.Exception);

            //データ
            Assert.NotNull(insertData);
            Assert.Equal(title, insertData!.Title);
            Assert.Equal(platform, insertData.Platform);
            Assert.Equal(itemId, insertData.ItemId);

        }

        [Fact]
        public async Task InsertAsync_Success_Duplicate_OtherUser()
        {
            //Arrange
            var platform = LibraryItemPlatform.Youtube;
            var itemId = "item_id_xxx_3";
            var title = "insertasync_Duplicate_OtherUser";

            //Act
            var actResult = await _repo.InsertAsync(platform, itemId, title);

            //Assert
            //インサートされているはずのデータを取得する
            var insertData = ApplicationDbContext.ItemLibraries
                .Where(item => item.ItemLibrariesEntityId == itemId)
                .Where(item => item.AspNetUserdId == user.Object.Id)
                .FirstOrDefault();

            //結果
            Assert.NotNull(actResult);
            Assert.Equal(ItemLibraryOperationResultType.Success, actResult.OperationResult);
            Assert.Null(actResult.Exception);

            //データ
            Assert.NotNull(insertData);
            Assert.Equal(title, insertData!.Title);
            Assert.Equal(platform, insertData.Platform);
            Assert.Equal(itemId, insertData.ItemId);

        }

        [Fact]
        public async Task InsertAsync_Duplicate_Mine()
        {
            //Arrange
            var platform = LibraryItemPlatform.Youtube;
            var itemId = "item_id_xxx_1";
            var title = "insertasync_duplicate";

            //Act
            var actResult = await _repo.InsertAsync(platform, itemId, title);

            //Assert
            //上記インサートされていないはず(元々存在するデータを取得するはず)
            var insertData = ApplicationDbContext.ItemLibraries
                .Where(item => item.ItemLibrariesEntityId == itemId)
                .Where(item => item.AspNetUserdId == user.Object.Id)
                .FirstOrDefault();

            //結果
            Assert.NotNull(actResult);
            Assert.Equal(ItemLibraryOperationResultType.Duplicate, actResult.OperationResult);
            Assert.Null(actResult.Exception);

            //データ
            Assert.NotNull(insertData);
            Assert.NotEqual(title, insertData!.Title);
        }

        [Fact]
        public async Task DeleteAsync_Success()
        {
            //Arrange
            var entityid = "entity_id_1";

            //Act
            var actResult = await _repo.DeleteAsync(entityid);

            //Assert
            //結果
            Assert.Equal(ItemLibraryOperationResultType.Success, actResult.OperationResult);
            Assert.Null(actResult?.Exception);

            //データが存在しないことを確認する
            var deletedData = ApplicationDbContext.ItemLibraries
                .Where(item => item.ItemLibrariesEntityId == entityid)
                .FirstOrDefault();
            Assert.Null(deletedData);

        }

        [Fact]
        public async Task DeleteAsync_NotFound()
        {
            //Arrange
            var entityid = "entity_id_not_exist";

            //Act
            var actResult = await _repo.DeleteAsync(entityid);

            //Assert
            //結果
            Assert.Equal(ItemLibraryOperationResultType.NotFound, actResult.OperationResult);
            Assert.Null(actResult?.Exception);
        }

        [Fact]
        public async Task DeleteAsync_TryToDeleteOtherUserItem()
        {
            //Arrange
            var entityid = "entity_id_3";

            //Act
            var actResult = await _repo.DeleteAsync(entityid);

            //Assert
            //結果
            Assert.Equal(ItemLibraryOperationResultType.NotFound, actResult.OperationResult);
            Assert.Null(actResult?.Exception);
        }

    }
}
