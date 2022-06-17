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
                        ItemLibrariesEntityId = $"itementity_{i}",
                        AspNetUserdId = userMoq.Object.Id
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
        public void GetPlayListHeaderAll_Success()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            //Act
            var result = _repo.GetPlayListHeaderAll(userMoq.Object);

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

            var headerId = "playlistheadersentityid_1";

            var addDetail = new PlayListDetailsEntity()
            {
                ItemSeq = 0,
                PlayListHeadersEntityId = "playlistheadersentityid_1",
            };
            addDetail.ItemLibrariesEntity = ApplicationDbContext.ItemLibraries
                .Where(item => item.ItemLibrariesEntityId == "itementity_13").First();

            //Act
            var result = await _repo.AddItemToPlayListAsync(headerId, addDetail, userMoq.Object);

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
            Assert.Equal("itementity_13", insertResult.Last().ItemLibrariesEntityId);
            Assert.Equal("playlistheadersentityid_1", insertResult.Last().PlayListHeadersEntityId);
            Assert.Equal(4, insertResult.Last().ItemSeq);

            //結果セットのAssert
            Assert.Equal(PlayListOperationResultType.Success, result.OperationResult);
            Assert.Null(result.Exception);
            Assert.Equal(headerId, result.HeaderEntity!.PlayListHeadersEntityId);
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

            var headerId = "not_exist_id";

            var addDetail = new PlayListDetailsEntity()
            {
                ItemSeq = 0,
                PlayListHeadersEntityId = "playlistheadersentityid_1"
            };
            addDetail.ItemLibrariesEntity = ApplicationDbContext.ItemLibraries
               .Where(item => item.ItemLibrariesEntityId == "itementity_13").First();

            //Act
            var result = await _repo.AddItemToPlayListAsync(headerId, addDetail, userMoq.Object);

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

            var headerId = "playlistheadersentityid_1";

            var addDetail = new List<PlayListDetailsEntity>()
            {
                new PlayListDetailsEntity()
                {
                    ItemSeq = 0,
                    ItemLibrariesEntity = ApplicationDbContext.ItemLibraries.Where(item => item.ItemLibrariesEntityId == "itementity_0").First(),
                },
                new PlayListDetailsEntity()
                {
                    ItemSeq = 0,
                    ItemLibrariesEntity = ApplicationDbContext.ItemLibraries.Where(item => item.ItemLibrariesEntityId == "itementity_1").First(),
                },
                new PlayListDetailsEntity()
                {
                    ItemSeq = 0,
                    ItemLibrariesEntity = ApplicationDbContext.ItemLibraries.Where(item => item.ItemLibrariesEntityId == "itementity_2").First(),
                },
            };

            //Act
            var result = await _repo.AddItemToPlayListAsync(headerId, addDetail, userMoq.Object);

            //Assert
            //インサートされているはずのデータを取得
            var insertResult = ApplicationDbContext.PlayListHeaders
                .Where(header => header.PlayListHeadersEntityId == headerId && header.AspNetUserdId == userMoq.Object.Id)
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
            Assert.Equal("itementity_2", insertResult.Last().ItemLibrariesEntityId);
            Assert.Equal("playlistheadersentityid_1", insertResult.Last().PlayListHeadersEntityId);
            Assert.Equal(6, insertResult.Last().ItemSeq);

            //結果セットのAssert
            Assert.Equal(PlayListOperationResultType.Success, result.OperationResult);
            Assert.Null(result.Exception);
            Assert.Equal(headerId, result.HeaderEntity!.PlayListHeadersEntityId);


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

            var headerId = "not_exist_id";

            var addDetail = new List<PlayListDetailsEntity>()
            {
                new PlayListDetailsEntity()
                {
                    ItemSeq = 0,
                    ItemLibrariesEntity = ApplicationDbContext.ItemLibraries.Where(item => item.ItemLibrariesEntityId == "itementity_0").First(),
                },
                new PlayListDetailsEntity()
                {
                    ItemSeq = 0,
                    ItemLibrariesEntity = ApplicationDbContext.ItemLibraries.Where(item => item.ItemLibrariesEntityId == "itementity_1").First(),
                },
                new PlayListDetailsEntity()
                {
                    ItemSeq = 0,
                    ItemLibrariesEntity = ApplicationDbContext.ItemLibraries.Where(item => item.ItemLibrariesEntityId == "itementity_2").First(),
                },
            };

            //Act
            var result = await _repo.AddItemToPlayListAsync(headerId, addDetail, userMoq.Object);

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

            var headerId = "playlistheadersentityid_1";

            var deleteId = "playlistdetailsentityid_2";

            //Act
            var result = await _repo.RemoveItemFromPlayListAsync(headerId, deleteId, userMoq.Object);

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
            Assert.Equal(headerId, result.HeaderEntity!.PlayListHeadersEntityId);

        }

        /// <summary>
        /// プレイリストアイテムの削除失敗　ヘッダーが存在しない
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RemoveItemFromPlayListAsync_NoExistHeaderId()
        {

            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var headerId = "no_exist_headerid";

            var deleteId = "playlistdetailsentityid_2";

            //Act
            var result = await _repo.RemoveItemFromPlayListAsync(headerId, deleteId, userMoq.Object);

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
            Assert.Equal("削除対象のプレイリストが存在しませんでした。", appEx.Message);

        }

        /// <summary>
        /// プレイリストアイテムの削除失敗　詳細アイテムが存在しない
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task RemoveItemFromPlayListAsync_NoExistDetailId()
        {

            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var headerId = "playlistheadersentityid_1";

            var deleteId = "no_exist_playlistdetailid";

            //Act
            var result = await _repo.RemoveItemFromPlayListAsync(headerId, deleteId, userMoq.Object);

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
            Assert.Equal("削除対象のプレイリストアイテムが存在しませんでした。", appEx.Message);

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

            var headerId = "playlistheadersentityid_3";

            var deleteId = "playlistdetailsentityid_13";

            //Act
            var result = await _repo.RemoveItemFromPlayListAsync(headerId, deleteId, userMoq.Object);

            //Assert
            //削除されていないことを確認
            var deleteResult = ApplicationDbContext.PlayListHeaders
                .Where(header => header.PlayListHeadersEntityId == "playlistheadersentityid_3" && header.AspNetUserdId == "test_user_id_other")
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

        /// <summary>
        /// プレイリストアイテムの取得　成功
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPlayListAsync_Success()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var headerId = "playlistheadersentityid_2";

            //Act
            var result = await _repo.GetPlayListAsync(headerId, userMoq.Object);

            //Assert
            Assert.Null(result.Exception);
            Assert.Equal(PlayListOperationResultType.Success, result.OperationResult);
            Assert.Equal(headerId, result.HeaderEntity!.PlayListHeadersEntityId);
            Assert.Equal(userMoq.Object.Id, result.HeaderEntity!.AspNetUserdId);
            Assert.Equal("playlistTest_1", result.HeaderEntity!.Name);
            Assert.Equal(3, result.HeaderEntity!.Details.Count);

        }

        /// <summary>
        /// プレイリストアイテムの取得　失敗・見つからない
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPlayListAsync_NotFound()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var headerId = "not_exist_headerid";

            //Act
            var result = await _repo.GetPlayListAsync(headerId, userMoq.Object);

            //Assert
            Assert.Equal(PlayListOperationResultType.NotFound, result.OperationResult);
            Assert.Null(result.HeaderEntity);
            var appEx = Assert.IsAssignableFrom<ApplicationException>(result.Exception);
            Assert.Equal("プレイリストが存在しませんでした。", appEx.Message);
        }

        /// <summary>
        /// プレイリストアイテムの取得　他ユーザーのデータ
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GetPlayListAsync_TryToDeleteOtherUser()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var headerId = "playlistheadersentityid_3";

            //Act
            var result = await _repo.GetPlayListAsync(headerId, userMoq.Object);

            //Assert
            Assert.Equal(PlayListOperationResultType.NotFound, result.OperationResult);
            Assert.Null(result.HeaderEntity);
            var appEx = Assert.IsAssignableFrom<ApplicationException>(result.Exception);
            Assert.Equal("他ユーザーのプレイリストデータを取得しようとしました。", appEx.Message);
        }

        /// <summary>
        /// プレイリストアイテムの順序更新　ヘッダーが見つからない
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdatePlayListItemSeqAsync_HeaderNotFound()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var headerId = "not_exist_header_id";
            var detailId = "playlistdetailsentityid_2";
            var itemSeq = 0;

            //Act
            var result = await _repo.UpdatePlayListItemSeqAsync(headerId, detailId,itemSeq, userMoq.Object);

            //Assert
            Assert.Equal(PlayListOperationResultType.NotFound, result.OperationResult);
            Assert.Null(result.HeaderEntity);
            var appEx = Assert.IsAssignableFrom<ApplicationException>(result.Exception);
            Assert.Equal("プレイリストが存在しませんでした。",appEx.Message);
        }

        /// <summary>
        /// プレイリストアイテムの順序更新　詳細アイテムが見つからない
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdatePlayListItemSeqAsync_DetailNotFound()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var headerId = "playlistheadersentityid_1";
            var detailId = "not_exist_detailid";
            var itemSeq = 0;

            //Act
            var result = await _repo.UpdatePlayListItemSeqAsync(headerId, detailId, itemSeq, userMoq.Object);

            //Assert
            Assert.Equal(PlayListOperationResultType.NotFound, result.OperationResult);
            Assert.Null(result.HeaderEntity);
            var appEx = Assert.IsAssignableFrom<ApplicationException>(result.Exception);
            Assert.Equal("更新対象のプレイリストアイテムが存在しませんでした。", appEx.Message);
        }

        /// <summary>
        /// プレイリストアイテムの順序更新　成功・隣同士
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdatePlayListItemSeqAsync_SideBySideItem()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var headerId = "playlistheadersentityid_1";
            var detailId = "playlistdetailsentityid_2";
            var itemSeq = 0;

            //入れ替え対象となる詳細ID
            var targetDetailId = "playlistdetailsentityid_1";

            //Act
            var result = await _repo.UpdatePlayListItemSeqAsync(headerId, detailId, itemSeq, userMoq.Object);

            //Assert
            Assert.Equal(PlayListOperationResultType.Success, result.OperationResult);
            Assert.Null(result.Exception);

            //順序が入れ替わっていることを確認
            Assert.NotNull(result.HeaderEntity);
            Assert.Equal(0, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == detailId).ItemSeq);
            Assert.Equal(1, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == targetDetailId).ItemSeq);
            Assert.Equal(1, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_3").ItemSeq);
            Assert.Equal(1, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_4").ItemSeq);

            //DBも入れ替わっているかどうかを確認
            //更新対象
            var updateDetail = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == detailId);
            //交換対象
            var targetDetail = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == targetDetailId);

            //その他アイテム
            var item_other_1 = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_3");
            var item_other_2 = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_4");

            Assert.Equal(0, updateDetail.ItemSeq);
            Assert.Equal(1, targetDetail.ItemSeq);
            Assert.Equal(2, item_other_1.ItemSeq);
            Assert.Equal(3, item_other_2.ItemSeq);

        }

        /// <summary>
        /// プレイリストアイテムの順序更新　成功・1つ飛ばし
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdatePlayListItemSeqAsync_OneItemInBetween()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var headerId = "playlistheadersentityid_1";
            var detailId = "playlistdetailsentityid_3";
            var itemSeq = 0;

            //入れ替え対象となる詳細ID
            var targetDetailId = "playlistdetailsentityid_1";

            //Act
            var result = await _repo.UpdatePlayListItemSeqAsync(headerId, detailId, itemSeq, userMoq.Object);

            //Assert
            Assert.Equal(PlayListOperationResultType.Success, result.OperationResult);
            Assert.Null(result.Exception);

            //順序が入れ替わっていることを確認
            Assert.NotNull(result.HeaderEntity);
            Assert.Equal(0, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == detailId).ItemSeq);
            Assert.Equal(1, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == targetDetailId).ItemSeq);
            Assert.Equal(2, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_2").ItemSeq);
            Assert.Equal(3, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_4").ItemSeq);

            //DBも入れ替わっているかどうかを確認
            //更新対象
            var updateDetail = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == detailId);
            //交換対象
            var targetDetail = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == targetDetailId);

            //その他アイテム
            var item_other_1 = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_2");
            var item_other_2 = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_4");

            Assert.Equal(0, updateDetail.ItemSeq);
            Assert.Equal(1, targetDetail.ItemSeq);
            Assert.Equal(2, item_other_1.ItemSeq);
            Assert.Equal(3, item_other_2.ItemSeq);

        }

        /// <summary>
        /// プレイリストアイテムの順序更新　先頭アイテムの更新
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdatePlayListItemSeqAsync_FirstItem()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var headerId = "playlistheadersentityid_1";
            var detailId = "playlistdetailsentityid_1";
            var itemSeq = 2;

            //入れ替え対象となる詳細ID
            var targetDetailId = "playlistdetailsentityid_3";

            //Act
            var result = await _repo.UpdatePlayListItemSeqAsync(headerId, detailId, itemSeq, userMoq.Object);

            //Assert
            Assert.Equal(PlayListOperationResultType.Success, result.OperationResult);
            Assert.Null(result.Exception);

            //順序が入れ替わっていることを確認
            Assert.NotNull(result.HeaderEntity);
            Assert.Equal(2, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == detailId).ItemSeq);
            Assert.Equal(1, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == targetDetailId).ItemSeq);
            Assert.Equal(0, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_2").ItemSeq);
            Assert.Equal(3, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_4").ItemSeq);

            //DBも入れ替わっているかどうかを確認
            //更新対象
            var updateDetail = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == detailId);
            //交換対象
            var targetDetail = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == targetDetailId);

            //その他アイテム
            var item_other_1 = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_2");
            var item_other_2 = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_4");

            Assert.Equal(2, updateDetail.ItemSeq);
            Assert.Equal(1, targetDetail.ItemSeq);
            Assert.Equal(0, item_other_1.ItemSeq);
            Assert.Equal(3, item_other_2.ItemSeq);

        }

        /// <summary>
        /// プレイリストアイテムの順序更新　最後アイテムの更新
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdatePlayListItemSeqAsync_LastItem()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var headerId = "playlistheadersentityid_1";
            var detailId = "playlistdetailsentityid_4";
            var itemSeq = 1;

            //入れ替え対象となる詳細ID
            var targetDetailId = "playlistdetailsentityid_2";

            //Act
            var result = await _repo.UpdatePlayListItemSeqAsync(headerId, detailId, itemSeq, userMoq.Object);

            //Assert
            Assert.Equal(PlayListOperationResultType.Success, result.OperationResult);
            Assert.Null(result.Exception);

            //順序が入れ替わっていることを確認
            Assert.NotNull(result.HeaderEntity);
            Assert.Equal(1, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == detailId).ItemSeq);
            Assert.Equal(2, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == targetDetailId).ItemSeq);
            Assert.Equal(0, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_1").ItemSeq);
            Assert.Equal(3, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_3").ItemSeq);

            //DBも入れ替わっているかどうかを確認
            //更新対象
            var updateDetail = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == detailId);
            //交換対象
            var targetDetail = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == targetDetailId);

            //その他アイテム
            var item_other_1 = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_1");
            var item_other_2 = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_3");

            Assert.Equal(1, updateDetail.ItemSeq);
            Assert.Equal(2, targetDetail.ItemSeq);
            Assert.Equal(0, item_other_1.ItemSeq);
            Assert.Equal(3, item_other_2.ItemSeq);

        }

        /// <summary>
        /// プレイリストアイテムの順序更新　最大連番以上の値で更新
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdatePlayListItemSeqAsync_ExceedMaxSeq()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var headerId = "playlistheadersentityid_1";
            var detailId = "playlistdetailsentityid_1";
            var itemSeq = 4;

            //入れ替え対象となる詳細ID
            var targetDetailId = "playlistdetailsentityid_4";

            //Act
            var result = await _repo.UpdatePlayListItemSeqAsync(headerId, detailId, itemSeq, userMoq.Object);

            //Assert
            Assert.Equal(PlayListOperationResultType.Success, result.OperationResult);
            Assert.Null(result.Exception);

            //順序が入れ替わっていることを確認
            Assert.NotNull(result.HeaderEntity);
            Assert.Equal(3, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == detailId).ItemSeq);
            Assert.Equal(2, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == targetDetailId).ItemSeq);
            Assert.Equal(0, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_2").ItemSeq);
            Assert.Equal(1, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_3").ItemSeq);

            //DBも入れ替わっているかどうかを確認
            //更新対象
            var updateDetail = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == detailId);
            //交換対象
            var targetDetail = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == targetDetailId);

            //その他アイテム
            var item_other_1 = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_2");
            var item_other_2 = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_3");

            Assert.Equal(3, updateDetail.ItemSeq);
            Assert.Equal(2, targetDetail.ItemSeq);
            Assert.Equal(0, item_other_1.ItemSeq);
            Assert.Equal(1, item_other_2.ItemSeq);

        }

        /// <summary>
        /// プレイリストアイテムの順序更新　負の値で更新
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdatePlayListItemSeqAsync_NegativeSeq()
        {

            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var headerId = "playlistheadersentityid_1";
            var detailId = "playlistdetailsentityid_1";
            var itemSeq = -1;

            //入れ替え対象となる詳細ID
            var targetDetailId = "playlistdetailsentityid_4";

            //Act,Assert
            var argEx = await Assert.ThrowsAsync<ArgumentException>(() => _repo.UpdatePlayListItemSeqAsync(headerId, detailId, itemSeq, userMoq.Object));

            //Assert
            Assert.Equal("itemSeq", argEx.ParamName);
            Assert.Equal("プレイリスト連番に負の値が指定されました。", argEx.Message);
        }

        /// <summary>
        /// プレイリストアイテムの順序更新　同一の値で更新
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task UpdatePlayListItemSeqAsync_SameSeq()
        {
            //Arrange
            ApplicationDbContext.Database.EnsureClean();
            SeedPlayListData();

            var headerId = "playlistheadersentityid_1";
            var detailId = "playlistdetailsentityid_4";
            var itemSeq = 3;//同一seqを設定

            //Act
            var result = await _repo.UpdatePlayListItemSeqAsync(headerId, detailId, itemSeq, userMoq.Object);

            //Assert
            Assert.Equal(PlayListOperationResultType.Success, result.OperationResult);
            Assert.Null(result.Exception);

            //順序が変わっていないことを確認
            Assert.NotNull(result.HeaderEntity);
            Assert.Equal(3, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == detailId).ItemSeq);
            Assert.Equal(0, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_1").ItemSeq);
            Assert.Equal(1, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_2").ItemSeq);
            Assert.Equal(2, result.HeaderEntity!.Details.Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_3").ItemSeq);

            //DBも順序が変わっていないことを確認
            //更新対象
            var updateDetail = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == detailId);
            //交換対象
            var targetDetail = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_1");

            //その他アイテム
            var item_other_1 = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_2");
            var item_other_2 = ApplicationDbContext.PlayListDetails
                .Single(detail => detail.PlayListDetailsEntityId == "playlistdetailsentityid_3");

            Assert.Equal(3, updateDetail.ItemSeq);
            Assert.Equal(0, targetDetail.ItemSeq);
            Assert.Equal(1, item_other_1.ItemSeq);
            Assert.Equal(2, item_other_2.ItemSeq);

        }









    }

}
