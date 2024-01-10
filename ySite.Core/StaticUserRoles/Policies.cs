using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.Core.StaticUserRoles
{
    public static class Policies
    {
        public const string ReadPolicy = "ReadPolicy";
        public const string ReadAndWritePolicy = "AddAndReadPolicy";
        public const string FullControlPolicy = "FullControlPolicy";

        public const string GenericOwnerPolicy = "GenericOwnerPolicy";
        public const string DeletePostPolicy = "DeletePostPolicy";
        public const string EditPostPolicy = "EditPostPolicy";

        public const string EditCommentPolicy = "EditCommentPolicy";
        public const string DeleteCommentPolicy = "DeleteCommentPolicy";

        public const string DeleteReactionPolicy = "DeleteReactionPolicy";


        public const string DeleteFriendPolicy = "DeleteFriendPolicy";


        public const string FullControlOrDeleteReactionPolicy = "FullControlOrDeleteReactionPolicy";
    }
}
