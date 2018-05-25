using System;
using static GameOfLife.Domain.ErrorMessages;

namespace GameOfLife.Domain
{
    //The "game" is a zero-player game, meaning that its evolution is determined by its initial state, requiring no further input.One interacts with the Game of Life by creating an initial configuration and observing how it evolves, or, for advanced "players", by creating patterns with particular properties. This is a simplified version allowing 3 distinct seeds, a diamond shape, a square, and a cross shape.

    //The initial pattern constitutes the** seed** of the system.The first generation is created by applying the above rules simultaneously to every cell in the seed—births and deaths occur simultaneously, and the discrete moment at which this happens is sometimes called a** tick** (in other words, each generation is a pure function of the preceding one). The rules continue to be applied repeatedly to create further generations.

    //*Conway chose his rules carefully*, after considerable experimentation, to meet these criteria:
    //1. There should be no **explosive growth**.
    //2. There should exist** small initial patterns** with chaotic, unpredictable outcomes.
    //3. There should be potential for von Neumann universal constructors.


    public class GameOfLife
    {
        public const uint MINIMUM_MATRIX_SIZE = 2;

        public GameOfLife(uint matrixSize)
        {
            if (matrixSize < MINIMUM_MATRIX_SIZE)
            {
                throw new ArgumentOutOfRangeException(MatrixSizeOutOfRange);
            }
            this.MatrixSize = matrixSize;
            this.Initialise();
        }

        public uint MatrixSize { get; }
        public Life[,] Lives { get; private set; }

        private void Initialise()
        {
            InitialiseLives();
            BuildRelationships();
        }

        private void BuildRelationships()
        {
            for (int rowIndex = 0; rowIndex < MatrixSize; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < MatrixSize; columnIndex++)
                {
                    ConfigureNeighbours(Lives[columnIndex, rowIndex]);
                }
            }
        }

        // TODO: Reduce the size and complexity
        private void ConfigureNeighbours(Life life)
        {
            var currentColumn = life.Position.Column;
            var currentRow = life.Position.Row;
            var columnToTheRight = currentColumn + 1;
            var columnToTheLeft = currentColumn - 1;
            var rowBelow = currentRow + 1;
            var rowAbove = currentRow - 1;
            bool isOnTheTop = currentRow == 0;
            bool isOnTheLeft = currentColumn == 0;
            bool isOnTheRight = currentColumn == MatrixSize - 1;
            bool isOnTheBottom = currentRow == MatrixSize - 1;

            // Start to the right and spiral round based on if it is or isn't possible

            // Right
            if (!isOnTheRight)
            {
                life.AddNeighbours(this.Lives[columnToTheRight, currentRow]);
            }

            // Right Below Diagonal
            if (!isOnTheRight && !isOnTheBottom)
            {
                life.AddNeighbours(this.Lives[columnToTheRight, rowBelow]);
            }

            // Below
            if (!isOnTheBottom)
            {
                life.AddNeighbours(this.Lives[currentColumn, rowBelow]);
            }

            // Left Below Diagonal
            if (!isOnTheLeft && !isOnTheBottom)
            {
                life.AddNeighbours(this.Lives[columnToTheLeft, rowBelow]);
            }

            // Left
            if (!isOnTheLeft)
            {
                life.AddNeighbours(this.Lives[columnToTheLeft, currentRow]);
            }

            // Left Above Diagonal
            if (!isOnTheLeft && !isOnTheTop)
            {
                life.AddNeighbours(this.Lives[columnToTheLeft, rowAbove]);
            }

            // Above
            if (!isOnTheTop)
            {
                life.AddNeighbours(this.Lives[currentColumn, rowAbove]);
            }

            // Right Above Diagonal
            if (!isOnTheTop && !isOnTheRight)
            {
                life.AddNeighbours(this.Lives[columnToTheRight, rowAbove]);
            }
        }

        private void InitialiseLives()
        {
            Lives = new Life[MatrixSize, MatrixSize];
            for (var rowIndex = 0; rowIndex < MatrixSize; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < MatrixSize; columnIndex++)
                {
                    Lives[columnIndex, rowIndex] = new Life(columnIndex, rowIndex);
                }
            }
        }

        //TODO: Create a visual way of testing this
        //public override string ToString()
        //{
        //    StringBuilder result = new StringBuilder();
        //    //  --- --- ---
        //    // | X |   |   |
        //    //  --- --- ---
        //    // |   | X | X |
        //    //  --- --- ---
        //    // |   |   |   |
        //    //  --- --- ---
        //    return result.ToString();
        //}
    }
}
