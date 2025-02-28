using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace WebApplicationCQRS.Domain.Entities;

public class User
{
    public User(int id, string name, string email, string password, DateOnly created, DateTime updated, DateTime lastPasswordChangedAt)
    {
        Id = id;
        Name = name;
        Email = email;
        Password = password;
        Created = created;
        Updated = updated;
        LastPasswordChangedAt = lastPasswordChangedAt;
    }
    public User(){}
    
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }

    [Column(TypeName = "varchar(100)")] public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateOnly Created { get; set; }
    public DateTime Updated { get; set; }
    public DateTime LastPasswordChangedAt { get; set; }
}