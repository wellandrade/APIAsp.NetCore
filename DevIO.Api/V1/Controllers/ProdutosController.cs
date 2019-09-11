using AutoMapper;
using DevIO.Api.Controllers;
using DevIO.Api.DTO;
using DevIO.Business.Interface;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DevIO.Api.V1.Controllers
{
    [Route("api/produtos")]
    public class ProdutosController : MainController
    {
        private readonly IProdutoRepositorio _produtoRepositorio;
        private readonly IProdutoServico _produtoServico;
        private readonly IMapper _mapper;
        private readonly INotificador _notificador;

        public ProdutosController(IProdutoRepositorio produtoRepositorio, IProdutoServico produtoServico,
                                  INotificador notificador, IMapper mapper)
            : base(notificador)
        {
            _produtoRepositorio = produtoRepositorio;
            _produtoServico = produtoServico;
            _mapper = mapper;
            _notificador = notificador;
        }

        [HttpGet]
        public async Task<IEnumerable<ProdutoDTO>> ObterTodos()
        {
            return _mapper.Map<IEnumerable<ProdutoDTO>>(await _produtoRepositorio.ObterTodos());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<IEnumerable<ProdutoDTO>>> ObterPorId(Guid id)
        {
            var produto = await ObterProdutoPorId(id);

            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        [HttpPost] // Upload de base 64 
        public async Task<ActionResult<ProdutoDTO>> Adicionar(ProdutoDTO produto)
        {
            if (!ModelState.IsValid)
            {
                return CustomizarResponse(ModelState);
            }

            var imagemNome = Guid.NewGuid() + "_" + produto.Imagem;

            if (!UploadArquivo(produto.ImagemUpload, imagemNome))
            {
                return CustomizarResponse();
            }

            produto.Imagem = imagemNome;
            await _produtoServico.Adicionar(_mapper.Map<Produto>(produto));

            return Ok();
        }

        [HttpPost("Adicionar")] // Upload de imagem grande
        public async Task<ActionResult<ProdutoDTO>> AdicionarIFormFile(ProdutoImageDTO produto)
        {
            if (!ModelState.IsValid)
            {
                return CustomizarResponse(ModelState);
            }

            var imagemNome = Guid.NewGuid() + "_";

            if (!UploadArquivoPesado(produto.ImagemUpload, imagemNome))
            {
                return CustomizarResponse();
            }

            produto.Imagem = imagemNome;
            await _produtoServico.Adicionar(_mapper.Map<Produto>(produto));

            return Ok();
        }

        [RequestSizeLimit(400000000)] // Liberando até 40 mg
        [HttpPost("imagem")]
        public async Task<ActionResult> AdicionarImagemIFormFile(IFormFile file)
        {
            return Ok(file);
        }

        private bool UploadArquivo(string arquivo, string imgNome)
        {
            var imgByteArray = Convert.FromBase64String(arquivo);

            if (string.IsNullOrEmpty(arquivo))
            {
                ModelState.AddModelError(string.Empty, "Forneça uma imagem para o produto");
                return false;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", imgNome);

            if (System.IO.File.Exists(filePath))
            {
                ModelState.AddModelError(string.Empty, "Ja existe um arquivo com esse nome");
                return false;
            }

            // Aqui o arquivo sera escrito
            System.IO.File.WriteAllBytes(filePath, imgByteArray);

            return true;
        }

        private bool UploadArquivoPesado(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Forneça uma imagem para o produto");
                return false;
            }

            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/imagens", arquivo.FileName);

            if (System.IO.File.Exists(filePath))
            {
                ModelState.AddModelError(string.Empty, "Ja existe um arquivo com esse nome");
                return false;
            }

            // Aqui o arquivo sera escrito
            using (var strem = new FileStream(filePath, FileMode.Create))
            {
                arquivo.CopyTo(strem);
            }

            return true;
        }

        private async Task<ProdutoDTO> ObterProdutoPorId(Guid id)
        {
            return _mapper.Map<ProdutoDTO>(await _produtoRepositorio.ObterProdutoFornecedor(id));
        }

    }
}
