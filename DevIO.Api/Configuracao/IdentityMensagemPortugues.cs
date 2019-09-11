using Microsoft.AspNetCore.Identity;

namespace DevIO.Api.Configuracao
{
    public class IdentityMensagemPortugues : IdentityErrorDescriber
    {
        // Tratar as mensagem do identity para portugues 
        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = nameof(DefaultError),
                Description = "Ocorreu um erro desconhecido"
            };
        }

        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = nameof(DefaultError),
                Description = "Senha incorreta"
            };
        }
    }
}