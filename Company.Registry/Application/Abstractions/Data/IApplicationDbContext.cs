using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Company> Companies { get; set; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}