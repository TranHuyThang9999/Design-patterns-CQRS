using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplicationCQRS.Domain.Entities;

public class User : BaseEntity
{
    public User(string name, string email, string password, string avatarUrl, DateTime lastPasswordChangedAt)
    {
        Name = name;
        Email = email;
        Password = password;
        AvatarUrl = avatarUrl;
        LastPasswordChangedAt = lastPasswordChangedAt;
    }

    public User()
    {
    }

    [Column(TypeName = "varchar(100)")] public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    [Column(TypeName = "varchar(100)")] public string AvatarUrl { get; set; }
    public DateTime LastPasswordChangedAt { get; set; }
}

public class UserUpdateProfile
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string AvatarUrl { get; set; }
    public string Password { get; set; }
}