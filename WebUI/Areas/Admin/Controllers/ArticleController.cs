using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebUI.Data;
using WebUI.Helper;
using WebUI.Models;

namespace WebUI.Areas.Admin.Controllers;
[Area(nameof(Admin))]
public class ArticleController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public ArticleController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
    }

    public async Task<IActionResult> Index()
    {
        var articles = await _context.Articles
            .Include(x => x.ArticleCategory)
            .Include(x => x.ArticleTags)
            .ThenInclude(x => x.Tag)
            .ToListAsync();
        return View(articles);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        var categories = await _context.ArticleCategories.ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        var tags = await _context.Tags.ToListAsync();
        ViewData["Tags"] = tags;
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Article article, IFormFile file, List<int> tags)
    {
        article.CreatedDate = DateTime.Now;
        article.PhotoUrl = await file.SaveFileAsync(_env.WebRootPath, "uploads");
        await _context.AddAsync(article);
        await _context.SaveChangesAsync();

        for (int i = 0; i < tags.Count; i++)
        {
            ArticleTag articleTag = new()
            {
                ArticleId = article.Id,
                TagId = tags[i]
            };
            await _context.AddAsync(articleTag);
        }

        await _context.SaveChangesAsync();

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> Edit(int id)
    {
        var categories = await _context.ArticleCategories.ToListAsync();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        var tags = await _context.Tags.ToListAsync();
        ViewData["Tags"] = tags;
        var article = await _context.Articles
            .Include(x => x.ArticleCategory)
            .Include(x => x.ArticleTags)
            .ThenInclude(x => x.Tag)
            .FirstOrDefaultAsync(x => x.Id == id);

        return View(article);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(Article article, IFormFile file, List<int> tags)
    {
        var articleDb = await _context.Articles
            .Include(x => x.ArticleCategory)
            .Include(x => x.ArticleTags)
            .ThenInclude(x => x.Tag)
            .FirstOrDefaultAsync(x => x.Id == article.Id);

        if (file != null)
            articleDb.PhotoUrl = await file.SaveFileAsync(_env.WebRootPath, "uploads");
        else
            article.PhotoUrl = articleDb.PhotoUrl;

        articleDb.Title = article.Title;
        articleDb.Content = article.Content;
        
        articleDb.ArticleCategoryId = article.ArticleCategoryId;
        await _context.SaveChangesAsync();

        var articleTags = await _context.ArticleTags.Where(x => x.ArticleId ==  article.Id).ToListAsync();
        _context.RemoveRange(articleTags);

        for (int i = 0; i < tags.Count; i++)
        {
            ArticleTag articleTag = new()
            {
                ArticleId = article.Id,
                TagId = tags[i]
            };
            await _context.AddAsync(articleTag);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}
