using DevIO.Business.Interface;
using DevIO.Business.Models;
using DevIO.Business.Models.Validacao;
using System;
using System.Threading.Tasks;

namespace DevIO.Business.Service
{
    public class FornecedorServico : BaseServico, IFornecedorServico
    {
        private readonly IFornecedorRepositorio _fornecedorRepositorio;
        private readonly IEnderecoRepositorio _enderecoRepositorio;

        public FornecedorServico(IFornecedorRepositorio fornecedorRepositorio, INotificador notificador, IEnderecoRepositorio enderecoRepositorio) : base(notificador)
        {
            _fornecedorRepositorio = fornecedorRepositorio;
            _enderecoRepositorio = enderecoRepositorio;
        }

        public async Task<bool> Adicionar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidacao(), fornecedor))
            {
                return false;
            }

            if (_fornecedorRepositorio.Buscar(fornecedor.Documento))
            {
                Notificar("Já existe um fornecedor para o documento informado");
            }

            await _fornecedorRepositorio.Adicionar(fornecedor);

            return true;
        }

        public async Task<bool> Atualizar(Fornecedor fornecedor)
        {
            if (!ExecutarValidacao(new FornecedorValidacao(), fornecedor))
            {
                return false;
            }

            await _fornecedorRepositorio.Atualizar(fornecedor);

            return true;
        }

        public async Task<bool> Remover(Guid id)
        {
            try
            {
                var endereco = await _fornecedorRepositorio.ObterFornecedorEndereco(id);

                if (endereco != null)
                {
                    _enderecoRepositorio.Remover(id);
                }

                _fornecedorRepositorio.Excluir(id);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
