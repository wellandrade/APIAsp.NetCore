using DevIO.Business.Interface;
using DevIO.Business.Models;
using DevIO.Data.Content;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace DevIO.Data.Repositorio
{
    public class FornecedorRepositorio : IFornecedorRepositorio
    {
        private readonly MeuDbContext _db;

        public FornecedorRepositorio(MeuDbContext db)
        {
            _db = db;
        }

        public async Task Atualizar(Fornecedor fornecedor)
        {
            _db.Entry<Fornecedor>(fornecedor).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public bool Excluir(Guid id)
        {
            var fornecedor = _db.Fornecedores.Find(id);

            if (fornecedor != null)
            {
                _db.Fornecedores.Remove(fornecedor);
                _db.SaveChanges();
                return true;
            }

            return false;
        }

        public async Task<IEnumerable<Fornecedor>> ListarTodos()
        {
            return await _db.Fornecedores.AsNoTracking().ToListAsync();
        }

        public async Task<Fornecedor> ObterFornecedorEndereco(Guid id)
        {
            return await _db.Fornecedores.AsNoTracking()
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Fornecedor> ObterFornecedorProdutosEndereco(Guid id)
        {
            return await _db.Fornecedores.AsNoTracking()
                //.Include(p => p.Produtos)
                .Include(c => c.Endereco)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Fornecedor> ObterPorId(Guid Id)
        {
            return await _db.Fornecedores.FindAsync(Id);
        }

        public bool Buscar(string documento)
        {
            return _db.Fornecedores.Where(x => x.Documento == documento).Any();
        }

        public async Task Adicionar(Fornecedor fornecedor)
        {
            _db.Fornecedores.Add(fornecedor);
            await _db.SaveChangesAsync();
        }
    }
}
