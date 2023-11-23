using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class SuperUserModel
    {
        public int Id { get; set; }
        public required string Login { get; set; }
        public required string Senha { get; set; }
    }
}
