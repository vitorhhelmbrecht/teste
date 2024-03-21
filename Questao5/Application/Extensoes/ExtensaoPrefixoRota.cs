using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Routing;

namespace Questao5.Application.Extensoes
{
    public static class ExtensaoPrefixoRota
    {
        public static void UseRoutePrefix(this MvcOptions options, string prefix) {
            var routeAttribute = new RouteAttribute(prefix);
            options.Conventions.Add(new RoutePrefix(routeAttribute));
        }
    }

    public class RoutePrefix : IApplicationModelConvention
    {
        private readonly AttributeRouteModel _routePrefix;

        public RoutePrefix(IRouteTemplateProvider route) {
            _routePrefix = new AttributeRouteModel(route);
        }

        public void Apply(ApplicationModel application) {
            foreach (var selector in application.Controllers.SelectMany(c => c.Selectors)) {
                if (selector.AttributeRouteModel != null) {
                    selector.AttributeRouteModel = AttributeRouteModel.CombineAttributeRouteModel(_routePrefix, selector.AttributeRouteModel);
                } else {
                    selector.AttributeRouteModel = _routePrefix;
                }
            }
        }
    }
}
