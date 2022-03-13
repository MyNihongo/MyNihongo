using Microsoft.IdentityModel.Tokens;

namespace MyNihongo.WebApi.Models;

public sealed record ConfigurationTokenOptions(TimeSpan Expires, SecurityKey SigningKey);