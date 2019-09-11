using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DevIO.Api.DTO
{
    public class RegistrarUsuarioDTO
    {
        [Required(ErrorMessage = "O campo {0} nao foi fornecido")]
        [EmailAddress(ErrorMessage = "O campo {0} esta no formato invalido")]
        public string Email { get; set; }


        [Required(ErrorMessage = "O campo {0} nao foi fornecido")]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre 3 e 50 caracteres")]
        public string Senha { get; set; }

        [Compare("Senha", ErrorMessage = "As senhas nao conferem")]
        public string ConfirmacaoSenha { get; set; }
    }

    public class LoginUsuarioDTO
    {
        [Required(ErrorMessage = "O campo {0} nao foi fornecido")]
        [EmailAddress(ErrorMessage = "O campo {0} esta no formato invalido")]
        public string Email { get; set; }


        [Required(ErrorMessage = "O campo {0} nao foi fornecido")]
        [StringLength(50, ErrorMessage = "O campo {0} precisa ter entre 3 e 50 caracteres")]
        public string Senha { get; set; }
    }

    public class UsuarioTokenDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<ClaimDTO> Claims { get; set; }
    }

    public class LoginResponseDTO
    {
        public string AcessToken { get; set; }
        public double ExpireIn { get; set; }
        public UsuarioTokenDTO UsuarioToken { get; set; }
    }

    public class ClaimDTO
    {
        public string Value { get; set; }
        public string Type { get; set; }
    }

}
