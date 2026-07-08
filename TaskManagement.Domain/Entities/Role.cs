namespace TaskManagement.Domain.Entities;

using TaskManagement.Domain.Common.Models;

public class Role : Entity
{
    public string Name { get; private set; } = string.Empty;

    private Role() { }

    private Role(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public static Role Create(string name)
    {
        return new Role(Guid.NewGuid(), name);
    }
}
