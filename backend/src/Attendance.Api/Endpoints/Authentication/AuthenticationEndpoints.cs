using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Attendance.Domain.Entities;
using Attendance.Infrastructure.Persistence;
using global::Attendance.Infrastructure.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Attendance.Api.Endpoints.Authentication;

public static class AuthenticationEndpoints
{
    // Defines the contract matching your frontend LoginRequest models perfectly
    public record LoginRequestDto(string Email, string Password);

    // Defines the contract matching your frontend LoginResponse types perfectly
    public record LoginResponseDto(string Token, string FullName, int Role, DateTime ExpiresAt);

    public static void MapAuthenticationEndpoints(this IEndpointRouteBuilder app)
    {
        // Creates the base routing group.
        var group = app.MapGroup("/api/auth")
                       .WithTags("Authentication");

        group.MapPost("/login", async Task<Results<Ok<LoginResponseDto>, BadRequest<string>, UnauthorizedHttpResult>> (
            LoginRequestDto request,
            ApplicationDbContext context,
            IConfiguration configuration) =>
        {
            if (request == null || string.IsNullOrWhiteSpace(request.Email))
            {
                return TypedResults.BadRequest("Request payload parameters cannot be null.");
            }

            // 1. Fetch matching user entry from the PostgreSQL context
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.Email.ToLower() == request.Email.ToLower().Trim());

            if (user == null)
            {
                return TypedResults.Unauthorized();
            }

            // 2. Validate hashed password against input credentials
            var passwordHasher = new PasswordHasher();
            bool isPasswordValid = passwordHasher.VerifyPassword(request.Password, user.PasswordHash);

            if (!isPasswordValid)
            {
                return TypedResults.Unauthorized();
            }

            // 3. Extract your validated environment JWT parameters from builder configuration contexts
            var jwtSecret = configuration["Jwt:Secret"] ?? "THIS_IS_A_DEVELOPMENT_SECRET_KEY_1234567890_ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            var jwtIssuer = configuration["Jwt:Issuer"] ?? "Attendance.Api";
            var jwtAudience = configuration["Jwt:Audience"] ?? "Attendance.Client";

            // 4. Construct Security token authorizations descriptor payload mapping metadata matrices
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(jwtSecret);
            var expiresAt = DateTime.UtcNow.AddHours(2);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = expiresAt,
                Issuer = jwtIssuer,
                Audience = jwtAudience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // 5. Build clean responsive structure payload back upward to the browser client execution loop
            var fullName = $"{user.FirstName} {user.LastName}".Trim();
            var responsePayload = new LoginResponseDto(
                Token: tokenString,
                FullName: string.IsNullOrWhiteSpace(fullName) ? "System User" : fullName,
                Role: (int)user.Role,
                ExpiresAt: expiresAt
            );

            return TypedResults.Ok(responsePayload);
        });
    }
}