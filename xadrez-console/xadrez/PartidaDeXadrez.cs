using System.Collections.Generic;
using xadrez_console.tabuleiro;
namespace xadrez_console.xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set; }
        public int Turno { get; private set; }
        public Cor JogadorAtual { get; private set; }
        public bool Terminada { get; private set; }
        private HashSet<Peca> Pecas;
        private HashSet<Peca> PecasCapturadas;

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            Pecas = new HashSet<Peca>();
            PecasCapturadas = new HashSet<Peca>();
            ColocaPecas();
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetiraPeca(origem);
            p.IncrementaQtdeMovimentos();
            Peca pecaCapturada = Tab.RetiraPeca(destino);
            Tab.ColocaPeca(p, destino);
            if(pecaCapturada != null)
            {
                PecasCapturadas.Add(pecaCapturada);
            }
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            ExecutaMovimento(origem, destino);
            Turno++;
            MudaJogador();
        }

        public void ValidaPosicaoDeOrigem(Posicao pos)
        {
            if(Tab.Peca(pos) == null)
            {
                throw new TabuleiroException("Não existe peça na posição de origem escolhida!");
            }
            if (JogadorAtual != Tab.Peca(pos).Cor)
            {
                throw new TabuleiroException("A peça de origem escolhida não é sua!");
            }
            if(!Tab.Peca(pos).ExisteMovimentosPossiveis())
            { 
                throw new TabuleiroException("Não há movimentos possíveis para a peça de origem escolhida!");
            }
        }

        public void ValidaPosicaoDeDestino(Posicao origem, Posicao destino)
        {
            if (!Tab.Peca(origem).PodeMoverPara(destino))
            {
                throw new TabuleiroException("Posição de destino inválida!");
            }
        }

        private void MudaJogador()
        {
            if(JogadorAtual == Cor.Branca)
            {
                JogadorAtual = Cor.Preta;
            }
            else
            {
                JogadorAtual = Cor.Branca;
            }
        }

        public HashSet<Peca> Capturadas(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach(Peca x in PecasCapturadas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            return aux;
        }

        public HashSet<Peca> PecasEmJogo(Cor cor)
        {
            HashSet<Peca> aux = new HashSet<Peca>();
            foreach (Peca x in Pecas)
            {
                if (x.Cor == cor)
                {
                    aux.Add(x);
                }
            }
            aux.ExceptWith(Capturadas(cor));
            return aux;
        }

        public void ColocaNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocaPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);
        }

        private void ColocaPecas()
        {
            ColocaNovaPeca('c', 1, new Torre(Cor.Branca, Tab));
            ColocaNovaPeca('c', 2, new Torre(Cor.Branca, Tab));
            ColocaNovaPeca('d', 2, new Torre(Cor.Branca, Tab));
            ColocaNovaPeca('e', 2, new Torre(Cor.Branca, Tab));
            ColocaNovaPeca('e', 1, new Torre(Cor.Branca, Tab));
            ColocaNovaPeca('d', 1, new Torre(Cor.Branca, Tab));

            ColocaNovaPeca('c', 7, new Torre(Cor.Preta, Tab));
            ColocaNovaPeca('c', 8, new Torre(Cor.Preta, Tab));
            ColocaNovaPeca('d', 7, new Torre(Cor.Preta, Tab));
            ColocaNovaPeca('e', 7, new Torre(Cor.Preta, Tab));
            ColocaNovaPeca('e', 8, new Torre(Cor.Preta, Tab));
            ColocaNovaPeca('d', 8, new Torre(Cor.Preta, Tab));
        }
    }
}
