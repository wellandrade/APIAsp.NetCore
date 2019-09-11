using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.DTO
{
    public class FornecedorDTO
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigaório")]
        [StringLength(100, ErrorMessage = "O campo {0} deve conter entre {1} e {2} caracteres", MinimumLength = 3)]
        public string Nome { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "O campo {0} deve conter entre {1} e {2}, caracteres", MinimumLength = 3)]
        public string Documento { get; set; }

        public EnderecoDTO Endereco { get; set; }

        public bool Ativo { get; set; }

        public IEnumerable<ProdutoDTO> Produtos { get; set; }
    }
}
