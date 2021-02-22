﻿using System;
using xadrez_console.tabuleiro;
namespace xadrez_console.xadrez
{
    class PartidaDeXadrez
    {
        public Tabuleiro Tab { get; private set; }
        private int Turno;
        private Cor JogadorAtual;
        public bool Terminada { get; private set; }

        public PartidaDeXadrez()
        {
            Tab = new Tabuleiro(8, 8);
            Turno = 1;
            JogadorAtual = Cor.Branca;
            Terminada = false;
            ColocaPecas();
        }

        public void ExecutaMovimento(Posicao origem, Posicao destino)
        {
            Peca p = Tab.RetiraPeca(origem);
            p.IncrementaQtdeMovimentos();
            Peca pecaCapturada = Tab.RetiraPeca(destino);
            Tab.ColocaPeca(p, destino);
        }

        private void ColocaPecas()
        {
            Tab.ColocaPeca(new Torre(Cor.Branca, Tab), new PosicaoXadrez('c', 1).ToPosicao());
            Tab.ColocaPeca(new Torre(Cor.Branca, Tab), new PosicaoXadrez('c', 2).ToPosicao());
            Tab.ColocaPeca(new Torre(Cor.Branca, Tab), new PosicaoXadrez('d', 2).ToPosicao());
            Tab.ColocaPeca(new Torre(Cor.Branca, Tab), new PosicaoXadrez('e', 2).ToPosicao());
            Tab.ColocaPeca(new Torre(Cor.Branca, Tab), new PosicaoXadrez('e', 1).ToPosicao());
            Tab.ColocaPeca(new Torre(Cor.Branca, Tab), new PosicaoXadrez('d', 1).ToPosicao());

            Tab.ColocaPeca(new Torre(Cor.Preta, Tab), new PosicaoXadrez('c', 7).ToPosicao());
            Tab.ColocaPeca(new Torre(Cor.Preta, Tab), new PosicaoXadrez('c', 8).ToPosicao());
            Tab.ColocaPeca(new Torre(Cor.Preta, Tab), new PosicaoXadrez('d', 7).ToPosicao());
            Tab.ColocaPeca(new Torre(Cor.Preta, Tab), new PosicaoXadrez('e', 7).ToPosicao());
            Tab.ColocaPeca(new Torre(Cor.Preta, Tab), new PosicaoXadrez('e', 8).ToPosicao());
            Tab.ColocaPeca(new Torre(Cor.Preta, Tab), new PosicaoXadrez('d', 8).ToPosicao());
        }
    }
}
