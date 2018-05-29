using System;
using System.Diagnostics;
using System.Text;
using static GameOfLife.Domain.ErrorMessages;

namespace GameOfLife.Domain
{
    public class GameOfLife : IGameOfLife
    {
        public const uint MINIMUM_MATRIX_SIZE = 2;
        private event EventHandler CalculateLifeExpectancies;
        private event EventHandler TransferLifeStates;
        public event EventHandler GenerateFinished;

        public GameOfLife(uint matrixSize)
        {
            if (matrixSize < MINIMUM_MATRIX_SIZE)
            {
                throw new ArgumentOutOfRangeException(MatrixSizeOutOfRange);
            }
            MatrixSize = matrixSize;
            Initialise();
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
        public void Generate()
        {
            OnCalculatingLifeExpectancies();
            OnTransferLifeStates();            
#if DEBUG
            Debug.WriteLine(ToString());
#endif
            OnGenerateFinished();
        }

        public override string ToString()
        {
            return PictureIt();
        }

        protected virtual string PictureIt()
        {
            var result = new StringBuilder();
            var currentRow = 0;
            ForeachPosition(position =>
            {
                if (currentRow != position.Row)
                {
                    result.AppendLine();
                    currentRow = position.Row;
                }
                var currentLife = Lives[position.Column, position.Row];
                result.AppendFormat("| [{0}]({1},{2}) |",
                    currentLife.CurrentLifeState == LifeState.Alive ? '+' : '-',
                    position.Column,
                    position.Row);
            });
            return result.ToString();
        }

        private void Initialise()
        {
            InitialiseLives();
            BuildRelationships();
        }

        private void BuildRelationships()
        {
            ForeachPosition(position => SetupNeighbours(Lives[position.Column, position.Row]));
        }

        private void SetupNeighbours(Life life)
        {          
            AddNeighbourOnTheRight(life);
            AddNeighbourOnTheBelowRightDiagonal(life);
            AddNeighbourBelow(life);
            AddNeighbourBelowLeftDiagnol(life);
            AddNeighbourToTheLeft(life);
            AddNeighbourOnTheAboveLeftDiagonal(life);
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
                life.AddNeighbours(Lives[columnToTheRight, rowAbove]);
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
                life.AddNeighbours(Lives[currentColumn, rowAbove]);
            }
        }

        private void AddNeighbourOnTheAboveLeftDiagonal(Life life)
        {
            var currentColumn = life.Position.Column;
            var currentRow = life.Position.Row;
            var columnToTheLeft = currentColumn - 1;
            var rowAbove = currentRow - 1;
            var isOnTheTop = currentRow == 0;
            var isOnTheLeft = currentColumn == 0;
            if (!isOnTheLeft && !isOnTheTop)
            {
                life.AddNeighbours(Lives[columnToTheLeft, rowAbove]);
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
                life.AddNeighbours(Lives[columnToTheLeft, currentRow]);
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
                life.AddNeighbours(Lives[columnToTheLeft, rowBelow]);
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
                life.AddNeighbours(Lives[currentColumn, rowBelow]);
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
                life.AddNeighbours(Lives[columnToTheRight, rowBelow]);
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
                life.AddNeighbours(Lives[columnToTheRight, currentRow]);
            }
        }

        private void InitialiseLives()
        {
            Lives = new Life[MatrixSize, MatrixSize];
            ForeachPosition(position =>
            {
                var newLife = new Life(position);
                Lives[position.Column, position.Row] = newLife;
                CalculateLifeExpectancies += (sender, args) => newLife.CalculateLifeExpectancy();
                TransferLifeStates += (sender, args) => newLife.TransferLifeState();
            });
        }

        private void ForeachPosition(Action<Position> doAction)
        {
            for (var rowIndex = 0; rowIndex < MatrixSize; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < MatrixSize; columnIndex++)
                {
                    doAction?.Invoke(new Position(columnIndex, rowIndex));
                }
            }
        }

        protected virtual void OnCalculatingLifeExpectancies()
        {
            CalculateLifeExpectancies?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnTransferLifeStates()
        {
            TransferLifeStates?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnGenerateFinished()
        {
            GenerateFinished?.Invoke(this, EventArgs.Empty);
        }
    }
}
