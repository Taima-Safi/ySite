using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.Core.StaticUserRoles
{
    public static class Permissions
    {
        public static class Permission
        {
            public const int None = 0;
            public const int Read = 1;
            public const int Write = 2;
            public const int Update = 3;
            public const int Delete = 4;
        }

        //public static List<string> GeneratePermissionsList(string module, List<string> excludedPermissions = null)
        //{
        //    var allPermissions =  new List<string>()
        //    {
        //        $"Permissions.{module}.View",
        //        $"Permissions.{module}.Create",
        //        $"Permissions.{module}.Edit",
        //        $"Permissions.{module}.Delete"
        //    };

        //    if (excludedPermissions != null)
        //    {
        //        foreach (var excludedPermission in excludedPermissions)
        //        {
        //            allPermissions.Remove($"Permissions.{module}.{excludedPermission}");
        //        }
        //    }
        //    return allPermissions;
        //}

    }
}
