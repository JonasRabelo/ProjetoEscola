using ProjetoEscolaMJV.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjetoEscolaMJV.Models
{
    public class AlunoSemSenhaModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome do aluno")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o login do aluno")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Escolha a série do aluno")]
        public Series Serie { get; set; }

        [Required(ErrorMessage = "Digite o e-mail do aluno!")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido!")]
        public string Email { get; set; }
    }
}
