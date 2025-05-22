namespace WebUI.Models;

public class ArticleCategory
{
    public int Id { get; set; }
    public string Name { get; set; }
    public List<Article> Articles { get; set; }
}
