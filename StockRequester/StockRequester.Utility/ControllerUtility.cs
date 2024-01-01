using Microsoft.AspNetCore.Mvc;

namespace StockRequester.Utility
{
    public static class ControllerUtility
    {
        public static string ControllerName(Type controllerType)
        {
            Type baseType = typeof(Controller);
            if(baseType.IsAssignableFrom(controllerType))
            {
                int lastControllerIndex = controllerType.Name.LastIndexOf("Controller");
                if(lastControllerIndex > 0)
                {
                    return controllerType.Name.Substring(0, lastControllerIndex);
                }
            }

            return controllerType.Name;
        }

    }
}
