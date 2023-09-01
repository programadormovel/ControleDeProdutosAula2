using ControleDeProdutosAula.Models;
using ControleDeProdutosAula.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;
using Microsoft.IdentityModel.Tokens;

namespace ControleDeProdutosAula.Controllers
{
    public class ProdutoController : Controller
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private IHostingEnvironment Environment;
        public const string SessionKeyUser = "_Usuario";
        public const string SessionKeyEmail = "_Email";
        public const string SessionKeyNivel = "_Nivel";

        private readonly ILogger<IndexModel> _logger;

        public ProdutoController(IProdutoRepositorio produtoRepositorio, IHostingEnvironment _environment)
        {
            _produtoRepositorio = produtoRepositorio;
            Environment = _environment;
        }


        public async Task<IActionResult> Index()
        {
            List<ProdutoModel> produtos = await _produtoRepositorio.BuscarTodos();

            var usuario = HttpContext.Session.GetString(SessionKeyUser);
            if (!usuario.IsNullOrEmpty())
            {
                return await Task.FromResult(View(produtos));
            }
            return await Task.FromResult(RedirectToAction("Index", "Home"));

        }

        [Route("api/Produtos")]
        [HttpGet]
        public async Task<ActionResult<List<ProdutoModel>>> Produtos()
        {
            List<ProdutoModel> produtos = await _produtoRepositorio.BuscarTodos();

            return await Task.FromResult(produtos);
        }

        public async Task<IActionResult> Criar()
        {
            return await Task.FromResult(View());
        }

        public async Task<IActionResult> Editar(long id)
        {
            ProdutoModel produto = await _produtoRepositorio.ListarPorId(id);

            return await Task.FromResult(View(produto));
        }

        public async Task<IActionResult> ApagarConfirmacao(long id)
        {
            ProdutoModel produto = await _produtoRepositorio.ListarPorId(id);

            return await Task.FromResult(View(produto));
        }

        public async Task<IActionResult> Apagar(long id)
        {
            await _produtoRepositorio.Apagar(id);

            return await Task.FromResult(RedirectToAction("Index"));
        }

        [HttpPost]
        public async Task<IActionResult> Criar(ProdutoModel produto,
            IFormFile? imagemCarregada)
        {
            ProdutoModel model = produto;

            List<ValidationResult> results = new List<ValidationResult>();
            ValidationContext context = new ValidationContext(model, null, null);

            bool isValid = Validator.TryValidateObject(model, context, results, true);

            if (!isValid)
            {
                foreach (var validationResult in results)
                {
                    return View(model);
                }
            }

            model.DataDeRegistro = DateTime.Now;
            model.Ativo = true;

            // Carregamento de imagem
            string wwwPath = this.Environment.WebRootPath;
            string path = Path.Combine(wwwPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string fileName = Path.GetFileName(imagemCarregada!.FileName);

            var caminhoCompleto = Path.Combine(path, fileName);

            using (FileStream stream = new FileStream(caminhoCompleto, FileMode.Create))
            {
                imagemCarregada.CopyTo(stream);
                model.NomeDaFoto = caminhoCompleto;
            }

            model.Foto = Util.ReadFully2(caminhoCompleto);

            await _produtoRepositorio.Adicionar(model);

            return await Task.FromResult(RedirectToAction("Index"));
        }

        [HttpPost]
        public async Task<IActionResult> Alterar(ProdutoModel produto)
        {
            await _produtoRepositorio.Atualizar(produto);
            return await Task.FromResult(RedirectToAction("Index"));
        }
    }
}
