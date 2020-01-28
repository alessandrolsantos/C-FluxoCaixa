using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LancTrans.Classes
{
    public class Lancamentos
    {
        public int codigo { get; set; }
        public DateTime data_hora { get; set; }       
        public string  descricao { get; set; }
        public decimal valor { get; set; }
        public string tipo { get; set; }
        public int codigo_conta { get; set; }
        public decimal? debito { get; set; }
        public decimal? credito { get; set; }
        public decimal saldo { get; set; }
    }
}
