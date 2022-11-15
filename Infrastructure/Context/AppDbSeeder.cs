using Application.Contracts.Repository;
using Domain.Aggregators.Project;
using Infrastructure.Identity;
using Infrastructure.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SharedLibrary.Authentication;

namespace Infrastructure.Context;

public class AppDbSeeder
{
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<AppDbSeeder> _logger;
    private readonly IRepository<Project> _projectRepository;
    private readonly IRepository<Tag> _tagRepository;
    private readonly SecuritySettings _securitySettings;
    private readonly MockDataSettings _mockDataSettings;
    private readonly string _lorem = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. \r\nEtiam eu turpis molestie, dictum est a, mattis tellus. Sed dignissim, metus nec fringilla accumsan, risus sem sollicitudin lacus, ut interdum tellus elit sed risus. \r\n";
    public AppDbSeeder(
        RoleManager<IdentityRole> roleManager,
        UserManager<AppUser> userManager,
        ILogger<AppDbSeeder> logger,
        IOptions<SecuritySettings> securitySettings,
        IRepository<Project> projectRepository,
        IRepository<Tag> tagRepository,
        IOptions<MockDataSettings> mockDataOptions)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _logger = logger;
        _projectRepository = projectRepository;
        _tagRepository = tagRepository;
        _securitySettings = securitySettings.Value;
        _mockDataSettings = mockDataOptions.Value;
    }

    public async Task SeedDataAsync()
    {
        foreach (var role in SHRoles.DefaultRoles)
        {
            if (await _roleManager.Roles.SingleOrDefaultAsync(x => x.Name == role) == null)
            {
                await _roleManager.CreateAsync(new()
                {
                    Name = role
                });
                _logger.LogInformation($"Role {role} created");
            }
        }

        if (await _userManager.Users.SingleOrDefaultAsync(x => x.Email == _securitySettings.RootUserEmail) == null)
        {
            var adminUser = AppUser.Create(_securitySettings.RootUserEmail, SHRoles.Admin, SHRoles.Admin, null, null);
            await _userManager.CreateAsync(adminUser, _securitySettings.DefaultPassword);
            await _userManager.AddToRoleAsync(adminUser, SHRoles.Admin);
            await _userManager.AddToRoleAsync(adminUser, SHRoles.Dev);
            _logger.LogInformation($"Root user {adminUser.Email} created");
        }
        if (!await _tagRepository.AnyAsync())
        {
            var tags = new List<Tag>()
            {
                Tag.Create("Web"),
                Tag.Create("Desktop"),
                Tag.Create("Android"),
                Tag.Create("Test")
            };

            await _tagRepository.AddRangeAsync(tags);

            if (_mockDataSettings.AddProjects && !await _projectRepository.AnyAsync())
            {

                var projects = new List<Project>();
                for (int i = 0; i < 10; i++)
                {
                    projects.Add(Project.Create($"Mobile app #{i}", _lorem, "Client.exe", _lorem, tags.Skip(2).Take(1).ToList()));
                }

                for (int i = 10; i < 20; i++)
                {
                    projects.Add(Project.Create($"Desktop app #{i}", _lorem, "Client.exe", _lorem, tags.Skip(1).Take(1).ToList()));
                }
                for (int i = 20; i < 30; i++)
                {
                    projects.Add(Project.Create($"Service web app #{i}", _lorem, "Client.exe", _lorem, tags.Take(1).ToList()));
                }
                for (int i = 30; i < 40; i++)
                {
                    projects.Add(Project.Create($"Desktop test app #{i}", _lorem, "Client.exe", _lorem, tags.Skip(1).Take(1).Concat(tags.Skip(3).Take(1)).ToList()));
                }
                await _projectRepository.AddRangeAsync(projects);
            }
        }



    }
}
