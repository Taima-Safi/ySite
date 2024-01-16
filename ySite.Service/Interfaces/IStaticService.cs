using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ySite.Core.Helper;

namespace ySite.Service.Interfaces
{
    public interface IStaticService
    {
        ValidationResult AllowUplaod(IFormFile? ClientFile);
    }
}
