namespace TaskManagement.Domain.Repositories;

using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Domain.Entities;

public interface IRoleRepository : IRepository<Role>
{
    Task<Role?> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}
