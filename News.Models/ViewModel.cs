namespace News.Models;

public class ViewModel
{
        public Guid? ViewId { get; set; }
        public DateTime? ViewAt { get; set; }
        public Guid? UserId { get; set; }
        public Guid? NewsId { get; set; }
}