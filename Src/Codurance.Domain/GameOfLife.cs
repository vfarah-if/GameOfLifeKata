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

        public void SeedLife(params Position[] positions)
        {
            foreach (var position in positions)
            {
                Lives[position.Column, position.Row].BringToLife();                
            }
        }

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
                    SetupNeighbours(Lives[columnIndex, rowIndex]);
                }
            }
        }

        private void SetupNeighbours(Life life)
        {          
            AddNeighbourOnTheRight(life);
            AddNeighbourOnTheBelowRightDiagonal(life);
            AddNeighbourBelow(life);
            AddNeighbourBelowLeftDiagnol(life);
            AddNeighbourToTheLeft(life);
            AddNeighbourOnTheAboveDiagonal(life);
            AddNeighbourAbove(life);
            AddNeighbourOnTheAboveRightDiagonal(life);
        }

        private void AddNeighbourOnTheAboveRightDiagonal(Life life)
        {
            var currentColumn = life.Position.Column;
            var currentRow = life.Position.Row;
            var columnToTheRight = currentColumn + 1;
            var rowAbove = currentRow - 1;
            var isOnTheTop = currentRow == 0;
            var isOnTheRight = currentColumn == MatrixSize - 1;
            if (!isOnTheTop && !isOnTheRight)
            {
                life.AddNeighbours(this.Lives[columnToTheRight, rowAbove]);
            }
        }

        private void AddNeighbourAbove(Life life)
        {
            var currentColumn = life.Position.Column;
            var currentRow = life.Position.Row;
            var rowAbove = currentRow - 1;
            var isOnTheTop = currentRow == 0;

            if (!isOnTheTop)
            {
                life.AddNeighbours(this.Lives[currentColumn, rowAbove]);
            }
        }

        private void AddNeighbourOnTheAboveDiagonal(Life life)
        {
            var currentColumn = life.Position.Column;
            var currentRow = life.Position.Row;
            var columnToTheLeft = currentColumn - 1;
            var rowAbove = currentRow - 1;
            var isOnTheTop = currentRow == 0;
            var isOnTheLeft = currentColumn == 0;
            if (!isOnTheLeft && !isOnTheTop)
            {
                life.AddNeighbours(this.Lives[columnToTheLeft, rowAbove]);
            }
        }

        private void AddNeighbourToTheLeft(Life life)
        {
            var currentColumn = life.Position.Column;
            var currentRow = life.Position.Row;
            var columnToTheLeft = currentColumn - 1;
            var isOnTheLeft = currentColumn == 0;
            if (!isOnTheLeft)
            {
                life.AddNeighbours(this.Lives[columnToTheLeft, currentRow]);
            }
        }

        private void AddNeighbourBelowLeftDiagnol(Life life)
        {
            var currentColumn = life.Position.Column;
            var currentRow = life.Position.Row;
            var columnToTheLeft = currentColumn - 1;
            var rowBelow = currentRow + 1;
            var isOnTheLeft = currentColumn == 0;
            var isOnTheBottom = currentRow == MatrixSize - 1;
            if (!isOnTheLeft && !isOnTheBottom)
            {
                life.AddNeighbours(this.Lives[columnToTheLeft, rowBelow]);
            }
        }

        private void AddNeighbourBelow(Life life)
        {
            var currentColumn = life.Position.Column;
            var currentRow = life.Position.Row;
            var rowBelow = currentRow + 1;
            var isOnTheBottom = currentRow == MatrixSize - 1;
            if (!isOnTheBottom)
            {
                life.AddNeighbours(this.Lives[currentColumn, rowBelow]);
            }
        }

        private void AddNeighbourOnTheBelowRightDiagonal(Life life)
        {
            var currentColumn = life.Position.Column;
            var currentRow = life.Position.Row;
            var columnToTheRight = currentColumn + 1;
            var rowBelow = currentRow + 1;
            var isOnTheRight = currentColumn == MatrixSize - 1;
            var isOnTheBottom = currentRow == MatrixSize - 1;

            if (!isOnTheRight && !isOnTheBottom)
            {
                life.AddNeighbours(this.Lives[columnToTheRight, rowBelow]);
            }
        }

        private void AddNeighbourOnTheRight(Life life)
        {
            var currentColumn = life.Position.Column;
            var currentRow = life.Position.Row;
            var columnToTheRight = currentColumn + 1;
            bool isOnTheRight = currentColumn == MatrixSize - 1;

            if (!isOnTheRight)
            {
                life.AddNeighbours(this.Lives[columnToTheRight, currentRow]);
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

        //TODO: Create a visual way of seeing this
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
