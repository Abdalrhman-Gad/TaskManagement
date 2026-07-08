namespace TaskManagement.Application.Users.Queries.GetCurrentUser;

using MediatR;
using System.Threading;
using System.Threading.Tasks;
using TaskManagement.Application.Users.DTOs;
using TaskManagement.Domain.Repositories;
using System.Linq;
using System;

public class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, UserDto>
{
    private readonly IUserRepository _userRepository;

    public GetCurrentUserQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);
        
        if (user == null)
        {
            throw new Exception("User not found.");
        }

        return new UserDto
        {
            Id = user.Id,
            Name = user.Name,
            Email = user.Email,
            Role = user.UserRoles?.FirstOrDefault()?.Role?.Name,
            CreatedAt = user.CreatedAt
        };
    }
}
