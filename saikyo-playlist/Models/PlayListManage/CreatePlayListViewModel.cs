using saikyo_playlist.Data;
using System.ComponentModel.DataAnnotations;
using saikyo_playlist.Repository.Interfaces;
using saikyo_playlist.Repository.Implements;
using Microsoft.AspNetCore.Identity;

namespace saikyo_playlist.Models.PlayListManage
{
    public class CreateEditDeletePlayListViewModel
    {

        public string PlayListHeaderId { get; set; }

        [Required]
        [Display(Name = "プレイリスト名")]
        public string Title { get; set; }

        /// <summary>
        /// アイテムライブラリの一覧
        /// </summary>
        public List<ItemLibrariesEntity> Libraries { get; set; }

        /// <summary>
        /// アイテムライブラリのうち、プレイリストに追加するよう選択されたIDリスト。
        /// 追加順に連番を振ります。
        /// </summary>
        public List<string> SelectedLibraryHeaderIdList { get; set; }

        /// <summary>
        /// このプレイリストに登録されているアイテムの情報。
        /// </summary>
        public List<PlayListDetailsEntity> PlayListDetails { get; set; }

        public string ErrorMessage { get; set; }

        public CreateEditDeletePlayListViewModel()
        {
            PlayListHeaderId = "";
            Title = "";
            Libraries = new List<ItemLibrariesEntity>();
            PlayListDetails = new List<PlayListDetailsEntity>();
            SelectedLibraryHeaderIdList = new List<string>();
            ErrorMessage = "";
        }

        public async Task SetPlayList(string playListHeaderId, IPlayListRepository playListRepository, IdentityUser user)
        {
            var getResult = await playListRepository.GetPlayListAsync(playListHeaderId, user);
            if(getResult.OperationResult != PlayListOperationResultType.Success)
            {
                //取得に失敗
                throw new ApplicationException("プレイリストの取得に失敗しました。", getResult.Exception);
            }
            else
            {
                PlayListHeaderId = playListHeaderId;
                PlayListDetails = getResult.HeaderEntity!.Details.ToList();
            }

        }

    }

}
