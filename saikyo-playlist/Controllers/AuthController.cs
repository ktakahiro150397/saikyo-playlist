using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using saikyo_playlist.Models;
using System.Security.Cryptography;

namespace saikyo_playlist.Controllers
{
    public class AuthController : Controller
    {

        private UserManager<IdentityUser> UserManager { get; set; }

        private SignInManager<IdentityUser> SignInManager { get; set; }


        public AuthController(UserManager<IdentityUser> _userManager,
            SignInManager<IdentityUser> _signInManager)
        {
            UserManager = _userManager;
            SignInManager = _signInManager;
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(IdentityInputCreateNewModel model)
        {

            if (ModelState.IsValid)
            {
                //新規ユーザーの作成
                var user = new IdentityUser() { UserName = model.UserName };
                var result = await UserManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {

                    //新規作成成功時、トップページへ戻る
                    return View("../Home/Index");

                    ////新規ユーザーの登録成功
                    ////作成したユーザーでログインする
                    //var loginResult = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);

                    //if (loginResult.Succeeded)
                    //{

                    //}
                    //else
                    //{
                    //    //ログイン失敗時
                    //    ViewData["Create_Result"] = $"{model.UserName}でのログインに失敗しました。アカウントは作成されています。";
                    //    return View();
                    //}

                }
                else
                {
                    //新規ユーザーの登録失敗
                    ViewData["Create_Result"] = $"{model.UserName}での登録に失敗しました。";
                    return View();
                }
            }
            else
            {
                ViewData["Create_Result"] = $"入力内容が正しくありません。";
                return View();

            }



        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(IdentityInputCreateNewModel model)
        {

            var result = await SignInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);

            if (result.Succeeded)
            {
                //ログイン成功
                return View("../Home/Index");
            }
            else
            {
                //ログイン失敗
                ViewData["Login_Result"] = $"{model.UserName}でのログインに失敗しました。";
                return View();
            }
        }


    }
}
