namespace DevIO.Api.Extension
{
    public class AppSettings
    {
        public string Secret { get; set; } // Chave de criptografia

        public int ExpiracaoHoras { get;set;}

        public string Emissor { get; set; } // Quem emite

        public string ValidoEm { get; set; } // Em quais url o token e valido
    }
}
