namespace ProjetoEscolaMJV.Extensions
{
    public static class Extensions
    {
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
