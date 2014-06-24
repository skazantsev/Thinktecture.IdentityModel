using System;
using System.Linq;
using System.Threading.Tasks;
using Thinktecture.IdentityModel.Owin.ResourceAuthorization;

namespace WebApp
{
    public class ChinookAuthorization : ResourceAuthorizationManager
    {
        public override Task<bool> CheckAccessAsync(ResourceAuthorizationContext context)
        {
            var resource = context.Resource.First().Value;

            if (resource == ChinookResources.Track)
            {
                return CheckTrackAccessAsync(context);
            }
            
            if (resource == ChinookResources.Album)
            {
                return CheckAlbumAccessAsync(context);
            }

            return Nok();
        }

        private Task<bool> CheckTrackAccessAsync(ResourceAuthorizationContext context)
        {
            return Eval(context.Principal.IsInRole("Admin"));
        }

        private Task<bool> CheckAlbumAccessAsync(ResourceAuthorizationContext context)
        {
            if (!context.Principal.Identity.IsAuthenticated)
            {
                return Nok();
            }

            var action = context.Action.First().Value;
            if (action == ChinookResources.AlbumActions.Edit)
            {
                return CheckAlbumEditAccessAsync(context);
            }

            return Ok();
        }

        private Task<bool> CheckAlbumEditAccessAsync(ResourceAuthorizationContext context)
        {
            if (!context.Principal.IsInRole("Admin") && !context.Principal.IsInRole("Manager"))
            {
                return Nok();
            }

            if (context.Resource.Count() == 2)
            {
                return CheckAlbumEditAccessByIdAsync(context);
            }

            return Ok();
        }

        private Task<bool> CheckAlbumEditAccessByIdAsync(ResourceAuthorizationContext context)
        {
            var id = context.Resource.Skip(1).Take(1).Single().Value;
            if (id == "1")
            {
                return Eval("bob".Equals(context.Principal.Identity.Name, StringComparison.OrdinalIgnoreCase));
            }

            return Ok();
        }
    }
}