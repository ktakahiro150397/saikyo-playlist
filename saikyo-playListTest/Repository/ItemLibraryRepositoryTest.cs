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



    }
}
