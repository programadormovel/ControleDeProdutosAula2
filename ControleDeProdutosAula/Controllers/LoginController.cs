using ControleDeProdutosAula.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeProdutosAula.Controllers
{
	public class LoginController : Controller
	{
		private readonly ILoginRepositorio _loginRepositorio;

		public LoginController(ILoginRepositorio loginRepositorio)
		{
			_loginRepositorio = loginRepositorio;
		}

		async public Task<IActionResult> Index()
		{

			return await Task.FromResult(View());
		}

		[HttpPost]
        async public Task<IActionResult> Index(string email, string senha)
        {


            return await Task.FromResult(RedirectToAction("Index", "Home"));
        }

    }
}
