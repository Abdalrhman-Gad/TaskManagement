namespace TaskManagement.Infrastructure.Persistence;

using Microsoft.EntityFrameworkCore;
using System.Linq;
using TaskManagement.Application.Common.Interfaces;
using TaskManagement.Domain.Entities;

public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly IPasswordHasher _passwordHasher;

    public DatabaseSeeder(ApplicationDbContext context, IPasswordHasher passwordHasher)
    {
        _context = context;
        _passwordHasher = passwordHasher;
    }

    public async System.Threading.Tasks.Task SeedAsync()
    {
        await _context.Database.MigrateAsync();

        if (!await _context.Roles.AnyAsync(r => r.Name == "Admin"))
        {
            var adminRole = Role.Create("Admin");
            await _context.Roles.AddAsync(adminRole);
            await _context.SaveChangesAsync();
        }

        if (!await _context.Roles.AnyAsync(r => r.Name == "User"))
        {
            var userRole = Role.Create("User");
            await _context.Roles.AddAsync(userRole);
            await _context.SaveChangesAsync();
        }

        if (!await _context.Users.AnyAsync(u => u.Email == "admin@example.com"))
        {
            var adminRole = await _context.Roles.FirstAsync(r => r.Name == "Admin");
            var adminUser = User.Create("Admin", "admin@example.com", _passwordHasher.Hash("Admin@123"));
            adminUser.AddRole(adminRole);

            await _context.Users.AddAsync(adminUser);
            await _context.SaveChangesAsync();
        }
    }
}
