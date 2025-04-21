using System.ComponentModel.DataAnnotations;

namespace WebUI.Models;

public class Slider
{
    public int Id { get; set; }
    [Required, MaxLength(100), MinLength(1)]
    public string SubTitle { get; set; }
    public string Title { get; set; }
    public string? PhotoUrl { get; set; }
}
