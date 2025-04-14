using Microsoft.EntityFrameworkCore;
using WebUI.Models;

namespace WebUI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Category> Categories { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Testimonial> Testimonials { get; set; }
}
