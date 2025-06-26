using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Data;

public sealed class AppUser : IdentityUser<Guid>
{
    public AppUser()
    {
        Id = Guid.NewGuid();
    }

    public AppUser(string email) : this()
    {
        Email = email;
        UserName = email;
        NormalizedEmail = email.ToUpperInvariant();
        NormalizedUserName = email.ToUpperInvariant();
    }
}

public class ApplicationIdentityDbContext(DbContextOptions<ApplicationIdentityDbContext> options)
    : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>(options);