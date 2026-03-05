namespace News.Entities;

public class Category: BaseEntity
{
    public string Name { get; set; } = null!;
    public ICollection<Article> Articles { get; set; }
}