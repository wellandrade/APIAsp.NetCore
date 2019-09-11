using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DevIO.Api.DTO
{
    public class EnderecoDTO
    {
        [Key]
        public Guid Id { get; set; }

        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigaório")]
        [StringLength(200, ErrorMessage = "O campo {0} deve conter entre {1} e {2} caracteres", MinimumLength = 3)]
        public string Logradouro { get; set; }


        [Required(ErrorMessage = "O campo {0} é obrigaório")]
        [StringLength(50, ErrorMessage = "O campo {0} deve conter entre {1} e {2} caracteres", MinimumLength = 3)]
        public string Numero { get; set; }

        public string Complemento { get; set; }

        public string Cep { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigaório")]
        [StringLength(100, ErrorMessage = "O campo {0} deve conter entre {1} e {2} caracteres", MinimumLength = 3)]
        public string Bairro { get; set; }

        public string Cidade { get; set; }

        public string Estado { get; set; }

        // EF Relacionamento
        public FornecedorDTO Fornecedor { get; set; }
    }
}
