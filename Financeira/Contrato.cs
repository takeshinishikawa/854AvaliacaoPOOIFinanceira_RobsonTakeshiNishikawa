using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financeira
{
    public abstract class Contrato
    {
        public Guid IdContrato { get; set; }
        public string Contratante { get; set; }
        public decimal Valor { get; set; }
        public int Prazo { get; set; }

        public abstract decimal CalcularPrestação();
        public abstract void ExibirInfo();
    }
}
