using DevIO.Business.Models;
using System.Threading.Tasks;

namespace DevIO.Business.Interface
{
    public interface IProdutoServico
    {
        Task<bool> Adicionar(Produto produto);
    }
}
