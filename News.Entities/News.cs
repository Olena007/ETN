using System.ComponentModel.DataAnnotations;

namespace News.Entities;

public class News
{
    [Key]
    public Guid NewsId { get; set; }

    public string Uri { get; set; }
    public string Lang { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public string DateTime { get; set; }
    public double Sim { get; set; }
    public string Url { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public List<string> Links { get; set; }
    public string? Image { get; set; }
    public string? EventUri { get; set; }
    public ICollection<View> Views { get; set; }
    public ICollection<Author> Authors { get; set; }
    public ICollection<Category> Categories { get; set; }
    public ICollection<Source> Source { get; set; }
    public ICollection<Video> Videos { get; set; }
    public ICollection<Location> Location { get; set; }
}

public class Source
{
    [Key] public Guid SourceId { get; set; }

    public Guid NewsId { get; set; }
    public string Uri { get; set; }
    public string? DataType { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public Location? Location { get; set; }
    public bool? LocationValidated { get; set; }
    public News News { get; set; }
}

public class Location
{
    [Key] 
    public Guid LocationId { get; set; }

    public Guid NewsId { get; set; }
    public string? Type { get; set; }
    public News News { get; set; }
    public ICollection<Country> Country { get; set; }
}

public class Country
{
    [Key] public Guid CountryId { get; set; }

    public Guid LocationId { get; set; }
    public string? Type { get; set; }
    public Location Location { get; set; }
}

public class Category
{
    [Key] public Guid CategoryId { get; set; }

    public Guid NewsId { get; set; }
    public string Uri { get; set; }
    public string? Label { get; set; }
    public int? Wgt { get; set; }
    public News News { get; set; }
}

public class Author
{
    [Key] public Guid AuthorId { get; set; }

    public Guid NewsId { get; set; }
    public string Uri { get; set; }
    public string? Name { get; set; }
    public string? Type { get; set; }
    public bool? IsAgency { get; set; }
    public News News { get; set; }
}

public class Video
{
    [Key] public Guid VideoId { get; set; }

    public Guid NewsId { get; set; }
    public string Uri { get; set; }
    public string? Label { get; set; }
    public News News { get; set; }
}