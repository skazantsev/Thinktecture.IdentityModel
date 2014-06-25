/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see LICENSE
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Thinktecture.IdentityModel.WebApi
{
    public class ResourceAuthorizeAttribute : AuthorizeAttribute
    {
        private string _action;
        private string[] _resources;

        public ResourceAuthorizeAttribute()
        { }

        public ResourceAuthorizeAttribute(string action, params string[] resources)
        {
            _action = action;
            _resources = resources;
        }

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var action = ActionFromAttribute() ?? actionContext.ActionFromController();
            var resources = ResourcesFromAttribute() ?? actionContext.ResourceFromController();

            resources.AddRange(actionContext.ResourcesFromRouteParameters());

            return CheckAccess(actionContext.Request, action, resources.ToArray());
        }

        protected virtual bool CheckAccess(HttpRequestMessage request, Claim action, params Claim[] resources)
        {
            var task = request.CheckAccessAsync(new[] { action } , resources);

            if (task.Wait(5000))
            {
                return task.Result;
            }
            else
            {
                throw new TimeoutException();
            }
        }

        private Claim ActionFromAttribute()
        {
            return !string.IsNullOrWhiteSpace(_action) ? new Claim("name", _action) : null;
        }

        private List<Claim> ResourcesFromAttribute()
        {
            if ((_resources != null) && (_resources.Any()))
            {
                return _resources.Select(r => new Claim("name", r)).ToList();
            }

            return null;
        }
    }
}