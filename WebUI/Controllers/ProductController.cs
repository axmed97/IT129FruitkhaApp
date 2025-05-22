using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebUI.Data;
using WebUI.ViewModels;

namespace WebUI.Controllers;
public class ProductController : Controller
{
    private readonly AppDbContext _context;

    public ProductController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Detail(int id)
    {
        var product = _context.Products.Include(x => x.Category)
            .FirstOrDefault(x => x.Id == id);
        
        var releated = _context.Products
            .Where(x => x.CategoryId == product.CategoryId && x.Id != product.Id)
            //.Except(product)
            .Take(3).ToList();

        DetailVM detailVM = new()
        {
            Product = product,
            ReleatedProduct = releated,
        };

        return View(detailVM);
    }


    
}
