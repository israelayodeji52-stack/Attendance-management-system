using Attendance.Application.Features.Authentication.Commands;
using Attendance.Contracts.Authentication;
using MediatR;

namespace Attendance.Api.Endpoints.Authentication;

public static class AuthenticationEndpoints
{
    public static IEndpointRouteBuilder MapAuthenticationEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/auth")
            .WithTags("Authentication");

        group.MapPost("/login", Login);

        return app;
    }

    private static async Task<IResult> Login(
        LoginRequest request,
        ISender sender,
        CancellationToken cancellationToken)
    {
        var command = new LoginCommand(
            request.Email,
            request.Password
        );

        var response = await sender.Send(command, cancellationToken);

        return Results.Ok(response);
    }
}
