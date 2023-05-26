﻿namespace Prohod.Infrastructure.Users.Authentication.Options;

public record AuthenticationOptions
{
    public required string Issuer { get; init; }
    
    public required string Audience { get; init; }
    
    public required string SigningKey { get; init; }
}