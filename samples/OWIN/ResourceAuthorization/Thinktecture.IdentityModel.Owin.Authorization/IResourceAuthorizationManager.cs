using System.Threading.Tasks;

namespace Thinktecture.IdentityModel.Owin.Authorization
{
    public interface IResourceAuthorizationManager
    {
        Task<bool> CheckAccessAsync(ResourceAuthorizationContext context);
    }
}