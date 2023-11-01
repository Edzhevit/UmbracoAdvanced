using System.Collections.Immutable;
using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;
using OpenIddict.Server.AspNetCore;

namespace UmbracoAdvanced.Web.Controllers;

[ApiController]
[Route("connect/{action}")]
public class AuthTokenController : ControllerBase
{
    private readonly IOpenIddictScopeManager _scopeManager;
    private readonly IOpenIddictApplicationManager _applicationManager;

    public AuthTokenController(IOpenIddictScopeManager scopeManager, IOpenIddictApplicationManager applicationManager)
    {
        _scopeManager = scopeManager;
        _applicationManager = applicationManager;
    }

    [HttpPost]
    public async Task<IActionResult> Token()
    {
        var request = HttpContext.GetOpenIddictServerRequest() ?? throw new InvalidOperationException("Invalid request");
        if (request.IsClientCredentialsGrantType())
        {
            var application = await _applicationManager.FindByClientIdAsync(request.ClientId);
            if (application == null)
            {
                throw new InvalidOperationException("Client was not found in the database.");
            }

            var identity = new ClaimsIdentity(authenticationType: TokenValidationParameters.DefaultAuthenticationType,
                nameType:OpenIddictConstants.Claims.Name, roleType:OpenIddictConstants.Claims.Role);

            identity
                .SetClaim(OpenIddictConstants.Claims.Subject, request.ClientId)
                .SetClaim(OpenIddictConstants.Claims.Name, await _applicationManager.GetDisplayNameAsync(application))
                .SetClaims(OpenIddictConstants.Claims.Role, new[] {"ClientApplication"}.ToImmutableArray());

            identity.SetScopes(request.GetScopes());
            identity.SetResources(await _scopeManager.ListResourcesAsync(identity.GetScopes()).ToListAsync());
            identity.SetDestinations(GetDestinations);

            return SignIn(new ClaimsPrincipal(identity), OpenIddictServerAspNetCoreDefaults.AuthenticationScheme);
        }

        throw new InvalidOperationException("The grant type is not supported.");
    }

    private IEnumerable<string> GetDestinations(Claim claim)
    {
        return claim.Type switch
        {
            OpenIddictConstants.Claims.Name or OpenIddictConstants.Claims.Subject => new[]
            {
                OpenIddictConstants.Destinations.AccessToken, OpenIddictConstants.Destinations.IdentityToken
            },

            _ => new[] { OpenIddictConstants.Destinations.AccessToken }
        };
    }
}