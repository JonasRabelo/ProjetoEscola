using Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class AlunoModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Digite o nome do aluno")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "Digite o login do aluno")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Digite a senha do aluno")]
        public string Senha { get; set; }

        [Required(ErrorMessage = "Escolha a série do aluno")]
        public Series Serie { get; set; }

        [Required(ErrorMessage = "Digite o e-mail do aluno!")]
        [EmailAddress(ErrorMessage = "O e-mail informado não é válido!")]
        public string Email { get; set; }

        public DateTime? DataDeCadastro { get; set; }
        public DateTime? DataDeAtualizacao { get; set; }

        public virtual List<MatriculaModel>? Matriculas { get; set; }

        public bool SenhaValida(string senha)
        {
            return Senha == senha;
        }
    }
}
