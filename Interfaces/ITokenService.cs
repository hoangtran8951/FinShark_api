using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiSTockapi.Models;

namespace apiSTockapi.Interfaces
{
    public interface ITokenService
    {
        string CreateToken(AppUser appUser);
    }
}