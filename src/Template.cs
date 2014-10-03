using Newtonsoft.Json;
using RazorEngine.Templating;

namespace Lski.HtmlActionResult {

	public class Template<T> : TemplateBase<T> {

		public string ApplicationRoot {
			get { return ViewBag._ApplicationRoot; }
		}

		public string Json(object obj) {
			return JsonConvert.SerializeObject(obj);
		}
	}
}