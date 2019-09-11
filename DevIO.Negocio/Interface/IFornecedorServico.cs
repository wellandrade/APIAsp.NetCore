using DevIO.Business.Models;
using System;
using System.Threading.Tasks;

namespace DevIO.Business.Interface
{
    public interface IFornecedorServico
    {
        Task<bool> Adicionar(Fornecedor fornecedor);

        Task<bool> Atualizar(Fornecedor fornecedor);

        Task<bool> Remover(Guid id);
    }
}
