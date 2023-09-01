using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ControleDeProdutosAula.Models
{
	public class IndexModel : PageModel
	{
		public const string SessionKeyUser = "_Usuario";
		public const string SessionKeyEmail = "_Email";

		private readonly ILogger<IndexModel> _logger;

		public IndexModel(ILogger<IndexModel> logger)
		{
			_logger = logger;
		}

		public void OnGet()
		{
			if (string.IsNullOrEmpty(HttpContext.Session.GetString(SessionKeyUser)))
			{
				HttpContext.Session.SetString(SessionKeyUser, "Adriano");
				//HttpContext.Session.SetInt32(SessionKeyAge, 73);
			}
			var usuario = HttpContext.Session.GetString(SessionKeyUser);
			//var age = HttpContext.Session.GetInt32(SessionKeyAge).ToString();

			_logger.LogInformation("Session User: {usuario}", usuario);
			//_logger.LogInformation("Session Age: {Age}", age);
		}
	}
}
