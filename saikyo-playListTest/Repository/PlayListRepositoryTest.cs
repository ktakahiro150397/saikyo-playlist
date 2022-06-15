using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Infrastructure;
using System.Security.Claims;
using TestSupport.EfHelpers;

namespace saikyo_playListTest.Repository
{

    public class PlayListRepositoryTest
    {

        private Mock<IdentityUser> userMoq;
        private Mock<YoutubeDataRepository> youtubeDataRepoMoq;
        private ApplicationDbContext ApplicationDbContext;
        private IPlayListRepository _repo { get; set; }
        private IConfiguration Configuration { get; set; }
        public PlayListRepositoryTest()
        {
            Configuration = new ConfigurationBuilder().AddUserSecrets<ItemLibraryRepositoryTest>().Build();

            var builder = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseSqlServer(Configuration.GetConnectionString("ApplicationDbContextConnection_Test_PlayList"));
            var options = builder.Options;
            ApplicationDbContext = new ApplicationDbContext(options);

            userMoq = new Mock<IdentityUser>();
            userMoq.Setup(user => user.Id).Returns("test_user_id");

            youtubeDataRepoMoq = new Mock<YoutubeDataRepository>();

            _repo = new PlayListRepository(ApplicationDbContext, Configuration);
        }

        /// <summary>
        /// プレイリストのシード用データを取得します。
        /// </summary>
        /// <returns></returns>
        internal void SeedPlayListData()
        {
            //ItemEntityを生成
            var itemEntities = new List<ItemLibrariesEntity>();

            using (var context = new ApplicationDbContext())
            {
                for (var i = 0; i < 30; i++)
                {
                    var item = new ItemLibrariesEntity()
                    {
                        ItemLibrariesEntityId = $"itementity_{i}"
                    };

                    ApplicationDbContext.ItemLibraries.Add(item);
                    ApplicationDbContext.SaveChanges();
                    ApplicationDbContext.Entry(item).State = EntityState.Detached;
                }
            }

            using (var context = new ApplicationDbContext())
            {
                var playListHeaders = new List<PlayListHeadersEntity>()
            {
                new PlayListHeadersEntity()
                {
                    PlayListHeadersEntityId = "playlistheadersentityid_1",
                    Name = "playlistTest_1",
                    AspNetUserdId = "test_user_id",
                    Details = new List<PlayListDetailsEntity>()
                    {
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "playlistdetailsentityid_1",
                            ItemSeq = 0,
                            PlayListHeadersEntityId = "playlistheadersentityid_1",
                            ItemLibrariesEntityId = "itementity_1",
                            ItemLibrariesEntity = new ItemLibrariesEntity()
                            {
                                ItemLibrariesEntityId = "itementity_1",
                            }

                        },
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "playlistdetailsentityid_2",
                            ItemSeq = 1,
                            PlayListHeadersEntityId = "playlistheadersentityid_1",
                            ItemLibrariesEntityId = "itementity_2",
                            ItemLibrariesEntity = new ItemLibrariesEntity()
                            {
                                ItemLibrariesEntityId = "itementity_2",
                            }
                        },
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "playlistdetailsentityid_3",
                            ItemSeq = 2,
                            PlayListHeadersEntityId = "playlistheadersentityid_1",
                            ItemLibrariesEntityId = "itementity_3",
                            ItemLibrariesEntity = new ItemLibrariesEntity()
                            {
                                ItemLibrariesEntityId = "itementity_3",
                            }
                        },
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "playlistdetailsentityid_4",
                            ItemSeq = 3,
                            PlayListHeadersEntityId = "playlistheadersentityid_1",
                            ItemLibrariesEntityId = "itementity_4",
                            ItemLibrariesEntity = new ItemLibrariesEntity()
                            {
                                ItemLibrariesEntityId = "itementity_4",
                            }
                        },
                    }
                },
                new PlayListHeadersEntity()
                {
                    PlayListHeadersEntityId = "playlistheadersentityid_2",
                    Name = "playlistTest_2",
                    AspNetUserdId = "test_user_id",
                    Details = new List<PlayListDetailsEntity>()
                    {
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "playlistdetailsentityid_5",
                            ItemSeq = 0,
                            PlayListHeadersEntityId = "playlistheadersentityid_2",
                            ItemLibrariesEntityId = "itementity_5",
                            ItemLibrariesEntity = new ItemLibrariesEntity()
                            {
                                ItemLibrariesEntityId = "itementity_5",
                            }
                        },
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "playlistdetailsentityid_6",
                            ItemSeq = 1,
                            PlayListHeadersEntityId = "playlistheadersentityid_2",
                            ItemLibrariesEntityId = "itementity_6",
                            ItemLibrariesEntity = new ItemLibrariesEntity()
                            {
                                ItemLibrariesEntityId = "itementity_6",
                            }
                        },
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "playlistdetailsentityid_7",
                            ItemSeq = 2,
                            PlayListHeadersEntityId = "playlistheadersentityid_2",
                            ItemLibrariesEntityId = "itementity_7",
                            ItemLibrariesEntity = new ItemLibrariesEntity()
                            {
                                ItemLibrariesEntityId = "itementity_7",
                            }
                        },
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "playlistdetailsentityid_8",
                            ItemSeq = 3,
                            PlayListHeadersEntityId = "playlistheadersentityid_2",
                            ItemLibrariesEntityId = "itementity_8",
                            ItemLibrariesEntity = new ItemLibrariesEntity()
                            {
                                ItemLibrariesEntityId = "itementity_8",
                            }
                        },
                    }
                },
                new PlayListHeadersEntity()
                {
                    PlayListHeadersEntityId = "playlistheadersentityid_3",
                    Name = "playlistTest_3",
                    AspNetUserdId = "test_user_id_other",
                    Details = new List<PlayListDetailsEntity>()
                    {
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "playlistdetailsentityid_10",
                            ItemSeq = 0,
                            PlayListHeadersEntityId = "playlistheadersentityid_3",
                            ItemLibrariesEntityId = "itementity_9",
                            ItemLibrariesEntity = new ItemLibrariesEntity()
                            {
                                ItemLibrariesEntityId = "itementity_9",
                            }
                        },
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "playlistdetailsentityid_11",
                            ItemSeq = 1,
                            PlayListHeadersEntityId = "playlistheadersentityid_3",
                            ItemLibrariesEntityId = "itementity_10",
                            ItemLibrariesEntity = new ItemLibrariesEntity()
                            {
                                ItemLibrariesEntityId = "itementity_10",
                            }
                        },
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "playlistdetailsentityid_12",
                            ItemSeq = 2,
                            PlayListHeadersEntityId = "playlistheadersentityid_3",
                            ItemLibrariesEntityId = "itementity_11",
                            ItemLibrariesEntity = new ItemLibrariesEntity()
                            {
                                ItemLibrariesEntityId = "itementity_11",
                            }
                        },
                        new PlayListDetailsEntity()
                        {
                            PlayListDetailsEntityId = "playlistdetailsentityid_13",
                            ItemSeq = 3,
                            PlayListHeadersEntityId = "playlistheadersentityid_3",
                            ItemLibrariesEntityId = "itementity_12",
                            ItemLibrariesEntity = new ItemLibrariesEntity()
                            {
                                ItemLibrariesEntityId = "itementity_12",
                            }
                        },
                    }
                },
            };

                foreach (var item in playListHeaders)
                {
                    foreach (var detail in item.Details)
                    {
                        ApplicationDbContext.Entry(detail.ItemLibrariesEntity).State = EntityState.Unchanged;
                    }
                }

                ApplicationDbContext.PlayListHeaders.AddRange(playListHeaders);
                ApplicationDbContext.SaveChanges();
            }


        }

        /// <summary>
        /// プレイリスト取得
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPlayListHeaderAll_Success()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            //Act
            var result = await _repo.GetPlayListHeaderAll(userMoq.Object);

            //Assert
            Assert.NotNull(result);
            var list = Assert.IsAssignableFrom<List<PlayListHeadersEntity>>(result);
            Assert.Equal(2, list.Count);

        }

        /// <summary>
        /// プレイリスト作成成功
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateNewPlayListAsync_Success()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            //Act
            var result = await _repo.CreateNewPlayListAsync("unitTest_playListName", userMoq.Object);

            //Assert
            //インサートされているはずのデータが存在することを確認
            var header = ApplicationDbContext.PlayListHeaders
                .Where(header => header.Name == "unitTest_playListName" && header.AspNetUserdId == "test_user_id")
                .FirstOrDefault();

            Assert.NotNull(header);
            Assert.Equal(PlayListOperationResultType.Success, result.OperationResult);

            //結果セット内のAssert
            Assert.Null(result.Exception);
            Assert.NotNull(result.HeaderEntity);
            Assert.Equal(header!.Name, result.HeaderEntity!.Name);
            Assert.Equal(header!.AspNetUserdId, result.HeaderEntity!.AspNetUserdId);
            Assert.Equal(header!.PlayListHeadersEntityId, result.HeaderEntity!.PlayListHeadersEntityId);
        }

        /// <summary>
        /// プレイリスト作成失敗 名前なし
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task CreateNewPlayListAsync_BlankName()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            //Act
            var result = await _repo.CreateNewPlayListAsync("", userMoq.Object);

            //Assert
            var appEx = Assert.IsAssignableFrom<ApplicationException>(result.Exception);
            Assert.Equal("プレイリストの名前が入力されていません。", appEx.Message);
            Assert.Equal(PlayListOperationResultType.NoName, result.OperationResult);
            Assert.Null(result.HeaderEntity);
        }

        /// <summary>
        /// プレイリストアイテム追加成功
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddItemToPlayListAsync_Success()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var header = ApplicationDbContext.PlayListHeaders
                .Where(header => header.PlayListHeadersEntityId == "playlistheadersentityid_1" && header.AspNetUserdId == userMoq.Object.Id)
                .First();

            var addDetail = new PlayListDetailsEntity()
            {
                ItemSeq = 0,
                ItemLibrariesEntityId = "itementity_add_AddItemToPlayListAsync_Success_1",
                PlayListHeadersEntityId = "playlistheadersentityid_1"
            };

            //Act
            var result = await _repo.AddItemToPlayListAsync(header, addDetail, userMoq.Object);

            //Assert
            //インサートされているはずのデータを取得
            var insertResult = ApplicationDbContext.PlayListHeaders
                .Where(header => header.PlayListHeadersEntityId == "playlistheadersentityid_1" && header.AspNetUserdId == userMoq.Object.Id)
                .Join(
                    ApplicationDbContext.PlayListDetails,
                    header => header.PlayListHeadersEntityId,
                    detail => detail.PlayListHeadersEntityId,
                    (header, detail) => new
                    {
                        header.PlayListHeadersEntityId,
                        detail.ItemLibrariesEntityId,
                        detail.ItemSeq
                    }
                )
                .OrderBy(elem => elem.ItemSeq)
                .ToList();
            Assert.Equal(5, insertResult.Count);
            Assert.Equal("itementity_add_AddItemToPlayListAsync_Success_1", insertResult.Last().ItemLibrariesEntityId);
            Assert.Equal("playlistheadersentityid_1", insertResult.Last().PlayListHeadersEntityId);
            Assert.Equal(4, insertResult.Last().ItemSeq);

            //結果セットのAssert
            Assert.Equal(PlayListOperationResultType.Success, result.OperationResult);
            Assert.Null(result.Exception);
            Assert.Equal(header.PlayListHeadersEntityId, result.HeaderEntity!.PlayListHeadersEntityId);
        }

        /// <summary>
        /// プレイリストアイテム追加失敗　ヘッダーが存在しない
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddItemToPlayListAsync_NoHeader()
        {

            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var header = new PlayListHeadersEntity()
            {
                PlayListHeadersEntityId = "not_exist_id"
            };

            var addDetail = new PlayListDetailsEntity()
            {
                ItemSeq = 0,
                ItemLibrariesEntityId = "itementity_add_AddItemToPlayListAsync_NoHeader_1",
                PlayListHeadersEntityId = "playlistheadersentityid_1"
            };

            //Act
            var result = await _repo.AddItemToPlayListAsync(header, addDetail, userMoq.Object);

            //Assert
            //結果セットのAssert
            Assert.Equal(PlayListOperationResultType.NotFound, result.OperationResult);
            var appEx = Assert.IsAssignableFrom<ApplicationException>(result.Exception);
            Assert.Equal("プレイリストが存在しませんでした。", appEx.Message);
        }

        /// <summary>
        /// プレイリストアイテムリストで追加成功
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddItemToPlayListAsync_Enumerable_Success()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var header = ApplicationDbContext.PlayListHeaders
                .Where(header => header.PlayListHeadersEntityId == "playlistheadersentityid_1" && header.AspNetUserdId == userMoq.Object.Id)
                .First();

            var addDetail = new List<PlayListDetailsEntity>()
            {
                new PlayListDetailsEntity()
                {
                    ItemSeq = 0,
                ItemLibrariesEntityId = "itementity_add_AddItemToPlayListAsync_Enumerable_Success_1",
                PlayListHeadersEntityId = "playlistheadersentityid_1"
                },
                new PlayListDetailsEntity()
                {
                    ItemSeq = 0,
                ItemLibrariesEntityId = "itementity_add_AddItemToPlayListAsync_Enumerable_Success_2",
                PlayListHeadersEntityId = "playlistheadersentityid_1"
                },
                new PlayListDetailsEntity()
                {
                    ItemSeq = 0,
                ItemLibrariesEntityId = "itementity_add_AddItemToPlayListAsync_Enumerable_Success_3",
                PlayListHeadersEntityId = "playlistheadersentityid_1"
                },
            };

            //Act
            var result = await _repo.AddItemToPlayListAsync(header, addDetail, userMoq.Object);

            //Assert
            //インサートされているはずのデータを取得
            var insertResult = ApplicationDbContext.PlayListHeaders
                .Where(header => header.PlayListHeadersEntityId == "playlistheadersentityid_1" && header.AspNetUserdId == userMoq.Object.Id)
                .Join(
                    ApplicationDbContext.PlayListDetails,
                    header => header.PlayListHeadersEntityId,
                    detail => detail.PlayListHeadersEntityId,
                    (header, detail) => new
                    {
                        header.PlayListHeadersEntityId,
                        detail.ItemLibrariesEntityId,
                        detail.ItemSeq
                    }
                )
                .OrderBy(elem => elem.ItemSeq)
                .ToList();
            Assert.Equal(7, insertResult.Count);
            Assert.Equal("itementity_add_AddItemToPlayListAsync_Enumerable_Success_3", insertResult.Last().ItemLibrariesEntityId);
            Assert.Equal("playlistheadersentityid_1", insertResult.Last().PlayListHeadersEntityId);
            Assert.Equal(6, insertResult.Last().ItemSeq);

            //結果セットのAssert
            Assert.Equal(PlayListOperationResultType.Success, result.OperationResult);
            Assert.Null(result.Exception);
            Assert.Equal(header.PlayListHeadersEntityId, result.HeaderEntity!.PlayListHeadersEntityId);


        }


        /// <summary>
        /// プレイリストアイテム追加失敗　ヘッダーが存在しない
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task AddItemToPlayListAsync_Enumerable_NoHeader()
        {

            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var header = new PlayListHeadersEntity()
            {
                PlayListHeadersEntityId = "not_exist_id"
            };

            var addDetail = new List<PlayListDetailsEntity>()
            {
                new PlayListDetailsEntity()
                {
                    ItemSeq = 0,
                ItemLibrariesEntityId = "itementity_add_AddItemToPlayListAsync_Enumerable_NoHeader_1",
                PlayListHeadersEntityId = "playlistheadersentityid_1"
                },
                new PlayListDetailsEntity()
                {
                    ItemSeq = 0,
                ItemLibrariesEntityId = "itementity_add_AddItemToPlayListAsync_Enumerable_NoHeader_2",
                PlayListHeadersEntityId = "playlistheadersentityid_1"
                },
                new PlayListDetailsEntity()
                {
                    ItemSeq = 0,
                ItemLibrariesEntityId = "itementity_add_AddItemToPlayListAsync_Enumerable_NoHeader_3",
                PlayListHeadersEntityId = "playlistheadersentityid_1"
                },
            };

            //Act
            var result = await _repo.AddItemToPlayListAsync(header, addDetail, userMoq.Object);

            //Assert
            //結果セットのAssert
            Assert.Equal(PlayListOperationResultType.NotFound, result.OperationResult);
            var appEx = Assert.IsAssignableFrom<ApplicationException>(result.Exception);
            Assert.Equal("プレイリストが存在しませんでした。", appEx.Message);
        }

        /// <summary>
        /// プレイリストアイテムの削除成功
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RemoveItemFromPlayListAsync_Success()
        {

            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var header = ApplicationDbContext.PlayListHeaders
                .Where(header => header.PlayListHeadersEntityId == "playlistheadersentityid_1" && header.AspNetUserdId == userMoq.Object.Id)
                .First();

            var deleteId = "playlistdetailsentityid_2";

            //Act
            var result = await _repo.RemoveItemFromPlayListAsync(header, deleteId, userMoq.Object);

            //Assert
            //インサートされているはずのデータを取得
            var deleteResult = ApplicationDbContext.PlayListHeaders
                .Where(header => header.PlayListHeadersEntityId == "playlistheadersentityid_1" && header.AspNetUserdId == userMoq.Object.Id)
                .Join(
                    ApplicationDbContext.PlayListDetails,
                    header => header.PlayListHeadersEntityId,
                    detail => detail.PlayListHeadersEntityId,
                    (header, detail) => new
                    {
                        header.PlayListHeadersEntityId,
                        detail.ItemLibrariesEntityId,
                        detail.PlayListDetailsEntityId,
                        detail.ItemSeq
                    }
                )
                .OrderBy(elem => elem.ItemSeq)
                .ToList();

            //削除済みデータが存在しないことを確認
            Assert.Equal(3, deleteResult.Count);
            Assert.DoesNotContain(deleteResult, elem => elem.PlayListDetailsEntityId == deleteId);

            //結果セットのAssert
            Assert.Equal(PlayListOperationResultType.Success, result.OperationResult);
            Assert.Null(result.Exception);
            Assert.Equal(header.PlayListHeadersEntityId, result.HeaderEntity!.PlayListHeadersEntityId);

        }

        /// <summary>
        /// プレイリストアイテムの削除失敗　存在しない
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RemoveItemFromPlayListAsync_NoExist()
        {

            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var header = ApplicationDbContext.PlayListHeaders
                .Where(header => header.PlayListHeadersEntityId == "playlistheadersentityid_1" && header.AspNetUserdId == userMoq.Object.Id)
                .First();

            var deleteId = "no_exist_playlistdetailid";

            //Act
            var result = await _repo.RemoveItemFromPlayListAsync(header, deleteId, userMoq.Object);

            //Assert
            //インサートされているはずのデータを取得
            var deleteResult = ApplicationDbContext.PlayListHeaders
                .Where(header => header.PlayListHeadersEntityId == "playlistheadersentityid_1" && header.AspNetUserdId == userMoq.Object.Id)
                .Join(
                    ApplicationDbContext.PlayListDetails,
                    header => header.PlayListHeadersEntityId,
                    detail => detail.PlayListHeadersEntityId,
                    (header, detail) => new
                    {
                        header.PlayListHeadersEntityId,
                        detail.ItemLibrariesEntityId,
                        detail.PlayListDetailsEntityId,
                        detail.ItemSeq
                    }
                )
                .OrderBy(elem => elem.ItemSeq)
                .ToList();

            //数が変わっていないことを確認
            Assert.Equal(4, deleteResult.Count);

            //結果セットのAssert
            Assert.Equal(PlayListOperationResultType.NotFound, result.OperationResult);
            var appEx = Assert.IsAssignableFrom<ApplicationException>(result.Exception);
            Assert.Equal("削除対象のアイテムが存在しませんでした。", appEx.Message);

        }

        /// <summary>
        /// プレイリストアイテムの削除失敗　他ユーザーのデータ
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RemoveItemFromPlayListAsync_TryToDeleteOtherUsersItem()
        {

            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var header = ApplicationDbContext.PlayListHeaders
                .Where(header => header.PlayListHeadersEntityId == "playlistheadersentityid_3" && header.AspNetUserdId == "test_user_id_other")
                .First();

            var deleteId = "playlistdetailsentityid_13";

            //Act
            var result = await _repo.RemoveItemFromPlayListAsync(header, deleteId, userMoq.Object);

            //Assert
            //削除されていないことを確認
            var deleteResult = ApplicationDbContext.PlayListHeaders
                .Where(header => header.PlayListHeadersEntityId == "playlistheadersentityid_1" && header.AspNetUserdId == "test_user_id_other")
                .Join(
                    ApplicationDbContext.PlayListDetails,
                    header => header.PlayListHeadersEntityId,
                    detail => detail.PlayListHeadersEntityId,
                    (header, detail) => new
                    {
                        header.PlayListHeadersEntityId,
                        detail.ItemLibrariesEntityId,
                        detail.PlayListDetailsEntityId,
                        detail.ItemSeq
                    }
                )
                .OrderBy(elem => elem.ItemSeq)
                .ToList();

            //数が変わっていないことを確認
            Assert.Equal(4, deleteResult.Count);

            //結果セットのAssert
            Assert.Equal(PlayListOperationResultType.NotFound, result.OperationResult);
            var appEx = Assert.IsAssignableFrom<ApplicationException>(result.Exception);
            Assert.Equal("他ユーザーのプレイリストデータを削除しようとしました。", appEx.Message);
        }


    }

}
