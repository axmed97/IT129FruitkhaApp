using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebUI.Data;
using WebUI.Helper;
using WebUI.Models;

namespace WebUI.Areas.Admin.Controllers;
[Area(nameof(Admin))]
[Authorize(Roles = "Admin, Moderator")]

public class ProductController(AppDbContext context, 
    IWebHostEnvironment env) : Controller
{

    public async Task<IActionResult> Index()
    {
        var products = context.Products.Include(x => x.Category).ToList();
        return View(products);  
    }

    [HttpGet]
    public IActionResult Create()
    {
        var categories = context.Categories.ToList();
        ViewBag.Categories = categories;
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]

    public async Task<IActionResult> Create(Product product, IFormFile file)
    {
        if (file == null)
        {
            //ModelState.AddModelError("error", "Image file reuired");
            ViewData["error"] = "Image file reuired";
            var categories = context.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }
        if(product.CategoryId == 0)
        {
            ViewData["error"] = "Please select category";
            var categories = context.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }
        if (!ModelState.IsValid)
        {
            var categories = context.Categories.ToList();
            ViewBag.Categories = categories;
            return View();
        }
        product.PhotoUrl = await file.SaveFileAsync(env.WebRootPath, "products");
        await context.Products.AddAsync(product);
        await context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var product = await context.Products.FindAsync(id);
        if (product == null) return NotFound();
        context.Products.Remove(product);
        await context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    public IActionResult Edit(int id)
    {
        var product = context.Products.Find(id);
        if (product == null) return NotFound();
        var categories = context.Categories.ToList();
        ViewBag.Categories = categories;
        return View(product);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Product product, IFormFile? file)
    {
        var productDb = await context.Products.FindAsync(product.Id);
        if (productDb == null) return NotFound();

        if (file != null)
            productDb.PhotoUrl = await file.SaveFileAsync(env.WebRootPath, "uploads");

        productDb.Description = product.Description;
        productDb.Price = product.Price;
        productDb.CategoryId = product.CategoryId;
        productDb.Discount = product.Discount;
        productDb.Count = product.Count;
        productDb.Name = product.Name;
        await context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
