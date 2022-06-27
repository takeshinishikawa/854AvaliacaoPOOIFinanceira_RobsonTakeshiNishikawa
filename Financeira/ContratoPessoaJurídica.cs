using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financeira
{
    public class ContratoPessoaJurídica : Contrato
    {
        public string CNPJ { get; set; }
        public string inscricaoEstadual { get; set; }
        public ContratoPessoaJurídica(string contratante, decimal valor, int prazo, string cnpj, string iEstadual)
        {
            IdContrato = Guid.NewGuid();
            Contratante = contratante;
            Valor = valor;
            Prazo = prazo;
            CNPJ = cnpj;
            inscricaoEstadual = iEstadual;
        }
        public override decimal CalcularPrestação()
        {
            return (Valor / Prazo) + 3;
        }
        public override void ExibirInfo()
        {
            string defMes = Prazo > 1 ? "meses" : "mês";
            long numeroCNPJ = Convert.ToInt64(CNPJ);
            long numeroIE = Convert.ToInt64(inscricaoEstadual);
            Console.WriteLine(@$"======= Contrato: {IdContrato} =======
CNPJ do Contratante: {numeroCNPJ.ToString(@"00\.000\.000\/0000\-00")}
Inscrição Estadual: {numeroIE.ToString(@"000\.000\.000")}
Nome do Contratante: {Contratante}
Valor: R$ {Valor.ToString("F")}
Prazo: {Prazo} {defMes}
Prestação: R$ {CalcularPrestação().ToString("F")}");
        }
    }
}
