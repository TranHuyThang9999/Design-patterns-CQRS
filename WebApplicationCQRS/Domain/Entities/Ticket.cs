namespace WebApplicationCQRS.Domain.Entities;

public class Ticket : BaseEntity
{
    public int CreatorId { get; set; }
    public string Name { get; set; }
    public string FileDescription {set;get;}
    
    public string Description { get; set; }
    public Ticket(){}

    public Ticket(int creatorId, string name, string fileDescription, string description)
    {
        CreatorId = creatorId;
        Name = name;
        FileDescription = fileDescription;
        Description = description;
    }
}