﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;
using System.Web.Security;
using System.Web.SessionState;
using Umbraco.Belle.System.PropertyEditors;
using Umbraco.Belle.System.Trees;
using Umbraco.Core;
using Umbraco.Web;
using System.Web.Mvc;

namespace Umbraco.Belle
{
    public class Global : UmbracoApplication
    {
        public Global()
        {
            //we don't want to put the Umbraco module in, but we need to ensure an UmbracoContext on each request, 
            // so we'll chuck it in here for now.
            this.BeginRequest += (sender, args) =>
                {
                    var httpContext = ((HttpApplication)sender).Context;
                    UmbracoContext.EnsureContext(new HttpContextWrapper(httpContext), ApplicationContext.Current);
                };
        }

        protected override void OnApplicationStarting(object sender, EventArgs e)
        {
            CreateRoutes();
            
            base.OnApplicationStarting(sender, e);

            //setup custom resolvers... this is only temporary until we move into the umbraco core
            PropertyEditorResolver.Current = new PropertyEditorResolver(() => PluginManager.Current.ResolvePropertyEditors());
            //setup the validators resolver with our predefined validators
            ValidatorsResolver.Current = new ValidatorsResolver(new[]
                {
                    new Lazy<Type>(() => typeof (RequiredValueValidator)),
                    new Lazy<Type>(() => typeof (RegexValueValidator)),
                    new Lazy<Type>(() => typeof (ValueTypeValueValidator)),
                    new Lazy<Type>(() => typeof (DelimitedValueValidator))
                });
        }

        private void CreateRoutes()
        {
            //For testing for now we'll route to /Belle/Main
            RouteTable.Routes.MapRoute(
                "umbraco-main",
                "Belle/Main/{action}/{id}",
                new { controller = "Main", action = "Index", id = UrlParameter.Optional });

            //For testing for now we'll route to /Belle/Main
            RouteTable.Routes.MapRoute(
                "umbraco-scripts",
                "Belle/SystemScripts/{action}/{id}",
                new { controller = "SystemScripts", action = "Index", id = UrlParameter.Optional });

        }
        
    }
}