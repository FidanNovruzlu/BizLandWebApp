using BizLandWebApp.DataContext;
using BizLandWebApp.Models;
using BizLandWebApp.ViewModels.ServiceVM;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace BizLandWebApp.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = "Admin")]
public class ServiceController : Controller
{
    private readonly BizLandDbContext _context;
    public ServiceController(BizLandDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        List<Service> services = _context.Services.ToList();
        return View(services);
    }
    public IActionResult Read(int id)
    {
        Service? services= _context.Services.FirstOrDefault(s => s.Id == id);
        if(services == null) return NotFound();
        return View(services);
    }
    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public async Task< IActionResult> Create(CreateServiceVM service)
    {
        if (!ModelState.IsValid) return View(service);

        if(_context.Services.Any(c => c.Title.Trim().ToLower() == service.Title.Trim().ToLower()))
        {
            ModelState.AddModelError("Title", "Service Already exist");
            return View(service);
        }
        Service newService = new Service()
        {
            Title= service.Title,
            Description= service.Description,
            IconName =service.IconName
        };
        await  _context.Services.AddAsync(newService);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
        Service? services= _context.Services.FirstOrDefault(c => c.Id == id);
        if(services == null) return NotFound();

        _context.Services.Remove(services);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
    public IActionResult Update(int id)
    {
        Service? services= _context.Services.FirstOrDefault(s => s.Id == id);
        if(services == null) return NotFound();
        return View(services);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Update(int id,Service newService)
    {
        Service? services = _context.Services.FirstOrDefault(s => s.Id == id);
        if (services == null) return NotFound();

        if (_context.Services.Any(c => c.Title.Trim().ToLower() == newService.Title.Trim().ToLower()))
        {
            ModelState.AddModelError("Title", "Service Already exist");
            return View();
        }
        services.Title = newService.Title;
        services.Description = newService.Description;
        services.IconName= newService.IconName;
     
        _context.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
}
