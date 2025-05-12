namespace News.Models;

public class NewsModel
{
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
}

public class Source
{
    public string Uri { get; set; }
    public string DataType { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public Location Location { get; set; }
    public bool LocationValidated { get; set; }
    public Ranking Ranking { get; set; }
}

public class Location
{
    public string Type { get; set; }
    public Label Label { get; set; }
}

public class Label
{
    public string Eng { get; set; }
}

public class Ranking
{
    public int ImportanceRank { get; set; }
}

public class Concept
{
    public string Uri { get; set; }
    public string Type { get; set; }
    public int Score { get; set; }
    public Label Label { get; set; }
    public string Image { get; set; }
    public TrendingScore TrendingScore { get; set; }
    public Location Location { get; set; }
}

public class Author
{
    public string Uri { get; set; }
    public string Name { get; set; }
    public string Type { get; set; }
    public bool IsAgency { get; set; }
}

public class TrendingScore
{
    public NewsScore News { get; set; }
}

public class NewsScore
{
    public double Score { get; set; }
    public int TestPopFq { get; set; }
    public int NullPopFq { get; set; }
}

public class Category
{
    public string Uri { get; set; }
    public string Label { get; set; }
    public int Wgt { get; set; }
}

public class Video
{
    public string Uri { get; set; }
    public string Label { get; set; }
}


public class Shares
{
    public int Facebook { get; set; }
}