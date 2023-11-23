using System.ComponentModel.DataAnnotations;

namespace ProjetoEscolaMJV.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Digite o login")]
        public string Login { get; set; }
        [Required(ErrorMessage = "Digite a senha")]
        public string Senha { get; set; }
        public string Tipo { get; set; }
        public int? Id { get; set; }
    }
}

