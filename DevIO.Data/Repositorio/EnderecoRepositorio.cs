using System;
using DevIO.Business.Interface;
using DevIO.Data.Content;

namespace DevIO.Data.Repositorio
{
    public class EnderecoRepositorio : IEnderecoRepositorio
    {
        private readonly MeuDbContext _db;

        public EnderecoRepositorio(MeuDbContext db)
        {
            _db = db;
        }

        public void Remover(Guid id)
        {
            var endereco = _db.Enderecos.Find(id);

            if (endereco != null)
            {
                _db.Enderecos.Remove(endereco);
                _db.SaveChanges();
            }
        }

    }
}
