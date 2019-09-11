using AutoMapper;
using DevIO.Api.Controllers;
using DevIO.Api.DTO;
using DevIO.Api.Extension;
using DevIO.Business.Interface;
using DevIO.Business.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.Api.V1.Controllers
{
    [Authorize] // Só vai poder fazer requisicao quem for autorizado  
    [ApiVersion("1.0")] // v1 da API
    [Route("api/v{version:apiVersion}/fornecedor")] // Utilizando versionamento da API 
    public class FornecedoresController : MainController
    {
        private readonly IFornecedorRepositorio _fornecedorRepositorio;
        private readonly IFornecedorServico _fornecedorServico;
        private readonly IMapper _mapper;
        private readonly INotificador _notificador;

        public FornecedoresController(IFornecedorRepositorio fornecedorRepositorio,
                                      IMapper mapper,
                                      IFornecedorServico fornecedorServico,
                                      INotificador notificador
                                      ) : base(notificador)
        {
            _fornecedorRepositorio = fornecedorRepositorio;
            _fornecedorServico = fornecedorServico;
            _mapper = mapper;
        }

        [AllowAnonymous] // Nao precisa de autorizacao pelo JTW 
        [HttpGet]
        public async Task<ActionResult<IEnumerable<FornecedorDTO>>> ObterTodos()
        {
            var fornecedor = _mapper.Map<IEnumerable<FornecedorDTO>>(await _fornecedorRepositorio.ListarTodos());

            return Ok(fornecedor);
        }

        [HttpGet("{id:guid}")] // Só vai chamar o método, se o Id for do tipo Guid
        public async Task<ActionResult<FornecedorDTO>> ObterPorId(Guid id)
        {
            var fornecedor = await ObterFornecedorProdutoEndereco(id);

            if (fornecedor == null)
            {
                return NotFound();
            }

            return Ok(fornecedor);
        }

        [ClaimsAuthorize("Fornecedor","Adicionar")]
        [HttpPost]
        public async Task<ActionResult<FornecedorDTO>> Adicionar(FornecedorDTO fornecedorDTO)
        {
            if (!ModelState.IsValid)
            {
                return CustomizarResponse(ModelState);
            }

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorDTO);
            var result = await _fornecedorServico.Adicionar(fornecedor); // Toda validacao no servico 

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [ClaimsAuthorize("Fornecedor","Atualizar")]
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<FornecedorDTO>> Atualizar(Guid id, FornecedorDTO fornecedorDTO)
        {
            if (id != fornecedorDTO.Id)
            {
                BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var fornecedor = _mapper.Map<Fornecedor>(fornecedorDTO);
            var result = await _fornecedorServico.Atualizar(fornecedor); // Toda validacao no servico 

            if (!result)
            {
                return BadRequest();
            }

            return Ok();
        }

        [ClaimsAuthorize("Fornecedor", "Excluir")]
        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<bool>> Excluir(Guid id)
        {
            var fornecedor = await ObterFornecedor(id);

            if (fornecedor == null)
            {
                return NotFound();
            }

            var result = await _fornecedorServico.Remover(id);

            if (!result)
            {
                return BadRequest();
            }

            return Ok(fornecedor);
        }

        public async Task<FornecedorDTO> ObterFornecedorProdutoEndereco(Guid id)
        {
            return _mapper.Map<FornecedorDTO>(await _fornecedorRepositorio.ObterFornecedorProdutosEndereco(id));
        }

        public async Task<FornecedorDTO> ObterFornecedor(Guid id)
        {
            return _mapper.Map<FornecedorDTO>(await _fornecedorRepositorio.ObterPorId(id));
        }
    }
}