namespace GameOfLife.Domain
{
    public interface IGameOfLife
    {
        Cell[,] Cells { get; }        
        uint ColumnSize { get; }
        uint RowSize { get; }

        void Generate();
        void SeedLife(params Position[] positions);
        string ToString();
    }
}