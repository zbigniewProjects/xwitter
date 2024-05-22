using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BlogProject.MVC.Utils
{
    public static class ViewUtils
    {
        public static string CalculateHowMuchTimePassed(DateTime date)
        {
            TimeSpan elapsedTime = DateTime.UtcNow - date;

            string elapsedTimeString;
            if (elapsedTime.Days > 0)
                elapsedTimeString = elapsedTime.Days == 1 ? $"{elapsedTime.Days} day ago" : $"{elapsedTime.Days} days ago";
            else if (elapsedTime.Hours > 0)
                elapsedTimeString = elapsedTime.Hours == 1 ? $"{elapsedTime.Hours} hour ago" : $"{elapsedTime.Hours} hours ago";
            else if (elapsedTime.Minutes > 0)
                elapsedTimeString = elapsedTime.Minutes == 1 ? $"{elapsedTime.Minutes} minute ago" : $"{elapsedTime.Minutes} minutes ago";
            else
                elapsedTimeString = "Less than a minute ago";

            return elapsedTimeString;
        }
    }

    //Taken from https://www.surinderbhomra.com/Blog/2018/09/02/ASPNET-Core-Render-Partial-View-To-String-Outside-Controller-Context
    public static class ControllerExtensions
    {
        public static async Task<string> RenderViewToStringAsync<TModel>(this Controller controller, string viewNamePath, TModel model)
        {
            if (string.IsNullOrEmpty(viewNamePath))
                viewNamePath = controller.ControllerContext.ActionDescriptor.ActionName;

            controller.ViewData.Model = model;

            using (StringWriter writer = new StringWriter())
            {
                try
                {
                    IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

                    ViewEngineResult viewResult = null;

                    if (viewNamePath.EndsWith(".cshtml"))
                        viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
                    else
                        viewResult = viewEngine.FindView(controller.ControllerContext, viewNamePath, false);

                    if (!viewResult.Success)
                        return $"A view with the name '{viewNamePath}' could not be found";

                    ViewContext viewContext = new ViewContext(
                        controller.ControllerContext,
                        viewResult.View,
                        controller.ViewData,
                        controller.TempData,
                        writer,
                        new HtmlHelperOptions()
                    );

                    await viewResult.View.RenderAsync(viewContext);

                    return writer.GetStringBuilder().ToString();
                }
                catch (Exception exc)
                {
                    return $"Failed - {exc.Message}";
                }
            }
        }

        /// <summary>
        /// Render a partial view to string, without a model present.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="controller"></param>
        /// <param name="viewNamePath"></param>
        /// <returns></returns>
        public static async Task<string> RenderViewToStringAsync(this Controller controller, string viewNamePath)
        {
            if (string.IsNullOrEmpty(viewNamePath))
                viewNamePath = controller.ControllerContext.ActionDescriptor.ActionName;

            using (StringWriter writer = new StringWriter())
            {
                try
                {
                    IViewEngine viewEngine = controller.HttpContext.RequestServices.GetService(typeof(ICompositeViewEngine)) as ICompositeViewEngine;

                    ViewEngineResult viewResult = null;

                    if (viewNamePath.EndsWith(".cshtml"))
                        viewResult = viewEngine.GetView(viewNamePath, viewNamePath, false);
                    else
                        viewResult = viewEngine.FindView(controller.ControllerContext, viewNamePath, false);

                    if (!viewResult.Success)
                        return $"A view with the name '{viewNamePath}' could not be found";

                    ViewContext viewContext = new ViewContext(
                        controller.ControllerContext,
                        viewResult.View,
                        controller.ViewData,
                        controller.TempData,
                        writer,
                        new HtmlHelperOptions()
                    );

                    await viewResult.View.RenderAsync(viewContext);

                    return writer.GetStringBuilder().ToString();
                }
                catch (Exception exc)
                {
                    return $"Failed - {exc.Message}";
                }
            }
        }

    }
}
