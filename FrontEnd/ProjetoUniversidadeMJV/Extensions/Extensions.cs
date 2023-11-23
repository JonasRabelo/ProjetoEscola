namespace ProjetoEscolaMJV.Extensions
{
    /// <summary>
    /// Classe estática de extensão para configurar um HttpClient no contexto de serviços.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Configura um HttpClient com base no endpoint especificado e adiciona-o como um serviço.
        /// </summary>
        /// <param name="services">Coleção de serviços na qual o HttpClient será configurado.</param>
        public static void ConfigurarHttpClient(this IServiceCollection services)
        {
            string endpoint = @"https://localhost:7180/api/";
           
            services.AddHttpClient("APIProjetoEscola", c =>
            {
                c.BaseAddress = new Uri(endpoint);
                c.DefaultRequestHeaders.Add("Accept", "application/json");
            });
        }
    }
}
