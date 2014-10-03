using System.IO;
using System.Web.Http.Results;

namespace System.Web.Http {

	public static class ApiControllerExtHtml {

		private const string ViewDirectory = @"/views";

		/// <summary>
		/// Returns a result to the user made up of parsed cshtml, if the view name is not supplied then it will default to finding a view of the same name as the action, in a folder the
		/// same name as the controller and under the views folder.
		///
		/// If the name is supplied it should be an application root relative one. Do not include '.cshtml' as that is included automatically
		/// </summary>
		/// <returns></returns>
		public static HtmlActionResult Html(this ApiController controller) {

			var root = GetRoot(controller);
			var view = GetView(controller);

			return new HtmlActionResult(view, null, root);
		}

		/// <summary>
		/// Returns a result to the user made up of parsed cshtml, if the view name is not supplied then it will default to finding a view of the same name as the action, in a folder the
		/// same name as the controller and under the views folder.
		///
		/// If the name is supplied it should be an application root relative one. Do not include '.cshtml' as that is included automatically
		/// </summary>
		/// <returns></returns>
		public static HtmlActionResult Html(this ApiController controller, string view) {

			var root = GetRoot(controller);

			return new HtmlActionResult(view, null, root);
		}

		/// <summary>
		/// Returns a result to the user made up of parsed cshtml, if the view name is not supplied then it will default to finding a view of the same name as the action, in a folder the
		/// same name as the controller and under the views folder.
		///
		/// If the name is supplied it should be an application root relative one. Do not include '.cshtml' as that is included automatically
		/// </summary>
		/// <returns></returns>
		public static HtmlActionResult Html(this ApiController controller, dynamic obj) {

			var root = GetRoot(controller);
			var view = GetView(controller);

			return new HtmlActionResult(view, obj, root);
		}

		/// <summary>
		/// Returns a result to the user made up of parsed cshtml, if the view name is not supplied then it will default to finding a view of the same name as the action, in a folder the
		/// same name as the controller and under the views folder.
		///
		/// If the name is supplied it should be an application root relative one. Do not include '.cshtml' as that is included automatically
		/// </summary>
		/// <returns></returns>
		public static HtmlActionResult Html(this ApiController controller, string view, dynamic obj) {

			var root = GetRoot(controller);

			return new HtmlActionResult(view, obj, root);
		}

		private static string GetRoot(ApiController controller) {

			// Gets the root by getting the absoluteUri of the application and removing the root
			return controller.Url.Content("~/").Substring(controller.Request.RequestUri.GetLeftPart(UriPartial.Authority).Length);
		}

		private static string GetView(ApiController controller) {

			return Path.Combine(ViewDirectory, controller.ControllerContext.ControllerDescriptor.ControllerName, controller.ActionContext.ActionDescriptor.ActionName);
		}
	}
}