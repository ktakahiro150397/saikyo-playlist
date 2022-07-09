using saikyo_playlist.Data;
using System.ComponentModel.DataAnnotations;
using saikyo_playlist.Repository.Interfaces;
using saikyo_playlist.Repository.Implements;
using Microsoft.AspNetCore.Identity;

namespace saikyo_playlist.Models.PlayListManage
{
    public class CreateEditDeletePlayListViewModel
    {
        /// <summary>
        /// プレイリストヘッダーのID。
        /// 画面から新規作成されている場合は空白が設定されます。
        /// </summary>
        public string PlayListHeaderId { get; set; }

        [Required]
        [Display(Name = "プレイリスト名")]
        public string Title { get; set; }

        /// <summary>
        /// アイテムライブラリの一覧
        /// </summary>
        public List<ItemLibrariesEntity> Libraries { get; set; }

        /// <summary>
        /// アイテムライブラリのうち、プレイリストに追加するよう選択されたアイテム情報リスト。
        /// </summary>
        public List<PlayListEditorDisplayData> SelectedLibraryInfo { get; set; }

        public string Url { get; set; }

        public string ErrorMessage { get; set; }

        public CreateEditDeletePlayListViewModel()
        {
            PlayListHeaderId = "";
            Title = "";
            Url = "";
            Libraries = new List<ItemLibrariesEntity>();
            SelectedLibraryInfo = new List<PlayListEditorDisplayData>();
            ErrorMessage = "";
        }

        public async Task SetPlayList(string playListHeaderId,IItemLibraryRepository itemLibraryRepository, IPlayListRepository playListRepository, IdentityUser user)
        {
            var getResult = playListRepository.GetPlayList(playListHeaderId, user);
            if(getResult.OperationResult != PlayListOperationResultType.Success)
            {
                //取得に失敗
                throw new ApplicationException("プレイリストの取得に失敗しました。", getResult.Exception);
            }
            else
            {
                PlayListHeaderId = playListHeaderId;
                var playListDetails = getResult.HeaderEntity!.Details;

                foreach(var detail in playListDetails)
                {
                    SelectedLibraryInfo.Add(
                        new PlayListEditorDisplayData()
                        {
                            ItemLibraryEntityId = detail.ItemLibrariesEntityId,
                            itemSeq = detail.ItemSeq,
                            PlayListDetailsEntityId = detail.PlayListDetailsEntityId,
                            ItemLibraryName = detail.ItemLibrariesEntity.Title,
                        });
                }
            }

            var libResult = await itemLibraryRepository.GetAllAsync(user);
            Libraries = libResult.ToList();
        }

    }

    /// <summary>
    /// プレイリストの編集画面で表示している・されているデータを表します。
    /// </summary>
    public class PlayListEditorDisplayData
    {
        /// <summary>
        /// プレイリスト詳細ID。
        /// 画面上から新規追加されている場合は空白が設定されます。
        /// </summary>
        public string PlayListDetailsEntityId { get; set; }

        /// <summary>
        /// 選択されたアイテムID。
        /// </summary>
        public string ItemLibraryEntityId { get; set; }

        /// <summary>
        /// 画面上に表示するアイテムライブラリの名前。
        /// 編集時のGET時に使用します。
        /// </summary>
        public string ItemLibraryName { get; set; }

        /// <summary>
        /// 選択されたアイテムIDの連番。
        /// </summary>
        public int itemSeq { get; set; }
       
        public PlayListEditorDisplayData()
        {
            PlayListDetailsEntityId = "";
            ItemLibraryEntityId = "";
        }

    }

}
