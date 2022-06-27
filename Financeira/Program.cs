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
                input = menuInicial();
                if (input == -5)
                {
                    if (telaConfirmação() == 1)
                        sair = true;
                    goto programa;
                }
                else if ((input == -1 || input == -2) && verificarOpção(input) < 0)
                    goto menuInicial;
                else if (input == 1)
                {
                cadastrarContrato:
                    int status = 0;
                    status = cadastrarContrato(ref contratosPF, ref contratosPJ);
                    if (status == -5 || status == -3)
                    {
                        if (telaConfirmação() == 1)
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
                    status = consultarContrato(ref contratosPF, ref contratosPJ);
                    if (status == -5 || status == -3)
                    {
                        if (telaConfirmação() == 1)
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
        static public int consultarContrato(ref List<ContratoPessoaFísica> contratosPF, ref List<ContratoPessoaJurídica> contratosPJ)
        {
            int input = menuConsulta(contratosPF, contratosPJ);
            if (input == -5 || input == -3 || input == -4)
                return input;
            else if ((input < 0 && verificarOpção(input) < 0))
                return -2;
            else
                return input;
        }
        static int menuConsulta(List<ContratoPessoaFísica> contratosPF, List<ContratoPessoaJurídica> contratosPJ)
        {
        menuConsulta:
            Console.WriteLine(@"======== CONSULTA DE CONTRATO ========
RETORNAR - Retornar ao menu anterior.
SAIR - ENCERRAR o programa.
Por favor, informe CPF OU CNPJ:");
            string input = Console.ReadLine().ToUpper();
            if (input == "SAIR")
                return -5;
            else if (input == "")
                return -2;
            else if (input == "RETORNAR")
                return -3;
            tratarString(ref input);
            int status = 0;
            if (validaCPF(input))
            {
                status = consultaPF(input, contratosPF);
                if (status == 1)
                    goto menuConsulta;
                else
                    return status;
            }
            else if (validaCNPJ(input))
            {
                status = consultaPJ(input, contratosPJ);
                if (status == 1)
                    goto menuConsulta;
                else
                    return status;
            }
            else
            {
                Console.WriteLine("O valor informado não é válido.");
                return -10;
            }
        }
        static public int consultaPF(string input, List<ContratoPessoaFísica> contratosPF)
        {
            Dictionary<int, Guid> dictContratosCliente = new Dictionary<int, Guid>();
            int index = 1;
            var listaContratosCliente = contratosPF.Where(c => c.CPF.Contains(input)).ToList();
            foreach (var contrato in listaContratosCliente)
            {
                dictContratosCliente.Add(index, contrato.IdContrato);
                index++;
            }
            selecionarOpção:
            string opcao = "";
            if (dictContratosCliente.Count > 0)
            {
                Console.WriteLine(@$"--------------------------------------------------------------------------
O cliente possui {dictContratosCliente.Count()} contratos.
Por favor, selecione um dos contratos abaixo: ");
                foreach (var contrato in dictContratosCliente)
                    Console.WriteLine($"{contrato.Key} - Identificador do Contrato: {contrato.Value}");
                opcao = Console.ReadLine().ToUpper();
                int tempOpcao;
                Guid tempIdContrato;
                if (int.TryParse(opcao, out tempOpcao) && dictContratosCliente.TryGetValue(tempOpcao, out tempIdContrato))
                {
                    List<ContratoPessoaFísica> contratoPessoaFísica = contratosPF.Where(c => c.IdContrato == tempIdContrato).ToList();
                    foreach (var contrato in contratoPessoaFísica)
                        contrato.ExibirInfo();
                }
                else
                {
                    Console.WriteLine("Opcão inválida.");
                    goto selecionarOpção;
                }
            }
            else
                Console.WriteLine("Não existe contrato para este cliente.");
            return menuConsultarMais();
        }
        static public int consultaPJ(string input, List<ContratoPessoaJurídica> contratosPJ)
        {
            Dictionary<int, Guid> dictContratosCliente = new Dictionary<int, Guid>();
            int index = 1;
            var listaContratosCliente = contratosPJ.Where(c => c.CNPJ.Contains(input)).ToList();
            foreach (var contrato in listaContratosCliente)
            {
                dictContratosCliente.Add(index, contrato.IdContrato);
                index++;
            }
        selecionarOpção:
            string opcao = "";
            if (dictContratosCliente.Count > 0)
            {
                Console.WriteLine(@$"--------------------------------------------------------------------------
O cliente possui {dictContratosCliente.Count()} contratos.
Por favor, selecione um dos contratos abaixo: ");
                foreach (var contrato in dictContratosCliente)
                    Console.WriteLine($"{contrato.Key} - Identificador do Contrato: {contrato.Value}");
                opcao = Console.ReadLine().ToUpper();
                int tempOpcao;
                Guid tempIdContrato;
                if (int.TryParse(opcao, out tempOpcao) && dictContratosCliente.TryGetValue(tempOpcao, out tempIdContrato))
                {
                    List<ContratoPessoaJurídica> contratoPessoaJurídica = contratosPJ.Where(c => c.IdContrato == tempIdContrato).ToList();
                    foreach (var contrato in contratoPessoaJurídica)
                        contrato.ExibirInfo();
                }
                else
                {
                    Console.WriteLine("Opcão inválida.");
                    goto selecionarOpção;
                }
            }
            else
                Console.WriteLine("Não existe contrato para este cliente.");
            return menuConsultarMais();
        }
        #region menuConsultarMais
        static int menuConsultarMais()
        {
        menuConsultarMais:
            Console.WriteLine(@"--------------------------------------------------------------------------
Deseja consultar novos contratos?
RETORNAR - Retornar ao menu anterior.
SAIR - ENCERRAR o programa.
1 - Sim
2 - Não");
            string input = Console.ReadLine();
            if (input == "SAIR")
                return -5;
            else if (input == "RETORNAR")
                return -3;
            else if (input == "" || (input != "1" && input != "2"))
            {
                Console.WriteLine("Valor inválido.");
                goto menuConsultarMais;
            }
            else if (input == "1")
                return 1;
            else
                return -4;
        }
        #endregion

        #region MenuInicial
        static int menuInicial()
        {
            Console.WriteLine(@"======== MENU INICIAL ========
Por favor, informe a opção desejada:
SAIR - Encerrar o programa
1 - Cadastro de Contrato
2 - Consulta de Contrato");
            string input = Console.ReadLine().ToUpper();
            if (input == "SAIR")
                return -5;
            else if (input == "")
                return -2;
            int opcaoEscolhida = 0;
            if (int.TryParse(input, out opcaoEscolhida))
                return opcaoEscolhida;
            else
                return -1;
        }
        #endregion
        #region VerificarOpção
        static int verificarOpção(int input)
        {
            if (input == -1 || input == -2)
            {
                Console.WriteLine("Opção inválida.");
                return input;
            }
            return input;
        }
        #endregion
        #region telaConfirmação
        static int telaConfirmação()
        {
            Console.WriteLine(@"======== TELA DE CONFIRMAÇÃO ========
Deseja prosseguir?
1 - SIM
2 - NÃO");
            string input = Console.ReadLine().ToUpper();
            int opcaoEscolhida = 0;
            int.TryParse(input, out opcaoEscolhida);
            if (opcaoEscolhida == 1 || opcaoEscolhida == 2)
                return opcaoEscolhida;
            else
            {
                Console.WriteLine("Opção inválida.");
                return telaConfirmação();
            }
        }
        #endregion
        #region cadastrarContrato
        static int cadastrarContrato(ref List<ContratoPessoaFísica> contratosPF, ref List<ContratoPessoaJurídica> contratosPJ)
        {
            int input = menuCadastro(ref contratosPF, ref contratosPJ);
            if (input == -5 || input == -3 || input == -4)
                return input;
            else if ((input < 0 && verificarOpção(input) < 0))
                return -2;
            else
                return input;
        }
        #endregion
        #region menuCadastro
        static int menuCadastro(ref List<ContratoPessoaFísica> contratosPF, ref List<ContratoPessoaJurídica> contratosPJ)
        {
        menuCadastro:
            Console.WriteLine(@"======== CADASTRO DE CONTRATO ========
RETORNAR - Retornar ao menu anterior.
SAIR - ENCERRAR o programa.
Por favor, informe CPF OU CNPJ:");
            string input = Console.ReadLine().ToUpper();
            if (input == "SAIR")
                return -5;
            else if (input == "")
                return -2;
            else if (input == "RETORNAR")
                return -3;
            tratarString(ref input);
            int status = 0;
            if (validaCPF(input))
            {
                status = cadastrarPF(input, ref contratosPF);
                if (status == 1)
                    goto menuCadastro;
                else
                    return status;
            }
            else if (validaCNPJ(input))
            {
                status = cadastrarPJ(input, ref contratosPJ);
                if (status == 1)
                    goto menuCadastro;
                else
                    return status;
            }
            else
            {
                Console.WriteLine("O valor informado não é válido.");
                return -10;
            }
        }
        #endregion
        #region tratarString
        static void tratarString(ref string input)
        {
            string temp = input;
            input = temp.Replace(" ", "");
            temp = input;
            input = temp.Replace(".", "");
            temp = input;
            input = temp.Replace("-", "");
            temp = input;
            input = temp.Replace("/", "");
        }
        #endregion
        #region validaCPF
        public static bool validaCPF(string cpf)
        {
            int[] multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            string tempCpf;
            string digito;
            int soma;
            int resto;
            cpf = cpf.Trim();
            cpf = cpf.Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;
            tempCpf = cpf.Substring(0, 9);
            soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCpf = tempCpf + digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cpf.EndsWith(digito);
        }
        #endregion
        #region validaCNPJ
        public static bool validaCNPJ(string cnpj)
        {
            int[] multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int[] multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            int soma;
            int resto;
            string digito;
            string tempCnpj;
            cnpj = cnpj.Trim();
            cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;
            tempCnpj = cnpj.Substring(0, 12);
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = resto.ToString();
            tempCnpj = tempCnpj + digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            resto = (soma % 11);
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;
            digito = digito + resto.ToString();
            return cnpj.EndsWith(digito);
        }
        #endregion
        #region cadastrarPJ
        static int cadastrarPJ(string cnpj, ref List<ContratoPessoaJurídica> contratosPJ)
        {
            int input;
            string tempIE = "";
            input = solicitarIE(ref tempIE);
            if (input != 1)
                return input;
            string tempContratante = "";
            input = solicitarContratante(ref tempContratante);
            if (input != 1)
                return input;
            decimal tempValor = 0;
            input = solicitarValor(ref tempValor);
            if (input != 1)
                return input;
            int tempPrazo = 0;
            input = solicitarPrazo(ref tempPrazo);
            if (input != 1)
                return input;
            ContratoPessoaJurídica contrato = new ContratoPessoaJurídica(tempContratante, tempValor, tempPrazo, cnpj, tempIE);
            contratosPJ.Add(contrato);
            return menuCadastrarMais();
        }
        #endregion
        #region cadastrarPF
        static int cadastrarPF(string cpf, ref List<ContratoPessoaFísica> contratosPF)
        {
            int input;
            string tempContratante = "";
            input = solicitarContratante(ref tempContratante);
            if (input != 1)
                return input;
            decimal tempValor = 0;
            input = solicitarValor(ref tempValor);
            if (input != 1)
                return input;
            int tempPrazo = 0;
            input = solicitarPrazo(ref tempPrazo);
            if (input != 1)
                return input;
            DateOnly tempDataNascimento;
            input = solicitarDataNascimento(ref tempDataNascimento);
            if (input != 1)
                return input;
            ContratoPessoaFísica contrato = new ContratoPessoaFísica(tempContratante, tempValor, tempPrazo, cpf, tempDataNascimento);
            contratosPF.Add(contrato);
            return menuCadastrarMais();
        }
        #endregion
        #region menuCadastrarMais
        static int menuCadastrarMais()
        {
        menuCadastrarMais:
            Console.WriteLine(@"--------------------------------------------------------------------------
Deseja cadastrar novos contratos?
RETORNAR - Retornar ao menu anterior.
SAIR - ENCERRAR o programa.
1 - Sim
2 - Não");
            string input = Console.ReadLine();
            if (input == "SAIR")
                return -5;
            else if (input == "RETORNAR")
                return -3;
            else if (input == "" || (input != "1" && input != "2"))
            {
                Console.WriteLine("Valor inválido.");
                goto menuCadastrarMais;
            }
            else if (input == "1")
                return 1;
            else
                return -4;
        }
        #endregion
        #region solicitarIE
        static int solicitarIE(ref string iEstadual)
        {
            Console.WriteLine(@"--------------------------------------------------------------------------
Por favor, informe a Inscrição Estadual do Contratante:");
            iEstadual = Console.ReadLine().ToUpper();
            if (iEstadual == "")
            {
                Console.WriteLine("Valor inválido.");
                solicitarContratante(ref iEstadual);
            }
            else if (iEstadual == "SAIR")
                return -5;
            else if (iEstadual == "RETORNAR")
                return -3;
            tratarString(ref iEstadual);
            if (validaIE(iEstadual) == false)
                solicitarIE(ref iEstadual);
            return 1;
        }
        #endregion
        #region validaIE
        public static bool validaIE(string iEstadual)
        {
            int lenght = iEstadual.Length;
            int parser = 0;
            while (parser < lenght && (iEstadual[parser] >= '0' && iEstadual[parser] <= '9'))
                parser++;
            if (parser == 12)
                return true;
            else
            {
                Console.WriteLine("Inscrição Estadual inválida! Formato esperado: \"XXX.XXX.XXX.XXX\"");
                return false;
            }
        }
        #endregion
        #region solicitarContratante
        static int solicitarContratante(ref string contratante)
        {
            Console.WriteLine(@"--------------------------------------------------------------------------
Por favor, informe o nome do Contratante:");
            contratante = Console.ReadLine().ToUpper();
            if (contratante == "")
            {
                Console.WriteLine("Valor inválido.");
                solicitarContratante(ref contratante);
            }
            else if (contratante == "SAIR")
                return -5;
            else if (contratante == "RETORNAR")
                return -3;
            return 1;
        }
        #endregion
        #region solicitarValor
        static int solicitarValor(ref decimal valor)
        {
            Console.WriteLine(@"--------------------------------------------------------------------------
Por favor, informe o valor do Contrato:");

            string input = Console.ReadLine().ToUpper();
            if (input == "")
            {
                Console.WriteLine("Valor inválido.");
                solicitarValor(ref valor);
            }
            else if (input == "SAIR")
                return -5;
            else if (input == "RETORNAR")
                return -3;
            if (!decimal.TryParse(input, out valor) || valor <= 0)
            {
                Console.WriteLine("Valor inválido");
                solicitarValor(ref valor);
            }
            return 1;
        }
        #endregion
        #region solicitarPrazo
        static int solicitarPrazo(ref int prazo)
        {
            Console.WriteLine(@"--------------------------------------------------------------------------
Por favor, informe o Prazo do Contrato:");
            string input = Console.ReadLine().ToUpper();
            if (input == "")
            {
                Console.WriteLine("Valor inválido.");
                solicitarPrazo(ref prazo);
            }
            else if (input == "SAIR")
                return -5;
            else if (input == "RETORNAR")
                return -3;
            if (!int.TryParse(input, out prazo) || prazo <= 0)
            {
                Console.WriteLine("Valor inválido");
                solicitarPrazo(ref prazo);
            }
            return 1;
        }
        #endregion
        #region solicitarDataNascimento
        static int solicitarDataNascimento(ref DateOnly DataNascimento)
        {
        solicitarDataNascimento:
            Console.WriteLine(@"--------------------------------------------------------------------------
Por favor, informe a Data de Nascimento do Contratante:");
            string input = Console.ReadLine().ToUpper();
            if (input == "")
            {
                Console.WriteLine("Valor inválido.");
                solicitarDataNascimento(ref DataNascimento);
            }
            else if (input == "SAIR")
                return -5;
            else if (input == "RETORNAR")
                return -3;
            DateTime dataAtual = DateTime.Now;
            DateTime tempDataNascimento;
            if (!DateTime.TryParse(input, out tempDataNascimento))
            {
                Console.WriteLine("Por favor, informe uma Data de Nascimento válida no formato dd/mm/yyyy.");
                goto solicitarDataNascimento;
            }
            if (!DateOnly.TryParse(input, out DataNascimento) || tempDataNascimento > dataAtual)
            {
                Console.WriteLine("Valor inválido");
                solicitarDataNascimento(ref DataNascimento);
            }
            return 1;
        }
        #endregion
    }
}