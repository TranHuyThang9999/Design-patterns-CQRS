using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationCQRS.Domain.Entities;

public abstract class BaseEntity
{
    protected BaseEntity()
    {
        Id = Random.Shared.Next(1,1000);
        CreatedAt = DateOnly.FromDateTime(DateTime.Now);
        UpdatedAt = DateTime.Now;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public int Id { get; private set; }
    public DateOnly CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; set; }
    public DateTime? DeletedAt { get; set; }

    public bool IsDeleted => DeletedAt.HasValue;

    public void MarkAsDeleted()
    {
        DeletedAt = DateTime.UtcNow;
    }
}