using DevIO.Business.Interface;
using DevIO.Business.Models;
using DevIO.Data.Content;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace DevIO.Data.Repositorio
{
    public class ProdutoRepositorio : IProdutoRepositorio
    {
        private readonly MeuDbContext _db;

        public ProdutoRepositorio(MeuDbContext db)
        {
            _db = db;
        }

        public async Task<Produto> ObterProdutoFornecedor(Guid id)
        {
            return await _db.Produtos.AsNoTracking().Include(f => f.Fornecedor).FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Produto>> ObterTodos()
        {
            return await _db.Produtos.Include(x => x.Fornecedor).OrderBy(x => x.Nome).ToListAsync();
        }

        public async Task<IEnumerable<Produto>> ObterProdutosPorFornecedor(Guid fornecedorId)
        {
            return await _db.Produtos.AsNoTracking().Where(x => x.FornecedorId == fornecedorId).ToListAsync();
        }

        public async Task Adicionar(Produto produto)
        {
            _db.Produtos.Add(produto);
            await _db.SaveChangesAsync();
        }
    }
}
