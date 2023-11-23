using System;

namespace ProjetoEscolaMJV.Models
{
    public class SuperUserModel
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        public required string Senha { get; set; }
    }
}
