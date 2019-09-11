using System;
using System.ComponentModel.DataAnnotations;

namespace DevIO.Api.DTO
{
    public class ProdutoDTO
    {
        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        public Guid FornecedorId { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigaório")]
        [StringLength(200, ErrorMessage = "O campo {0} deve conter entre {1} e {2} caracteres", MinimumLength = 3)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigaório")]
        [StringLength(1000, ErrorMessage = "O campo {0} deve conter entre {1} e {2} caracteres", MinimumLength = 3)]
        public string Descricao { get; set; }

        public string ImagemUpload { get; set; }

        public string Imagem { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigaório")]
        public decimal Valor { get; set; }

        [ScaffoldColumn(false)]
        public DateTime DataCadastro { get; set; }

        public bool Ativo { get; set; }

        [ScaffoldColumn(false)]
        public string Fornecedor { get; set; }
    }
}
