using Auth.Infrastructure.Persistence.Entities;
using Bogus;
using Bogus.Extensions;
using Microsoft.AspNetCore.Identity;

namespace Auth.Infrastructure.Persistence.Seeding.Fakers;

public sealed class AuthUserFaker : Faker<AuthUser>
{
    public AuthUserFaker(UserManager<AuthUser> userManager)
    {
        RuleFor(u => u.UserName,
                f => string
                        .Concat(f.Internet
                                        .UserName()
                                        .ClampLength(max: 10),
                                Guid.NewGuid().ToString("N")));
        RuleFor(u => u.Email,
                (f, u) => f.Internet
                    .Email(firstName: u.UserName));

        RuleFor(u => u.NormalizedUserName,
                (_, u) => userManager.NormalizeName(u.UserName));

        RuleFor(u => u.NormalizedEmail,
                (_, u) => userManager.NormalizeName(u.Email));

        RuleFor(u => u.PasswordHash,
                (_, u) => userManager.PasswordHasher
                    .HashPassword(u, "12345678Ab!"));

        RuleFor(u => u.SecurityStamp,
                () => Guid.NewGuid().ToString("N"));

        RuleFor(u => u.ConcurrencyStamp,
                () => Guid.NewGuid().ToString());

        RuleFor(u => u.CreatedAt,
                f => f.Date
                        .Recent(10, DateTime.UtcNow.AddDays(-30)));

        RuleFor(u => u.UpdatedAt,
                (f, t) => f.Date
                        .Recent(9, t.CreatedAt)
                        .OrNull(f, 0.2F));
    }
}
