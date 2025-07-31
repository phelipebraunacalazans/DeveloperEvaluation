namespace Ambev.DeveloperEvaluation.Common.Security;

public interface ICurrentUserAccessor
{
    /// <summary>
    /// Retrieves current system´s user.
    /// </summary>
    /// <returns>User of the system.</returns>
    IUser GetCurrentUser();
}