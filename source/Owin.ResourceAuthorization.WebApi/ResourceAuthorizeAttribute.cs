/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see LICENSE
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
            var actions = new List<Claim>();
            
            var action = ActionFromAttribute();
            if (action != null) actions.Add(action);
            
            actions.Add(actionContext.ActionFromController());

            var resources = new List<Claim>();
            var resourceList = ResourcesFromAttribute();
            if (resourceList != null) resources.AddRange(resourceList);
            resources.AddRange(actionContext.ResourceFromController());

            // filter "controller" since we're already adding it explicitly in the above code
            var routeClaims = actionContext.ResourcesFromRouteParameters().Where(x => x.Type != "controller");
            resources.AddRange(routeClaims);

            return CheckAccess(actionContext.Request, actions.ToArray(), resources.Distinct(new ClaimComparer()).ToArray());
        }

        protected virtual bool CheckAccess(HttpRequestMessage request, Claim[] actions, params Claim[] resources)
        {
            var task = request.CheckAccessAsync(actions, resources);

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

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            if (actionContext.ControllerContext.RequestContext.Principal != null &&
                actionContext.ControllerContext.RequestContext.Principal.Identity != null &&
                actionContext.ControllerContext.RequestContext.Principal.Identity.IsAuthenticated)
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Forbidden, "Forbidden");
            }
            else
            {
                actionContext.Response = actionContext.ControllerContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "Unauthorized");
            }
        }
    }
}