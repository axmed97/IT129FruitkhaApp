using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using WebUI.Data;
using WebUI.Models;
// CRUD - Create, Read, Update, Delete
namespace WebUI.Areas.Admin.Controllers;
[Area(nameof(Admin))]
public class SliderController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;
    public SliderController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public IActionResult Index()
    {
        var sliders = _context.Sliders.ToList();
        return View(sliders);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Slider slider, IFormFile file)
    {
        if (file == null)
        {
            ModelState.AddModelError("error", "Photo is required");
            return View();
        }

        if (!ModelState.IsValid)
            return View();

        

        string fileName = @"/uploads/" + Guid.NewGuid() + file.FileName;

        using var stream = new FileStream(_env.WebRootPath + fileName, FileMode.Create);
        await file.CopyToAsync(stream);
        
        slider.PhotoUrl = fileName;
        await _context.Sliders.AddAsync(slider);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Edit(int id)
    {
        var slider = _context.Sliders.FirstOrDefault(x => x.Id == id);
        if(slider == null) return NotFound();

        return View(slider);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(int id, Slider slider, IFormFile file)
    {

        var sliderDb = _context.Sliders.FirstOrDefault(x => x.Id == id);

        if (file != null)
        {
            string fileName = @"/uploads/" + Guid.NewGuid() + file.FileName;

            using var stream = new FileStream(_env.WebRootPath + fileName, FileMode.Create);
            await file.CopyToAsync(stream);
            sliderDb.PhotoUrl = fileName;
        }

        sliderDb.SubTitle = slider.SubTitle;
        sliderDb.Title = slider.Title;
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public IActionResult Delete(int id)
    {

        var slider = _context.Sliders.FirstOrDefault(x => x.Id == id);
        if(slider == null) return NotFound();
        return View(slider);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(Slider slider)
    {
        _context.Sliders.Remove(slider);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }
}
