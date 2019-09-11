using System.Threading.Tasks;
using DevIO.Business.Interface;
using DevIO.Business.Models;

namespace DevIO.Business.Service
{
    public class ProdutoServico : BaseServico, IProdutoServico
    {
        private readonly IProdutoRepositorio _produtoRepositorio;

        public ProdutoServico(INotificador notificador, IProdutoRepositorio produtoRepositorio) 
            : base(notificador)
        {
            _produtoRepositorio = produtoRepositorio;
        }

        public async Task<bool> Adicionar(Produto produto)
        {
            await _produtoRepositorio.Adicionar(produto);
            return true;
        }
    }
}
