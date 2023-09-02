﻿using ControleDeProdutosAula.Models;
using ControleDeProdutosAula.Repository;
using Microsoft.AspNetCore.Mvc;

namespace ControleDeProdutosAula.Controllers
{
	[Route("api/Produto")]
	[ApiController]
	public class ProdutoApiController : Controller
	{
		private readonly IProdutoRepositorio _produtoRepositorio;

		public ProdutoApiController(IProdutoRepositorio produtoRepositorio)
		{
			_produtoRepositorio = produtoRepositorio;
		}

		// GET: api/Produtos
		[HttpGet("Todos")]
		public async Task<ActionResult<List<ProdutoModel>>> ListarProdutos()
		{
			List<ProdutoModel> produtos = await _produtoRepositorio.BuscarTodos();

			return await Task.FromResult(produtos);
		}

		// GET: api/Produto/5
		[HttpGet("{id}")]
		public async Task<ActionResult<ProdutoModel>> ObterProdutoPorId(long id)
		{
			ProdutoModel produto = await _produtoRepositorio.ListarPorId(id);

			return await Task.FromResult(produto);
		}

		// POST: api/Produto
		[HttpPost]
		public async Task<ActionResult<ProdutoModel>> CadastrarProduto(ProdutoModel produto)
		{
			produto.DataDeRegistro = DateTime.Now;
			produto.Ativo = true;

			await _produtoRepositorio.Adicionar(produto);

			return await Task.FromResult(produto);
		}

		// PUT: api/Produto/{id}
		[HttpPut("{id}")]
		public async Task<ActionResult<ProdutoModel>> AlterarProduto(long id, ProdutoModel produto)
		{
			if (id > 0)
			{
				if (produto != null)
				{
					ProdutoModel produtoBase = await _produtoRepositorio.ListarPorId(id);

					if (produtoBase == null)
					{
						return await Task.FromResult(NotFound());
					}

					produtoBase.Descricao = produto.Descricao;
					produtoBase.CodigoDeBarras = produto.CodigoDeBarras;
					produtoBase.DataDeValidade = produto.DataDeValidade;
					produtoBase.Valor = produto.Valor;
					produtoBase.Quantidade = produto.Quantidade;
					produtoBase.NomeDaFoto = produto.NomeDaFoto;
					produtoBase.Foto = produto.Foto;
					produtoBase.Ativo = produto.Ativo;

					await _produtoRepositorio.Atualizar(produtoBase);

					return await Task.FromResult(NoContent());
				}
				return await Task.FromResult(BadRequest());
			}
			return await Task.FromResult(BadRequest());
		}

		// DELETE: api/Produto/5
		[HttpDelete("{id}")]
		public async Task<ActionResult<String>> ApagarProdutoPorId(long id)
		{
			ProdutoModel produtoBase = await _produtoRepositorio.ListarPorId(id);

			bool sucesso = await _produtoRepositorio.Apagar(id);

			if (sucesso)
				return await Task.FromResult($"Produto {produtoBase.Descricao} apagado com Sucesso!!!");
			else
				return await Task.FromResult($"Produto {produtoBase.Descricao} NÃO apagado - ERRO!!!");
		}
	}
}
