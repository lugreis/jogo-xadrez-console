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
        public bool Xeque { get; private set; }

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

        public Peca ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetiraPeca(origem);
            p.IncrementaQtdeMovimentos();
            Peca pecaCapturada = Tab.RetiraPeca(destino);
            Tab.ColocaPeca(p, destino);
            if(pecaCapturada != null)
            {
                PecasCapturadas.Add(pecaCapturada);
            }
            return pecaCapturada;
        }

        public void DesfazMovimento(Posicao origem, Posicao destino, Peca pecaCapturada)
        {
            Peca p = Tab.RetiraPeca(destino);
            p.DecrementaQtdeMovimentos();
            if(pecaCapturada != null)
            {
                Tab.ColocaPeca(pecaCapturada, destino);
                PecasCapturadas.Remove(pecaCapturada);
            }
            Tab.ColocaPeca(p, origem);
        }

        public void RealizaJogada(Posicao origem, Posicao destino)
        {
            Peca pecaCapturada = ExecutaMovimento(origem, destino);

            if(EstaEmXeque(JogadorAtual))
            {
                DesfazMovimento(origem, destino, pecaCapturada);
                throw new TabuleiroException("Você não pode se colocar em xeque!");
            }

            if (EstaEmXeque(Adversaria(JogadorAtual)))
            {
                Xeque = true;
            }
            else
            {
                Xeque = false;
            }

            if(TestaXequemate(Adversaria(JogadorAtual)))
            {
                Terminada = true;
            }
            else
            {
                Turno++;
                MudaJogador();
            }
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
            if (!Tab.Peca(origem).MovimentoPossivel(destino))
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
        
        private Cor Adversaria(Cor cor)
        {
            if(cor == Cor.Branca)
            {
                return Cor.Preta;
            }
            else
            {
                return Cor.Branca;
            }
        }

        private Peca Rei(Cor cor)
        {
            foreach(Peca x in PecasEmJogo(cor))
            {
                if(x is Rei)
                {
                    return x;
                }
            }
            return null;
        }

        public bool EstaEmXeque(Cor cor)
        {
            Peca R = Rei(cor);
            if(R == null)
            {
                throw new TabuleiroException("Não tem rei da cor" + cor + " no tabuleiro");
            }
            foreach(Peca x in PecasEmJogo(Adversaria(cor)))
            {
                bool[,] mat = x.MovimentosPossiveis();
                if(mat[R.Posicao.Linha, R.Posicao.Coluna])
                {
                    return true;
                }
            }
            return false;
        }

        public bool TestaXequemate(Cor cor)
        {
            if(!EstaEmXeque(cor))
            {
                return false;
            }
            foreach(Peca x in PecasEmJogo(cor))
            {
                bool[,] mat = x.MovimentosPossiveis();
                for(int i = 0; i < Tab.Linhas; i++)
                {
                    for(int j = 0; j < Tab.Colunas; j++)
                    {
                        if(mat[i, j])
                        {
                            Posicao origem = x.Posicao;
                            Posicao destino = new Posicao(i, j);
                            Peca pecaCapturada = ExecutaMovimento(origem, destino);
                            bool testeXeque = EstaEmXeque(cor);
                            DesfazMovimento(origem, destino, pecaCapturada);
                            if(!testeXeque)
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            return true;
        }

        public void ColocaNovaPeca(char coluna, int linha, Peca peca)
        {
            Tab.ColocaPeca(peca, new PosicaoXadrez(coluna, linha).ToPosicao());
            Pecas.Add(peca);
        }

        private void ColocaPecas()
        {
            ColocaNovaPeca('a', 1, new Torre(Cor.Branca, Tab));
            ColocaNovaPeca('b', 1, new Cavalo(Cor.Branca, Tab));
            ColocaNovaPeca('c', 1, new Bispo(Cor.Branca, Tab));
            ColocaNovaPeca('d', 1, new Dama(Cor.Branca, Tab));
            ColocaNovaPeca('e', 1, new Rei(Cor.Branca, Tab));
            ColocaNovaPeca('f', 1, new Bispo(Cor.Branca, Tab));
            ColocaNovaPeca('g', 1, new Cavalo(Cor.Branca, Tab));
            ColocaNovaPeca('h', 1, new Torre(Cor.Branca, Tab));
            ColocaNovaPeca('a', 2, new Peao(Cor.Branca, Tab));
            ColocaNovaPeca('b', 2, new Peao(Cor.Branca, Tab));
            ColocaNovaPeca('c', 2, new Peao(Cor.Branca, Tab));
            ColocaNovaPeca('d', 2, new Peao(Cor.Branca, Tab));
            ColocaNovaPeca('e', 2, new Peao(Cor.Branca, Tab));
            ColocaNovaPeca('f', 2, new Peao(Cor.Branca, Tab));
            ColocaNovaPeca('g', 2, new Peao(Cor.Branca, Tab));
            ColocaNovaPeca('h', 2, new Peao(Cor.Branca, Tab));

            ColocaNovaPeca('a', 8, new Torre(Cor.Preta, Tab));
            ColocaNovaPeca('b', 8, new Cavalo(Cor.Preta, Tab));
            ColocaNovaPeca('c', 8, new Bispo(Cor.Preta, Tab));
            ColocaNovaPeca('d', 8, new Dama(Cor.Preta, Tab));
            ColocaNovaPeca('e', 8, new Rei(Cor.Preta, Tab));
            ColocaNovaPeca('f', 8, new Bispo(Cor.Preta, Tab));
            ColocaNovaPeca('g', 8, new Cavalo(Cor.Preta, Tab));
            ColocaNovaPeca('h', 8, new Torre(Cor.Preta, Tab));
            ColocaNovaPeca('a', 7, new Peao(Cor.Preta, Tab));
            ColocaNovaPeca('b', 7, new Peao(Cor.Preta, Tab));
            ColocaNovaPeca('c', 7, new Peao(Cor.Preta, Tab));
            ColocaNovaPeca('d', 7, new Peao(Cor.Preta, Tab));
            ColocaNovaPeca('e', 7, new Peao(Cor.Preta, Tab));
            ColocaNovaPeca('f', 7, new Peao(Cor.Preta, Tab));
            ColocaNovaPeca('g', 7, new Peao(Cor.Preta, Tab));
            ColocaNovaPeca('h', 7, new Peao(Cor.Preta, Tab));
        }
    }
}
