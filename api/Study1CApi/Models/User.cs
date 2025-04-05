using System.Text.Json.Serialization;

namespace Study1CApi.Models;

public partial class User
{
    public Guid UserId { get; set; }

    public string UserSurname { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string? UserPatronymic { get; set; }

    public bool IsFirst { get; set; }

    public virtual ICollection<Course> Courses { get; set; } = new List<Course>();

    public virtual ICollection<UsersTask> UsersTasks { get; set; } = new List<UsersTask>();

    [JsonIgnore]
    public AuthUser? AuthUserNavigation { get; set; }
}
