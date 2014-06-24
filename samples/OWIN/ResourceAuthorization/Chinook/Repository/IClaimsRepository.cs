using System.Collections.Generic;
using System.Security.Claims;

namespace Chinook.Repository
{
    public interface IClaimsRepository
    {
        IEnumerable<Claim> GetClaimsForUser(string username);
    }
}
