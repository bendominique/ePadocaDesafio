using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebAPiEpadoca.Models
{
    public class Cliente
    {
        public int ID { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Telefone { get; set; }

        public string Senha { get; set; }

        public string Tipo { get; set; }

    }
}