namespace News.Models;

public class EntitiesModel
{
    public List<EntityModel> Persons { get; set; } = new();
    public List<EntityModel> Organizations { get; set; } = new();
    public List<EntityModel> Locations { get; set; } = new();
}