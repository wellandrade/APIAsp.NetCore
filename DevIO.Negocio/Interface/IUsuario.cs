using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace DevIO.Business.Interface
{
    public interface IUsuario
    {
        string Nome { get; set; }

        Guid ObterUsuarioPorId();

        string ObterUsuarioPorEmail();

        bool Autenticado();

        bool EstaNoRole(string role);

        IEnumerable<Claim> ObterClaimsIdentity();
    }
}
