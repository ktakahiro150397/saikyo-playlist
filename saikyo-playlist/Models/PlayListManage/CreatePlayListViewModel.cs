using saikyo_playlist.Data;
using System.ComponentModel.DataAnnotations;

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

        public string ErrorMessage { get; set; }

        public CreateEditDeletePlayListViewModel()
        {
            PlayListHeaderId = "";
            Title = "";
            Libraries = new List<ItemLibrariesEntity>();
            SelectedLibraryHeaderIdList = new List<string>();
            ErrorMessage = "";
        }
    }

}
