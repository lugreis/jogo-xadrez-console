using System;
using xadrez_console.tabuleiro;
using xadrez_console.xadrez;

namespace xadrez_console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                PartidaDeXadrez partida = new PartidaDeXadrez();

                while(!partida.Terminada)
                {
                    Console.Clear();
                    Tela.ImprimeTabuleiro(partida.Tab);

                    Console.WriteLine();
                    Console.Write("Origem: ");
                    Posicao origem = Tela.LePosicaoXadrez().ToPosicao();

                    bool[,] posicoesPossiveis = partida.Tab.Peca(origem).MovimentosPossiveis();

                    Console.Clear();
                    Tela.ImprimeTabuleiro(partida.Tab, posicoesPossiveis);

                    Console.WriteLine();
                    Console.Write("Destino: ");
                    Posicao destino = Tela.LePosicaoXadrez().ToPosicao();

                    partida.ExecutaMovimento(origem, destino);
                }
            

                Tela.ImprimeTabuleiro(partida.Tab);
            }
            catch(TabuleiroException e)
            {
                Console.WriteLine(e.Message);
            }

            Console.ReadLine();
        }
    }
}
