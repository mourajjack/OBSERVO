using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OBSERVO.Models
{
    public class Colaboradores
    {
        [PrimaryKey]
        public int Id { get; set; }
        public string Matricula { get; set; }
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public string Senha { get; set; }
        public string DataNascimento { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public string Funcao { get; set; }
        public string Escala { get; set; }
        public string Posto { get; set; }
        public string DataAdmissao { get; set; }
        public string Empresa { get; set; }
        public string Cnpj { get; set; }
        public string AbaSheets { get; set; }
    }
}
