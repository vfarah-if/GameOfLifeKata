namespace GameOfLife.Domain
{
    public interface IGameOfLife
    {
        Life[,] Lives { get; }
        uint RowSize { get;  }
        uint ColumnSize { get; }

        void Generate();
        void SeedLife(params Position[] positions);
        string ToString();
    }
}