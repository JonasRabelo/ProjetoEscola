using ProjetoEscolaMJV.Models;
using System.ComponentModel.DataAnnotations;

namespace ProjetoEscolaMJV.Models
{
    public class ProfessorSemSenhaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome do usuário")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o login do usuário")]
        public string Login { get; set; }

        [Required(ErrorMessage = "O e-mail informado não é válido!")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido!")]
        public string Email { get; set; }
    }
}
