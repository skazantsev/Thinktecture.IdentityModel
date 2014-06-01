/*
 * Copyright (c) Dominick Baier, Brock Allen.  All rights reserved.
 * see LICENSE
 */

using System;
using System.Net.Http;
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
            if (!string.IsNullOrWhiteSpace(_action))
            {
                return CheckAccess(actionContext.Request, _action, _resources);
            }
            else
            {
                var action = actionContext.ActionDescriptor.ActionName;
                var resource = actionContext.ControllerContext.ControllerDescriptor.ControllerName;

                return CheckAccess(actionContext.Request, action, resource);
            }
        }

        protected virtual bool CheckAccess(HttpRequestMessage request, string action, params string[] resources)
        {
            var task = request.CheckAccessAsync(_action, _resources);
            
            if (task.Wait(5000))
            {
                return task.Result;
            }
            else
            {
                throw new TimeoutException();
            }
        }
    }
}