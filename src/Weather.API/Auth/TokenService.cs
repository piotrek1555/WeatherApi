using API.Common.Constants;
using Infrastructure.Data.Entities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API.Auth;

public interface ITokenService
{
    string GenerateBearerToken(User user);
}   
public class TokenService: ITokenService
{
    private readonly TokenOptions _tokenOptions;

    public TokenService(IOptions<TokenOptions> tokenOptions)
    {
        _tokenOptions = tokenOptions.Value;
    }

    public string GenerateBearerToken(User user)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_tokenOptions.SigningKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var expiry = DateTimeOffset.Now.AddMinutes(_tokenOptions.AccessTokenLifetimeInMinutes);
        var userClaims = GetClaimsForUser(user);

        var securityToken = new JwtSecurityToken(
            issuer: _tokenOptions.Issuer,
            audience: _tokenOptions.Audience,
            claims: userClaims,
            notBefore: DateTime.Now,
            expires: expiry.DateTime,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(securityToken);
    }

    private IEnumerable<Claim> GetClaimsForUser(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ApiConstatns.Claims.Longitude, user.Longitude!),
            new(ApiConstatns.Claims.Latitude, user.Latitude!),
        };

        return claims;
    }
}
