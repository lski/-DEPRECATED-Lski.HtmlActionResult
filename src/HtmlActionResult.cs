using Lski.HtmlActionResult;
using RazorEngine.Templating;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace System.Web.Http.Results {

	/// <summary>
	/// Returns Html as an async IHttpActionResult. Either accepts an html file or cshtml file, using RazorEngine to cache
	///
	/// If you are debugging, you might want to turn caching off. This can be done globally in code:
	///
	/// HtmlActionResult.IgnoreCache = true;
	///
	/// Or via web.config:
	///
	/// lski.htmlactionresult.ignorecache : true
	/// </summary>
	public class HtmlActionResult : IHttpActionResult {

		private readonly string _template;
		private readonly dynamic _model;
		private readonly string _root;

		public static bool IgnoreCache {
			get { return RazorEngineWrapper.IgnoreCache; }
			set { RazorEngineWrapper.IgnoreCache = value; }
		}

		public HtmlActionResult(string name, dynamic model, string root = null) {

			_template = name;
			_model = model;
			_root = root ?? "/";
		}

		public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken) {

			var dvb = new DynamicViewBag();
			dvb.AddValue("_ApplicationRoot", _root);

			var parsedContent = RazorEngineWrapper.Parse(_template, _model, dvb);

			var response = new HttpResponseMessage(HttpStatusCode.OK) {
				Content = new StringContent(parsedContent)
			};

			response.Content.Headers.ContentType = new MediaTypeHeaderValue("text/html");

			return Task.FromResult(response);
		}
	}
}