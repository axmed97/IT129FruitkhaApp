using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Data;

namespace WebUI.Controllers;
public class ShopController : Controller
{
    private readonly AppDbContext _context;

    public ShopController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var categories = _context.Categories.ToList();
        ViewBag.Categories = categories;
        var products = _context.Products.Include(x => x.Category).ToList();
        return View(products);
    }
}
