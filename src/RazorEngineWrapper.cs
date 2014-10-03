using RazorEngine.Configuration;
using RazorEngine.Templating;
using System;
using System.Configuration;
using System.IO;
using System.Text.RegularExpressions;

namespace Lski.HtmlActionResult {

	/// <summary>
	/// Created as an easy to use wrapper for the razor engine, the caching is based on this article: http://stackoverflow.com/a/21330077/653458 I have cleaned it up slightly though
	/// </summary>
	internal static class RazorEngineWrapper {

		private const string _ignoreCacheSettingName = "razorenginewrapper.ignorecache";

		static RazorEngineWrapper() {

			var templateConfig = new TemplateServiceConfiguration {
				BaseTemplateType = typeof(Template<>),
				EncodedStringFactory = new RazorEngine.Text.RawStringFactory()
			};

			RazorEngine.Razor.SetTemplateService(new TemplateService(templateConfig));

			var setting = ConfigurationManager.AppSettings[_ignoreCacheSettingName];

			if ("true".Equals(setting, StringComparison.OrdinalIgnoreCase)) {
				IgnoreCache = true;
			}
		}

		/// <summary>
		/// If set to true it will reload the templates every time from disk and not cache the template. By default its false and increases performance by caching the template
		/// </summary>
		public static bool IgnoreCache { get; set; }

		public static string Parse(string name, object model, DynamicViewBag bag = null) {

			if (IgnoreCache) {

				var content = LoadContent(name);

				return (bag == null)
					? RazorEngine.Razor.Parse(content, model)
					: RazorEngine.Razor.Parse(content, model, bag, null);
			}
			
			if (RazorEngine.Razor.Resolve(name) == null) {
				RazorEngine.Razor.Compile(LoadContent(name), name);
			}

			return (bag == null) ? RazorEngine.Razor.Run(name, model) : RazorEngine.Razor.Run(name, model, bag);
		}

		private static string LoadContent(string name) {

			if (String.IsNullOrEmpty(name)) {
				throw new ArgumentException("name");
			}

			// If the template name starts with a tilde or forward slash, the template is app relative, so strip it and add the base directory
			var filename = name.StartsWith("~") || name.StartsWith("/")
								? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, Regex.Replace(name, @"^(\~\/|\/)", ""))
								: name;

			var pathsToTest = new[] {
				filename + ".cshtml",
				filename + ".vbhtml",
				filename + ".html",
				filename + ".htm"
			};

			foreach (var path in pathsToTest) {

				if (File.Exists(path)) {
					return File.ReadAllText(path);
				}
			}

			throw new FileNotFoundException(filename);
		}
	}
}