using ProjetoEscolaMJV.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjetoEscolaMJV.Models
{
    public class ProfessorModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome do usuário")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o login do usuário")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Digite a senha do usuário")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "O e-mail informado não é válido!")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido!")]
        public string Email { get; set; }
        public Perfil? Perfil { get; set; }
        public DateTime DataDeCadastro { get; set; }
        public DateTime? DataDeAtualizacao { get; set; }

        public virtual List<DisciplinaModel>? Disciplinas { get; set; }

        public bool SenhaValida(string senha)
        {
            return Senha == senha;
        }
    }
}
