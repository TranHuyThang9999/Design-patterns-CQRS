namespace WebApplicationCQRS.Domain.Entities;

public class Ticket : BaseEntity
{
    public int CreatorId { get; set; }
    public string Name { get; set; }
    public string FileDescription {set;get;}
    public Ticket(){}
    public Ticket(int creatorId, string name, string fileDescription)
    {
        CreatorId = creatorId;
        Name = name;
        FileDescription = fileDescription;
    }
    
}