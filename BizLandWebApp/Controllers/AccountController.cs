using BizLandWebApp.DataContext;
using BizLandWebApp.Models;
using BizLandWebApp.ViewModels.AccountVM;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using System.Net;
using System.Net.Mail;


namespace BizLandWebApp.Controllers;
public class AccountController : Controller
{
    private readonly UserManager<AppUser> _userManager;
    private readonly SignInManager<AppUser> _signInManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
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

        #region AddRole
        IdentityResult result = await _userManager.AddToRoleAsync(newUser, "Admin");
        if (!result.Succeeded)
        {
            foreach (IdentityError? error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
                return View(registerVM);
            }
        }
        #endregion

        string token = await _userManager.GenerateEmailConfirmationTokenAsync(newUser);
        string link = Url.Action("ConfrimUser", "Account", new {email=newUser.Email,token=token},HttpContext.Request.Scheme);

        MailMessage message=new MailMessage("7L2P4QW@code.edu.az",newUser.Email)
        {
            Subject="Confirmation Email",
            Body=$"<a href=\"{link}\">Click to confirm mail</a>",
            IsBodyHtml=true,
        };
        SmtpClient smtpClient= new SmtpClient()
        {
            Host="smtp.gmail.com",
            Port=587,
            EnableSsl=true,
            Credentials= new NetworkCredential("7L2P4QW@code.edu.az", "zprjiiettxbribqy")
        };

        smtpClient.Send(message);

        
        return RedirectToAction(nameof(Login));
    }
    public async Task<IActionResult> ConfrimUser(string email,string token)
    {
        AppUser user=  await _userManager.FindByEmailAsync(email);
        if (user == null) return NotFound();

       IdentityResult result=  await _userManager.ConfirmEmailAsync(user,token);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("", "Token confrim incorrect.");
            return View();
        }
        await _signInManager.SignInAsync(user, true);
        return RedirectToAction("Index","Home");
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

    #region Create Role
    //public async Task<IActionResult> CreateRole()
    //{
    //    IdentityRole role = new IdentityRole()
    //    {
    //        Name="Admin"
    //    };
    //    if(role==null) return NotFound();
    //    await _roleManager.CreateAsync(role);
    //    return Json("OK");
    //}
    #endregion
}
