using BizLandWebApp.DataContext;
using BizLandWebApp.Models;
using BizLandWebApp.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BizLandWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly BizLandDbContext _bizLandDbContext;
        public HomeController(BizLandDbContext bizLandDbContext)
        {
            _bizLandDbContext = bizLandDbContext;
        }

        public async Task< IActionResult> Index()
        {
            List<Team> teams=await _bizLandDbContext.Teams.ToListAsync();
            List<Service> services = await _bizLandDbContext.Services.ToListAsync();
            HomeVM homeVM= new HomeVM()
            {
                Teams= teams,
                Services= services
            };
            return View(homeVM);
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}