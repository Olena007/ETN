namespace News.Entities;

public class Views
{
    public Guid? ViewId { get; set; }
    public DateTime? ViewAt { get; set; }
    public Guid? UserId { get; set; }
    public User Users { get; set; }
}