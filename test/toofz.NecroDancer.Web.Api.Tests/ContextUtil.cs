// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved. See License.txt in the project root for license information.

// Copyright(c) Microsoft Open Technologies, Inc.All rights reserved.
// Microsoft Open Technologies would like to thank its contributors, a list
// of whom are at http://aspnetwebstack.codeplex.com/wikipage?title=Contributors.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License. You may
// obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied. See the License for the specific language governing permissions
// and limitations under the License.

using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.Http.Routing;
using Moq;

namespace System.Web.Http
{
    internal static class ContextUtil
    {
        public static HttpControllerContext CreateControllerContext(HttpConfiguration configuration = null, IHttpController instance = null, IHttpRouteData routeData = null, HttpRequestMessage request = null)
        {
            HttpConfiguration config = configuration ?? new HttpConfiguration();
            IHttpRouteData route = routeData ?? new HttpRouteData(new HttpRoute());
            HttpRequestMessage req = request ?? new HttpRequestMessage();
            req.SetConfiguration(config);
            req.SetRouteData(route);

            HttpControllerContext context = new HttpControllerContext(config, route, req);
            if (instance != null)
            {
                context.Controller = instance;
            }
            context.ControllerDescriptor = CreateControllerDescriptor(config);

            return context;
        }

        public static HttpActionContext CreateActionContext(HttpControllerContext controllerContext = null, HttpActionDescriptor actionDescriptor = null)
        {
            HttpControllerContext context = controllerContext ?? ContextUtil.CreateControllerContext();
            HttpActionDescriptor descriptor = actionDescriptor ?? CreateActionDescriptor();
            descriptor.ControllerDescriptor = context.ControllerDescriptor;
            return new HttpActionContext(context, descriptor);
        }

        public static HttpActionContext GetHttpActionContext(HttpRequestMessage request)
        {
            HttpActionContext actionContext = CreateActionContext();
            actionContext.ControllerContext.Request = request;
            return actionContext;
        }

        public static HttpActionExecutedContext GetActionExecutedContext(HttpRequestMessage request, HttpResponseMessage response)
        {
            HttpActionContext actionContext = CreateActionContext();
            actionContext.ControllerContext.Request = request;
            HttpActionExecutedContext actionExecutedContext = new HttpActionExecutedContext(actionContext, null) { Response = response };
            return actionExecutedContext;
        }

        public static HttpControllerDescriptor CreateControllerDescriptor(HttpConfiguration config = null)
        {
            if (config == null)
            {
                config = new HttpConfiguration();
            }
            return new HttpControllerDescriptor() { Configuration = config, ControllerName = "FooController" };
        }

        public static HttpActionDescriptor CreateActionDescriptor()
        {
            var mock = new Mock<HttpActionDescriptor>() { CallBase = true };
            mock.SetupGet(d => d.ActionName).Returns("Bar");
            return mock.Object;
        }
    }
}