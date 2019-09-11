using DevIO.Business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevIO.Business.Interface
{
    public interface IProdutoRepositorio
    {
        Task<IEnumerable<Produto>> ObterTodos();

        Task<Produto> ObterProdutoFornecedor(Guid id);

        Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId);

        Task Adicionar(Produto produto);
    }
}
