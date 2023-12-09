using System;
using System.Linq;
using System.Threading;

class Usuario
{
    public string NOME { get; set; }
    public int ENERGIA { get; set; }
    public int PONTOS { get; set; }
    public int GOLS { get; set; }

    public Usuario(string nome)
    {
         NOME= nome;
        ENERGIA = 10;
        PONTOS = 0;
        GOLS = 0;
    }
}

public class Program
{
    public static void Main()
    {
        while (true)
        {
            Console.Write("INSIRA O NOME DO PRIMEIRO JOGADOR: ");
            string nomeUsua01 = Console.ReadLine();
            Usuario user1 = new Usuario(nomeUsua01);

            Console.Write("EXISTIRA UM SEGUNDO JOGADOR HUMANO? (s/n): ");
            string retorno = Console.ReadLine();
            Console.WriteLine();

            Usuario user2;
            if (retorno.ToLower() == "s")
            {
                Console.Write("INSIRA O NOME DO SEGUNDO JOGADOR: ");
                string nomeUsua02 = Console.ReadLine();
                user2 = new Usuario(nomeUsua02);
            }
            else
            {
                user2 = new Usuario("COMPUTADOR");
            }

            user1.ENERGIA = 10;
            user2.ENERGIA = 10;

            string[] cartas = { "GOOOL", "PENALTI", "FALTA", "CARTAO AMARELO", "CARTAO VERMELHO", "ENERGIA" };

            while (user1.ENERGIA > 0 || user2.ENERGIA > 0)
            {
                for (int JOGADOR = 0; JOGADOR < 2; JOGADOR++)
                {
                    Usuario PLAYERAtual = (JOGADOR == 0) ? user1 : user2;
                    Usuario adversario = (JOGADOR == 0) ? user2 : user1;

                    if (PLAYERAtual.ENERGIA > 0)
                    {
                        Console.WriteLine("\nVEZ DE " + (JOGADOR == 0 ? user1.NOME : user2.NOME) + ":");

                        if (PLAYERAtual.NOME == "COMPUTADOR")
                        {
                            Console.WriteLine("O COMPUTADOR ESTA PENSANDO...");
                            Thread.Sleep(2000);

                            string[] CARTASComput = SortearAcoes();
                            Console.WriteLine("Ações: " + CARTASComput[0] + " / " + CARTASComput[1] + " / " + CARTASComput[2]);

                            VerificarRodada(CARTASComput, PLAYERAtual, adversario);
                        }
                        else
                        {
                            Console.Write("PRESSIONE QUALQUER TECLA PARA REALIZAR UMA ACAO...");
                            Console.ReadLine();

                            string[] CARTASJogador = SortearAcoes();
                            Console.WriteLine("Ações: " + CARTASJogador[0] + " / " + CARTASJogador[1] + " / " + CARTASJogador[2]);

                            VerificarRodada(CARTASJogador, PLAYERAtual, adversario);
                        }

                        PLAYERAtual.ENERGIA--;

                        Console.WriteLine("Energia restante - " + PLAYERAtual.NOME + ": " + PLAYERAtual.ENERGIA + " | " + adversario.NOME + ": " + adversario.ENERGIA);
                        Console.WriteLine("Gols             - " + PLAYERAtual.NOME + ": " + PLAYERAtual.GOLS + " | " + adversario.NOME + ": " + adversario.GOLS);
                        Console.WriteLine("Pontos           - " + PLAYERAtual.NOME + ": " + PLAYERAtual.PONTOS + " | " + adversario.NOME + ": " + adversario.PONTOS);
                    }
                    else
                    {
                        Console.WriteLine(PLAYERAtual.NOME + " NAO PODE REALIZAR UMA ACAO. ENERGIA INSUFICIENTE.");
                    }

                    if (user1.GOLS != user2.GOLS && (user1.ENERGIA == 0 || user2.ENERGIA == 0))
                        break;
                }

                if (user1.GOLS != user2.GOLS && (user1.ENERGIA == 0 || user2.ENERGIA == 0))
                    break;
            }

            ExibirResultadoFinal(user1, user2);

            Console.Write("INSIRA '-1' PARA SAIR OU '0' PARA UMA NOVA PARTIDA: ");
            string OPCAO = Console.ReadLine();

            if (OPCAO == "-1")
                break;
        }
    }

    static void VerificarRodada(string[] cartas, Usuario PLAYERAtual, Usuario adversario)
    {
        if (cartas[0] == "GOOL" && cartas[1] == "Gol" && cartas[2] == "GOOL")
        {
            PLAYERAtual.GOLS++;
            Console.WriteLine("GOOL! O JOGADOR " + PLAYERAtual.NOME + " MARCOU UM GOOL!");
            Console.WriteLine("PLACAR ATUAlIZADO: " + PLAYERAtual.NOME + ": " + PLAYERAtual.GOLS + " | " + adversario.NOME + ": " + adversario.GOLS);
        }
        else if (cartas[0] == "ENERGIA" && cartas[1] == "ENERGIA" && cartas[2] == "ENERGIA")
        {
            PLAYERAtual.ENERGIA++;
            Console.WriteLine("O JOGADOR " + PLAYERAtual.NOME + " ADQUIRIU UMA ENERGIA EXTRA!");
            Console.WriteLine("ENERGIA ATUALIZADA: " + PLAYERAtual.NOME + ": " + PLAYERAtual.ENERGIA + " | " + adversario.NOME + ": " + adversario.ENERGIA);
        }
        else if (cartas[0] == "PENALTI" && cartas[1] == "PENALTI" && cartas[2] == "PENALTI")
        {
            Console.WriteLine("JOGADOR " + PLAYERAtual.NOME + " CONTINUARA JOGANDO E DEVERA ESCOLHER OUTRAS TRES OPCOES (DIREITA, ESQUERDA OU CENTRO).");

            string[] OPCAOPenalti = { "DIREITA", "ESQUERDA", "CENTRO" };
            string escolhaJogador = ObterEscolha(OPCAOPenalti);

            Console.WriteLine("VEZ DO ADVERSARIO TENTAR DEFENDER...");

            string[] OPCAODefesa= { "DIREITA", "ESQUERDA", "CENTRO" };
            string escolhaAdversario = (adversario.NOME == "COMPUTADOR") ? SortearOpcao(OPCAODefesa) : ObterEscolha(OPCAODefesa);

            if (escolhaJogador != escolhaAdversario)
            {
                PLAYERAtual.GOLS++;
                Console.WriteLine("GOOL! O JOGADOR " + PLAYERAtual.NOME + " MARCOU UM GOOL!");
                Console.WriteLine("PLACAR ATUALIZADO: " + PLAYERAtual.NOME + ": " + PLAYERAtual.GOLS + " | " + adversario.NOME + ": " + adversario.GOLS);
            }
            else
            {
                Console.WriteLine("DEFENDEUUUU!!! NAO FOI CONVERTIDO EM GOOl.");
                Console.WriteLine("PLACAR ATUALIZADO: " + PLAYERAtual.NOME + ": " + PLAYERAtual.GOLS + " | " + adversario.NOME + ": " + adversario.GOLS);
            }
        }
        else if (cartas[0] == "FALTA" && cartas[1] == "FALTA" && cartas[2] == "FALTA")
        {
            Console.WriteLine("JOGADOR " + PLAYERAtual.NOME + " PASSE A VEZ PARA O ADVERSARIO.");
        }
        else if (cartas[0] == "CARTAO AMARELO" && cartas[1] == "CARTAO AMARELO" && cartas[2] == "CARTAO AMARELO")
        {
            PLAYERAtual.ENERGIA--;

            if (PLAYERAtual.ENERGIA == 0)
            {
                Console.WriteLine(PLAYERAtual.NOME + " PERDEU UMA ENERGIA. NO PROXIMO CARTAO AMARELO, PERDERA DUAS ENERGIAS E PASSARA A VEZ PARA O ADVERSARIO.");
            }
            else
            {
                Console.WriteLine(PLAYERAtual.NOME + " PERDEU UMA ENERGIA. ENERGIA RESTANTE: " + PLAYERAtual.ENERGIA);
            }
        }
        else if (cartas[0] == "CARTAO VERMELHO" && cartas[1] == "CARTAO VERMELHO" && cartas[2] == "CARTAO VERMELHO")
        {
            PLAYERAtual.ENERGIA -= 2;

            if (PLAYERAtual.ENERGIA <= 0)
            {
                Console.WriteLine(PLAYERAtual.NOME + "PERDEU DUAS ENERGIAS E PASSA A VEZ PARA O ADVERSARIO.");
            }
            else
            {
                Console.WriteLine(PLAYERAtual.NOME + "PERDEU DUAS ENERGIAS. ENERGIA RESTANTE: " + PLAYERAtual.ENERGIA);
            }
        }
        else
        {
            int pontosRodada = CalcularPontos(cartas, PLAYERAtual);
            Console.WriteLine("Pontuação da rodada: " + pontosRodada + " pontos.");
        }
    }

    static int CalcularPontos(string[] cartas, Usuario PLAYERAtual)
    {
        int pontos = 0;
        foreach (var carta in cartas)
        {
            switch (carta)
            {
                case "Gol":
                    pontos += 3;
                    break;
                case "Pênalti":
                    pontos += 2;
                    break;
                case "Falta":
                case "Cartão Amarelo":
                    pontos += 1;
                    break;
                case "Cartão Vermelho":
                    break; // Não INSERIR pontos
                case "Energia":
                    pontos += 2;
                    break;
            }
        }
        PLAYERAtual.PONTOS += pontos;
        return pontos;
    }

    static string[] SortearAcoes()
    {
        string[] cartas = { "GOOL", "PENALTI", "FALTA", "CARTAO AMARELO", "CARTAO VERMELHO", "ENERGIA" };
        return cartas.OrderBy(x => Guid.NewGuid()).ToArray();
    }

    static string SortearOpcao(string[] opcoes)
    {
        return opcoes[new Random().Next(opcoes.Length)];
    }

    static string ObterEscolha(string[] opcoes)
    {
        Console.WriteLine("ESCOLHA UMA OPCAO:");
        for (int i = 0; i < opcoes.Length; i++)
        {
            Console.WriteLine((i + 1) + ". " + opcoes[i]);
        }

        int escolha;
        while (true)
        {
            Console.Write("DIGITE O NUMERO DA OPCAO: ");
            if (int.TryParse(Console.ReadLine(), out escolha) && escolha >= 1 && escolha <= opcoes.Length)
            {
                break;
            }
            Console.WriteLine("ESCOLHA INCORRETA. TENTE NOVAMENTE.");
        }

        return opcoes[escolha - 1];
    }

    static void ExibirResultadoFinal(Usuario user1, Usuario user2)
    {
        Console.WriteLine("PARTIDA ENCERRADA!");

        if (user1.GOLS > user2.GOLS)
        {
            Console.WriteLine("PARABENS JOGADOR 1");
            Console.WriteLine("VOCE VENCEU COM " + user1.GOLS + " GOLS E " + user1.PONTOS + " PONTOS.");
            Console.WriteLine("O SEU ADVERSARIO FEZ " + user2.GOLS + " GOLS E " + user2.PONTOS + " PONTOS");
        }
        else if (user2.GOLS > user1.GOLS)
        {
            Console.WriteLine("PARABENS JOGADOR 2");
            Console.WriteLine("VOCE VENCEU COM " + user2.GOLS + " GOLS E " + user2.PONTOS + " PONTOS.");
            Console.WriteLine("O SEU ADVERSARIO FEZ " + user1.GOLS + " GOLS E " + user1.PONTOS + " PONTOS");
        }
        else
        {
            if (user1.PONTOS > user2.PONTOS)
            {
                Console.WriteLine("A PARTIDA EMPATOU!");
                Console.WriteLine("AMBOS OS JOGADORES FIZERAM A MESMA QUANTIDADE DE GOLS, POREM  O JOGADOR 1 VENCEU PELA A SUA MELHOR PONTUACAO.");
            }
            else if (user2.PONTOS > user1.PONTOS)
            {
                Console.WriteLine("A PARTIDA EMPATOU!");
                Console.WriteLine("AMBOS OS JOGADORES FIZERAM A MESMA QUANTIDADE DE GOLS, POREM  O JOGADOR 2 VENCEU PELA A SUA MELHOR PONTUACAO.");
            }
            else
            {
                Console.WriteLine("A PARTIDA EMPATOU!");
                Console.WriteLine("AMBOS OS JOGADORES FIZERAM A MESMA QUANTIDADE DE GOLS E A MESMA PONTUACAO.");
            }
        }
    }
}
