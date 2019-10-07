﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using System;
using System.IO;

namespace CarRental.Web.ViewRender
{
    public class ViewRender : IViewRender
    {
        private IRazorViewEngine _viewEngine;
        private ITempDataProvider _tempDataProvider;
        private IServiceProvider _serviceProvider;
        public ViewRender(IRazorViewEngine razorViewEngine, ITempDataProvider tempDataProvider, IServiceProvider serviceProvider)
        {
            _viewEngine = razorViewEngine;
            _tempDataProvider = tempDataProvider;
            _serviceProvider = serviceProvider;
        }

        public string Render<TModel>(string name, TModel model)
        {
            var actionContext = GetActionContext();
            var viewEngineResult = _viewEngine.FindView(actionContext, name, false);
            if (!viewEngineResult.Success)
            {
                throw new InvalidOperationException(string.Format("Couldn't find view '{0}'", name));
            }
            var view = viewEngineResult.View;
            using (var output = new StringWriter())
            {
                var viewContext = new ViewContext(
                actionContext,
                view,
                new ViewDataDictionary<TModel>(
                metadataProvider: new EmptyModelMetadataProvider(),
                modelState: new ModelStateDictionary())
                {
                    Model = model
                },
                new TempDataDictionary(
                actionContext.HttpContext,
               _tempDataProvider),
                output,
                new HtmlHelperOptions());
                view.RenderAsync(viewContext).GetAwaiter().GetResult();
                return output.ToString();
            }
        }

        private ActionContext GetActionContext()
        {
            var httpContext = new DefaultHttpContext();
            httpContext.RequestServices = _serviceProvider;
            return new ActionContext(httpContext, new RouteData(), new ActionDescriptor());
        }
    }
}
