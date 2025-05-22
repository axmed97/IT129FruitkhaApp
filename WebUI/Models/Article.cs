namespace WebUI.Models;

public class Article
{
    public int Id { get; set; }
    public string Title { get; set; }

    public string Content { get; set; }
    public string? PhotoUrl { get; set; }
    public DateTime CreatedDate { get; set; }
    public List<ArticleTag> ArticleTags { get; set; }
    public int ArticleCategoryId { get; set; }
    public ArticleCategory ArticleCategory { get; set; }
}
