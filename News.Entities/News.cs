using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace News.Entities;

public class News
{
    [Key]
    public string Uri { get; set; }
    public string Lang { get; set; }
    public string Date { get; set; }
    public string Time { get; set; }
    public string DateTime { get; set; }
    public double Sim { get; set; }
    public string Url { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public Source Source { get; set; }
    public List<Author> Authors { get; set; }
    public List<Concept> Concepts { get; set; }
    public List<Category> Categories { get; set; }
    public List<string> Links { get; set; }
    public List<Video> Videos { get; set; }
    public string Image { get; set; }
    public string EventUri { get; set; }
    public Location Location { get; set; }
    public Shares Shares { get; set; }
    public ICollection<View> Views { get; set; }
}

public class Source
{
    [Key]
    public string Uri { get; set; }
    public string DataType { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Location Location { get; set; }
    public bool LocationValidated { get; set; }
    public Ranking Ranking { get; set; }
}

[Owned]
public class Ranking
{
    public int ImportanceRank { get; set; }
}

[Owned]
public class Location
{
    public string Type { get; set; }
    public Label Label { get; set; }
    public Country Country { get; set; }
}

[Owned]
public class Country
{
    public string Type { get; set; }
    public Label Label { get; set; }
}

[Owned]
public class Label
{
    public string Eng { get; set; }
}

public class Concept
{
    [Key]
    public string Uri { get; set; }
    public string Type { get; set; }
    public double Score { get; set; }
    public Label Label { get; set; }
    public string Image { get; set; }
    [NotMapped]
    public Dictionary<string, List<string>> Synonyms { get; set; }
    public TrendingScore TrendingScore { get; set; }
    public Location Location { get; set; }
}

[Owned]
public class TrendingScore
{
    public NewsScore News { get; set; }
}

[Owned]
public class NewsScore
{
    public double Score { get; set; }
    public int TestPopFq { get; set; }
    public int NullPopFq { get; set; }
}

public class Category
{
    [Key]
    public string Uri { get; set; }
    public string Label { get; set; }
    public int Wgt { get; set; }
}

public class Author
{
    [Key]
    public string Uri { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public bool IsAgency { get; set; }
}

public class Video
{
    [Key]
    public string Uri { get; set; }
    public string Label { get; set; }
}

[Owned]
public class ExtractedDate
{
    public bool Amb { get; set; }
    public bool Imp { get; set; }
    public string Date { get; set; }
    public string DateEnd { get; set; }
    public int TextStart { get; set; }
    public int TextEnd { get; set; }
}

[Owned]
public class Shares
{
    public int Facebook { get; set; }
}
