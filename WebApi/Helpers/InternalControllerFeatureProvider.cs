using System.Reflection;
using EmptyService.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace EmptyService.WebApi.Helpers
{
    internal class InternalControllerFeatureProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            var isCustomController = !typeInfo.IsAbstract && typeof(BaseController).IsAssignableFrom(typeInfo);
            return isCustomController || base.IsController(typeInfo);
        }
    }
}
