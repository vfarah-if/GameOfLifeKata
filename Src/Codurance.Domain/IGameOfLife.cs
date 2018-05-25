namespace GameOfLife.Domain
{
    public interface IGameOfLife
    {
        Life[,] Lives { get; }
        uint MatrixSize { get; }

        void Generate();
        void SeedLife(params Position[] positions);
        string ToString();
    }
}