using Ambev.DeveloperEvaluation.Common.Security;
using System;

namespace Ambev.DeveloperEvaluation.WebIntegrationTesting.Security;

public class UserClaims : IUser
{
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}