namespace xadrez_console.tabuleiro
{
    abstract class Peca
    {
        public Posicao Posicao { get; set; }
        public Cor Cor { get; protected set; }
        public int QteMovimentos { get; protected set; }
        public  Tabuleiro Tab { get; set; }

        public Peca( Cor cor, Tabuleiro tab)
        {
            this.Posicao = null;
            this.Cor = cor;
            this.Tab = tab;
            this.QteMovimentos = 0;
        }

        public void IncrementaQtdeMovimentos()
        {
            QteMovimentos++;
        }

        public abstract bool[,] MovimentosPossiveis();
    }
}
