using System;
using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Owin.ResourceAuthorization
{
    public abstract class ResourceAuthorizationManager : IResourceAuthorizationManager
    {
        public virtual Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Ok()
        {
            return Task.FromResult(true);
        }

        public Task<bool> Nok()
        {
            return Task.FromResult(false);
        }
    }
}