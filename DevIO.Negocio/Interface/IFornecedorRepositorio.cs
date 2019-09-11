using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.Business.Interface
{
    public interface IFornecedorRepositorio
    {
        Task<Fornecedor> ObterPorId(Guid id);

        Task<IEnumerable<Fornecedor>> ListarTodos();

        Task Atualizar(Fornecedor fornecedor);

        Task Adicionar(Fornecedor fornecedor);

        bool Excluir(Guid id);

        Task<Fornecedor> ObterFornecedorEndereco(Guid id);

        Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id);

        bool Buscar(string documento);

    }
}
