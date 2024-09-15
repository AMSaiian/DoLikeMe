using AMSaiian.Shared.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Auth.Infrastructure.Persistence.Entities;

public class AuthUser : IdentityUser<Guid>, IAudited
{
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
