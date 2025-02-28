using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Runtime.CompilerServices;

namespace WebApplicationCQRS.Domain.Entities;

public class User
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; set; }
    [Column(TypeName = "varchar(100)")]
    [IndexerName(nameof(Name))]
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public DateOnly Created { get; set; }
    public DateTime Updated { get; set; }
    public DateTime LastPasswordChangedAt { get; set; }
}