using BizLandWebApp.DataContext;
using BizLandWebApp.Models;
using BizLandWebApp.ViewModels.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using System.Net;
using System.Net.Mail;


namespace BizLandWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Register(RegisterVM registerVM)
        {
            if(!ModelState.IsValid) return View(registerVM);

            AppUser newUser=new AppUser()
            {
               Surname= registerVM.Surname,
               Name= registerVM.Name,
               Email= registerVM.Email,
               UserName= registerVM.UserName
            };

            IdentityResult identityResult=  await _userManager.CreateAsync(newUser , registerVM.Password);
            if (!identityResult.Succeeded)
            {
                foreach(IdentityError? error in identityResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View(registerVM);
                }
            }

            return RedirectToAction(nameof(Login));
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task< IActionResult> Login(LoginVM login)
        {
            if (!ModelState.IsValid) return View(login);

            AppUser appUser=await _userManager.FindByNameAsync(login.UserName);
            if (appUser == null)
            {
                ModelState.AddModelError("", "Invalid password or username!");
                return View(login);
            }
            Microsoft.AspNetCore.Identity.SignInResult signInResult= await _signInManager.PasswordSignInAsync(appUser, login.Password, true, false);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Invalid password or username!");
                return View(login);
            }
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index","Home");
        }
    }
}
