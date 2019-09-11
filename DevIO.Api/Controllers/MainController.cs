using DevIO.Business.Interface;
using DevIO.Business.Notificacoes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;

namespace DevIO.Api.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    public abstract class MainController : ControllerBase
    {
        private readonly INotificador _notificador;

        public MainController(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected bool OperacaoValida()
        {
            return !_notificador.TemNotificacao();
        }

        protected ActionResult CustomizarResponse(object result = null)
        {
            if (OperacaoValida())
            {
                return Ok(new
                {
                    Sucesso = true,
                    Data = result
                });
            }

            return BadRequest(new
            {
                Sucesso = false,
                Erros = _notificador.ObterNotificacoes().Select(e => e.Mensagem)
            });

        }

        protected ActionResult CustomizarResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                NotificarErroModelInvalida(modelState);
            }

            return CustomizarResponse();
        }

        protected void NotificarErroModelInvalida(ModelStateDictionary modelState)
        {
            var erros = modelState.Values.SelectMany(e => e.Errors);

            string mensagemErro;

            foreach (var erro in erros)
            {
                mensagemErro = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message.ToString();
                NotificarErro(mensagemErro);
            }
        }

        protected void NotificarErro(string mensagem)
        {
            _notificador.Handler(new Notificacao(mensagem));
        }

        // validacao de notificacao de erro

        // validacao de modelstate

        // validacao da operacao de negocios
    }
}