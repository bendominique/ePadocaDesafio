using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPiEpadoca.Models
{
    public class Padaria
    {
        public int ID { get; set; }
        public string Nome_Padaria { get; set; }
        public string Endereco { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}