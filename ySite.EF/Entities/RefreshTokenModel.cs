﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ySite.EF.Entities
{
    [Owned]
    public class RefreshTokenModel
    {
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;
        public DateTime CreatedOn { set; get; }
        public DateTime? RevokedOn { set; get; }
        public bool IsActive => RevokedOn == null && !IsExpired;
    }
}
