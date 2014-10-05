using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Thinktecture.IdentityModel.WebApi;

namespace WebApp.Api
{
    public class AlbumController : ApiController
    {
        [ResourceAuthorize(ChinookResources.AlbumActions.View, ChinookResources.Album)]
        public IHttpActionResult Get()
        {
            return Ok();
        }

        public async Task<IHttpActionResult> Get(int id)
        {
            if (!(await Request.CheckAccessAsync(ChinookResources.AlbumActions.View, 
                                                ChinookResources.Album,
                                                id.ToString())))
            {
                return this.AccessDenied();
            }

            return Ok();
        }
    }
}
