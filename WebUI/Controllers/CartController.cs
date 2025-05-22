using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
using System.Text.Json;
using WebUI.Data;
using WebUI.DTOs;
using WebUI.Enums;
using WebUI.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebUI.Controllers;
public class CartController : Controller
{
    private readonly AppDbContext _context;

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult AddToBasket(int id, int quantity)
    {
        var cartCookie = Request.Cookies["cart"];

        List<CartCookieDto> cartCookies = new();

        CartCookieDto cartCookieDto = new()
        {
            Id = id,
            Quantity = quantity
        };

        if (cartCookie == null)
        {
            cartCookies.Add(cartCookieDto);
            var cookiejson = JsonSerializer.Serialize<List<CartCookieDto>>(cartCookies); // json
            Response.Cookies.Append("cart", cookiejson);
        }
        else
        {
            var data = JsonSerializer.Deserialize<List<CartCookieDto>>(cartCookie);
            var findData = data.FirstOrDefault(x => x.Id == id);

            if (findData != null)
            {
                findData.Quantity += quantity;
            }
            else
            {
                data.Add(cartCookieDto);
            }
            var cookiejson = JsonSerializer.Serialize(data);
            Response.Cookies.Append("cart", cookiejson);

        }

        var x = new
        {
            a = 5
        };
        return Json(new { success = true });
    }

    public IActionResult Cart()
    {
        return View();
    }

    public IActionResult GetProducts()
    {
        var cartCookie = Request.Cookies["cart"];
        var data = JsonSerializer.Deserialize<List<CartCookieDto>>(cartCookie);

        var quantity = data.Select(x => x.Quantity).ToList();
        var dataIds = data.Select(x => x.Id).ToList();

        var products = _context.Products.Where(x => dataIds.Contains(x.Id)).ToList();

        var productsData = new
        {
            quantity = quantity,
            products = products
        };

        return Json(productsData);
    }

    public IActionResult Checkout()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Checkout(Order order)
    {
        await _context.AddAsync(order);
        await _context.SaveChangesAsync();

        var cartCookie = Request.Cookies["cart"];
        var datas = JsonSerializer.Deserialize<List<CartCookieDto>>(cartCookie);

        var dataIds = datas.Select(x => x.Id).ToList();
        var qtys = datas.Select(x => x.Quantity).ToList();

        var products = _context.Products.Where(x => dataIds.Contains(x.Id)).ToList();

        List<OrderItem> orderItems = new();
        for (var i = 0; i < qtys.Count; i++)
        {
            orderItems.Add(new OrderItem
            {
                OrderId = order.Id,
                Name = products[i].Name,
                OrderStatus = (sbyte)OrderStatus.Pending,
                Price = products[i].Price,
                Quantity = qtys[i]
            });
        }

        await _context.AddRangeAsync(orderItems);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index", "Home");
    }
}
