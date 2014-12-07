﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Routing;

namespace Xania.AspNet.Simulator
{
    public class Router
    {
        private readonly Dictionary<string, ControllerBase> _controllerMap;
        public RouteCollection Routes { get; private set; }

        public Router()
        {
            _controllerMap = new Dictionary<String, ControllerBase>();
            Routes = new RouteCollection(new ActionRouterPathProvider());
        }

        public void RegisterController(string name, ControllerBase controller)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            _controllerMap.Add(name.ToLower(CultureInfo.InvariantCulture), controller);
        }

        protected internal virtual ControllerBase CreateController(string controllerName)
        {
            ControllerBase controller;
            if (_controllerMap.TryGetValue(controllerName.ToLower(CultureInfo.InvariantCulture), out controller))
                return controller;

            throw new KeyNotFoundException(controllerName);
        }

        public IAction Action(string url, string method = "GET")
        {
            var context = AspNetUtility.GetContext(url, method, null);
            var routeData = Routes.GetRouteData(context);

            if (routeData == null)
                return null;

            var controllerName = routeData.GetRequiredString("controller");
            var controller = CreateController(controllerName);
            var controllerDescriptor = new ReflectedControllerDescriptor(controller.GetType());
            var actionDescriptor = controllerDescriptor.FindAction(new ControllerContext(context, routeData, controller), routeData.GetRequiredString("action"));

            if (actionDescriptor == null)
                return null;

            return new ControllerAction(controller, actionDescriptor, method);
        }

        public Router RegisterDefaultRoutes()
        {
            Routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

            return this;
        }
    }
}