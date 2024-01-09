using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.Core.Dtos.FriendShip
{
    public class FriendShipDto
    {
        public string Message { get; set; }
        public List<friendDto> Friends { get; set; }
    }

    public class friendDto
    {
        public string FriendId { get; set; }
        public string UserName { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
