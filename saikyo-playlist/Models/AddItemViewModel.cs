using saikyo_playlist.Data;
using System.ComponentModel.DataAnnotations;

namespace saikyo_playlist.Models
{
    public class AddItemViewModel
    {

        [Display(Name = "タイトル")]
        public string TitleAlias { get; set; }

        [Display(Name = "URL")]
        [Required]
        public string Url { get; set; }

        [Display(Name ="プラットフォーム")]
        public LibraryItemPlatform Platform { get; set; }

        public string ErrorMessage { get; set; }

        public AddItemViewModel()
        {
            TitleAlias = "";
            Url = "";
            Platform = LibraryItemPlatform.NotSet;
        }

    }
}
