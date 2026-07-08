namespace TaskManagement.Application.Users.Commands.CreateUser;

using MediatR;
using System.Threading;
using System.Threading.Tasks;
using System;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Application.Users.DTOs;
using TaskManagement.Domain.Repositories;
using TaskManagement.Domain.Entities;
using System.Linq;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;

    public CreateUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IPasswordHasher passwordHasher, IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task<UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        if (!await _userRepository.IsEmailUniqueAsync(request.Email, cancellationToken))
        {
            throw new Exception("Email already exists.");
        }

        var hashedPassword = _passwordHasher.Hash(request.Password);
        var user = User.Create(request.Name, request.Email, hashedPassword);

        var roleName = string.IsNullOrEmpty(request.Role) ? "User" : request.Role;
        var role = await _roleRepository.GetByNameAsync(roleName, cancellationToken);
        if (role != null)
        {
            user.AddRole(role);
        }

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

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
