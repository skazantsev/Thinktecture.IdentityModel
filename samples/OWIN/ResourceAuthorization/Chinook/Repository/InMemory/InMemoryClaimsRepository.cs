using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Chinook.Repository.InMemory
{
    public class InMemoryClaimsRepository : IClaimsRepository
    {
        public IEnumerable<Claim> GetClaimsForUser(string username)
        {
            if (String.IsNullOrWhiteSpace(username))
            {
                return Enumerable.Empty<Claim>();
            }

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, username)
            };

            if (username == "alice")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Manager"));
            }

            if (username == "bob")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
            }
            return claims;
        }
    }
}
