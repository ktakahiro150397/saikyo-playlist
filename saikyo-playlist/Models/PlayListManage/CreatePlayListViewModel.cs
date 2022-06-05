using saikyo_playlist.Data;
using System.ComponentModel.DataAnnotations;

namespace saikyo_playlist.Models.PlayListManage
{
    public class CreatePlayListViewModel
    {
        [Required]
        [Display(Name = "プレイリスト名")]
        public string Title { get; set; }

        [Required]
        [Display(Name = "追加URL")]
        public string Urls { get; set; }

        public CreatePlayListViewModel()
        {
            Title = "";
            Urls = "";
        }
    }

}
