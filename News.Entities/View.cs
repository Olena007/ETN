namespace News.Entities;

public class View
{
    public Guid? ViewId { get; set; }
    public DateTime? ViewAt { get; set; }
    public Guid? UserId { get; set; }
    public string Uri { get; set; }
    public User Users { get; set; }
    public News News { get; set; }
}