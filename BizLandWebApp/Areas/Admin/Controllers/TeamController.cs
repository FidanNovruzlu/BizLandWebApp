using BizLandWebApp.DataContext;
using BizLandWebApp.Models;
using BizLandWebApp.ViewModels.TeamVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BizLandWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TeamController : Controller
    {
        private readonly BizLandDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public TeamController(BizLandDbContext context,IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task< IActionResult> Index()
        {
            List<Team> teams = await _context.Teams.ToListAsync();
            return View(teams);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateTeamVM team)
        {
            if (!ModelState.IsValid) return View();
            
            string guid= Guid.NewGuid().ToString();
            string newFilename = guid + team.Image.FileName;
            string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team", newFilename);
            
            using(FileStream fileStream=new FileStream(path, FileMode.CreateNew))
            {
                await team.Image.CopyToAsync(fileStream);
            }
            Team newTeam = new Team()
            {
                Name= team.Name,
                Job= team.Job,
                ImageName= newFilename
            };
            _context.Teams.Add(newTeam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Read(int id)
        {
            Team? team=await _context.Teams.FindAsync(id);
            if(team==null) return NotFound();
            return View(team);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            Team? team = await _context.Teams.FindAsync(id);
            if (team==null) return NotFound();

            if(team.ImageName != null)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team");
                if(System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
            }
            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int id)
        {
            Team? team = await _context.Teams.FindAsync(id);
            if(team==null) return NotFound();

            UpdateTeamVM TeamVM= new UpdateTeamVM()
            {
                Name= team.Name,
                Job= team.Job
            };
            return View(TeamVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(int id,UpdateTeamVM newTeam)
        {
            // Sual---------- Team? team= await  _context.Teams.FindAsync(id);
            Team? team=await _context.Teams.AsNoTracking().Where(t=> t.Id ==id).FirstOrDefaultAsync();
            if(team==null) return NotFound();

            if(!ModelState.IsValid)   return View(newTeam);
         
            if (newTeam.Image != null)
            {
                string path = Path.Combine(_webHostEnvironment.WebRootPath, "assets", "img", "team", team.ImageName);
                using (FileStream fileStream = new FileStream(path, FileMode.Create))
                {
                    await newTeam.Image.CopyToAsync(fileStream);
                }
                newTeam.ImageName = team.ImageName;
            }
            else
            {
                newTeam.ImageName = team.ImageName;
            }
            Team newDbTeam = new Team()
            {
                Id = team.Id,
                Name= newTeam.Name,
                Job= newTeam.Job,
                ImageName= newTeam.ImageName
            };
            _context.Teams.Update(newDbTeam);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
