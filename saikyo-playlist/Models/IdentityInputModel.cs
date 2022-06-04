#nullable disable

using System.ComponentModel.DataAnnotations;

namespace saikyo_playlist.Models
{

    /// <summary>
    /// アカウント新規作成時の入力データモデル
    /// </summary>
    public class IdentityInputCreateNewModel
    {

        /// <summary>
        /// ユーザー名
        /// </summary>
        [Required]
        [Display(Name = "ユーザー名")]
        public string UserName { get; set; }

        /// <summary>
        /// パスワード
        /// </summary>
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "パスワード")]
        public string Password { get; set; }

        /// <summary>
        /// パスワード確認
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "パスワード確認")]
        [Compare(nameof(Password),ErrorMessage = "入力されたデータがパスワードと一致しません。")]
        public string ConfirmPassword { get; set; }

    }
}
