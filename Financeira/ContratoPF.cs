using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Financeira
{
    public class ContratoPessoaFísica : Contrato
    {
        public string CPF { get; set; }
        public DateOnly DataDeNascimento { get; set; }

        public ContratoPessoaFísica(string contratante, decimal valor, int prazo, string cpf, DateOnly dataNascimento)
        {
            IdContrato = Guid.NewGuid();
            Contratante = contratante;
            Valor = valor;
            Prazo = prazo;
            CPF = cpf;
            DataDeNascimento = dataNascimento;
        }
        public int CalcularIdade()
        {
            int age = 0;
            age = DateTime.Now.Year - DataDeNascimento.Year;
            if (DateTime.Now.DayOfYear < DataDeNascimento.DayOfYear)
                age--;
            return age;
        }
        public override decimal CalcularPrestação()
        {
            int idade = CalcularIdade();
            int adicional = 0;
            if (idade <= 30)
                adicional = 1;
            else if (idade <= 40)
                adicional = 2;
            else if (idade <= 50)
                adicional = 3;
            else if (idade > 50)
                adicional = 4;
            return (Valor / Prazo) + adicional;
        }
        public override void ExibirInfo()
        {
            string defMes = Prazo > 1 ? "meses" : "mês";
            long numeroCPF = Convert.ToInt64(CPF);
            Console.WriteLine(@$"======= Contrato: {IdContrato} =======
CPF do Contratante: {numeroCPF.ToString(@"000\.000\.000\-00")}
Nome do Contratante: {Contratante}
Data de Nascimento: {DataDeNascimento}
Valor: R$ {Valor.ToString("F")}
Prazo: {Prazo} {defMes}
Prestação: R$ {CalcularPrestação().ToString("F")}");
        }
    }
}
