using ProjetoEscolaMJV.Models;

namespace ProjetoEscolaMJV.Helper
{
    public interface ISessaoUsuario<T>
    {
        void CriarSessaoDoUsuario(T aluno);
        void RemoverSessaoUsuario();
        T BuscarSessaoDoUsuario();
    }
}
