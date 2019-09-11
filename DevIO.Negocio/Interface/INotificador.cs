using DevIO.Business.Notificacoes;
using System.Collections.Generic;

namespace DevIO.Business.Interface
{
    public interface INotificador
    {
        void Handler(Notificacao notificacao);

        List<Notificacao> ObterNotificacoes();

        bool TemNotificacao();
    }
}
