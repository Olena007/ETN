namespace News.Models;

public class IndexArticlesModel
{
    public int TotalCount { get; set; }
    public int Processed { get; set; }
    public int FailedCount { get; set; }
    public List<Guid> FailedIds { get; set; }
}