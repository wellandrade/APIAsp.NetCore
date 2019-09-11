using AutoMapper;
using DevIO.Api.DTO;
using DevIO.Business.Models;

namespace DevIO.Api.Configuracao
{
    public class AutoMapperConfiguracao : Profile
    {
        public AutoMapperConfiguracao()
        {
            CreateMap<Fornecedor, FornecedorDTO>().ReverseMap();
            CreateMap<Endereco, EnderecoDTO>().ReverseMap();
            CreateMap<Produto, ProdutoDTO>().ReverseMap();
            CreateMap<Produto, ProdutoImageDTO>().ReverseMap();
        }
    }
}
