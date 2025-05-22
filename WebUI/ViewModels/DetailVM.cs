using WebUI.Models;

namespace WebUI.ViewModels;

public class DetailVM
{
    public Product Product { get; set; }
    public List<Product> ReleatedProduct { get; set; }
}
