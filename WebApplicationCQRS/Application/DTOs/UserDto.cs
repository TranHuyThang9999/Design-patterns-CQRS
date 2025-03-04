using System.Text.Json.Serialization;

namespace WebApplicationCQRS.Application.DTOs;

public class UserDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    [JsonIgnore]
    public string Email { get; set; }
    [JsonIgnore]
    public string AvatarUrl { get; set; }

    public UserDto() { }


    public UserDto(int id, string name, string email, string avatarUrl)
    {
        Id = id;
        Name = name;
        Email = email;
        AvatarUrl = avatarUrl;
    }
}
