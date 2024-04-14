using System.Security.Claims;
using ySite.Core.Helper;
using ySite.Core.StaticUserRoles;

namespace ySite.Service.Authorization;

public class AuthorizeHelper
{
    public static IEnumerable<int> GetPermissionFromClaim(string controllerName, IEnumerable<Claim> claims)
    {
        if (!claims.Any(c => c.Type == controllerName))
            return null;
        return claims.Where(c => c.Type == controllerName).Select(c => c.Value.To<int>());
    }

    public static int GetActionPermission(string actionName)
    {
        switch (actionName)
        {
            case "Get":
                return Permissions.Permission.Read;
            case "Add":
                return Permissions.Permission.Write;
            case "Update":
                return Permissions.Permission.Update;
            case "Delete":
                return Permissions.Permission.Delete;
            default:
                return Permissions.Permission.None;
        }
    }
}
