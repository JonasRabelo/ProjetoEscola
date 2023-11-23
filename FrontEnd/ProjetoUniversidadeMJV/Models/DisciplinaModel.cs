using ProjetoEscolaMJV.Enums;
using System.ComponentModel.DataAnnotations;

namespace ProjetoEscolaMJV.Models
{
    public class DisciplinaModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Digite o nome da disciplina")]
        public string Nome { get; set; }
        [Required(ErrorMessage = "Escolha uma série para a disciplina.")]
        public Series Serie { get; set; }

        public int ProfessorId { get; set; }
        [Required(ErrorMessage = "Escolha um status para a disciplina.")]
        public bool Status { get; set; }
        public DateTime? DataDeCadastro { get; set; }
        public DateTime? DataDeAtualizacao { get; set; }
        public virtual ProfessorModel? Professor { get; set; }

        public virtual List<MatriculaModel>? Matriculas { get; set; }
    }
}
