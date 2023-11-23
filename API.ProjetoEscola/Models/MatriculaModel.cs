namespace Models
{
    public class MatriculaModel
    {
        public int Id { get; set; }
        public DateTime? DataMatricula { get; set; }
        public DateTime? DataDeAtualizacao { get; set; }
        public double? Nota1 { get; set; }
        public double? Nota2 { get; set; }
        public double? Nota3 { get; set; }
        public double? Nota4 { get; set; }
        public double? MediaFinal { get; set; }
        public int AlunoId { get; set; }
        public virtual AlunoModel? Aluno { get; set; }

        public int DisciplinaId { get; set; }
        public virtual DisciplinaModel? Disciplina { get; set; }
    }
}
