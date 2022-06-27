using System;
using System.Text.RegularExpressions;

namespace Financeira
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<ContratoPessoaFísica> contratosPF = new();
            List<ContratoPessoaJurídica> contratosPJ = new();
            int input = 0;
            bool sair = false;
        programa:
            while (!sair)
            {
            menuInicial:
                input = Interacoes.menuInicial();
                if (input == -5)
                {
                    if (Interacoes.telaConfirmação() == 1)
                        sair = true;
                    goto programa;
                }
                else if ((input == -1 || input == -2) && Validacoes.verificarOpção(input) < 0)
                    goto menuInicial;
                else if (input == 1)
                {
                cadastrarContrato:
                    int status = 0;
                    status = Interacoes.cadastrarContrato(ref contratosPF, ref contratosPJ);
                    if (status == -5 || status == -3)
                    {
                        if (Interacoes.telaConfirmação() == 1)
                        {
                            if (status == -5)
                                sair = true;
                            goto programa;
                        }
                        else
                            goto cadastrarContrato;
                    }
                    else if (status == -2)
                        goto cadastrarContrato;
                    else if (status == -4)
                        goto menuInicial;
                    else
                    {
                        Console.WriteLine("Por favor, informe um valor válido.");
                        goto cadastrarContrato;
                    }
                }
                else if (input == 2 && (contratosPF.Count() > 0 || contratosPJ.Count() > 0))
                {
                consultarContrato:
                    int status = 0;
                    status = Interacoes.consultarContrato(ref contratosPF, ref contratosPJ);
                    if (status == -5 || status == -3)
                    {
                        if (Interacoes.telaConfirmação() == 1)
                        {
                            if (status == -5)
                                sair = true;
                            goto programa;
                        }
                        else
                            goto consultarContrato;
                    }
                    else if (status == -2)
                        goto consultarContrato;
                    else if (status == -4)
                        goto menuInicial;
                    else
                    {
                        Console.WriteLine("Por favor, informe um valor válido.");
                        goto consultarContrato;
                    }
                }
                else if (input == 2 && contratosPF.Count() == 0 && contratosPJ.Count() == 0)
                    Console.WriteLine("Não há contratos para exibir.");
            }
        }
    }
}