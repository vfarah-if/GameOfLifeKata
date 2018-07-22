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

        public GameOfLife(uint matrixSize) :
            this(matrixSize, matrixSize)
        {
        }

        public GameOfLife(uint columnSize, uint rowSize)
        {
            if (columnSize < MINIMUM_MATRIX_SIZE || rowSize < MINIMUM_MATRIX_SIZE)
            {
                throw new ArgumentOutOfRangeException(MatrixSizeOutOfRange);
            }

            this.ColumnSize = columnSize;
            this.RowSize = rowSize;
            Initialise();
        }

        public uint ColumnSize { get; }
        public uint RowSize { get; }
        public Cell[,] Cells { get; private set; }

        public void SeedLife(params Position[] positions)
        {
            foreach (var position in positions)
            {
                Cells[position.Column, position.Row].BringToLife();
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
                var currentLife = Cells[position.Column, position.Row];
                result.AppendFormat("| [{0}]({1},{2}) |",
                    currentLife.CurrentLifeState == LifeState.Alive ? '+' : ' ',
                    SpaceEquallyWithZeros(position.Column, ColumnSize),
                    SpaceEquallyWithZeros(position.Row, RowSize));
            });
            return result.ToString();
        }

        private static string SpaceEquallyWithZeros(int position, uint size)
        {
            return position.ToString().PadLeft(size.ToString().Length, '0');
        }

        private void Initialise()
        {
            InitialiseLives();
            BuildRelationships();
        }

        private void BuildRelationships()
        {
            ForeachPosition(position => SetupNeighbours(Cells[position.Column, position.Row]));
        }

        private void SetupNeighbours(Cell cell)
        {
            AddNeighbourOnTheRight(cell);
            AddNeighbourOnTheBelowRightDiagonal(cell);
            AddNeighbourBelow(cell);
            AddNeighbourBelowLeftDiagnol(cell);
            AddNeighbourToTheLeft(cell);
            AddNeighbourOnTheAboveLeftDiagonal(cell);
            AddNeighbourAbove(cell);
            AddNeighbourOnTheAboveRightDiagonal(cell);
        }

        private void AddNeighbourOnTheAboveRightDiagonal(Cell cell)
        {
            var currentColumn = cell.Position.Column;
            var currentRow = cell.Position.Row;
            var columnToTheRight = currentColumn + 1;
            var rowAbove = currentRow - 1;
            var isOnTheTop = currentRow == 0;
            var isOnTheRight = currentColumn == ColumnSize - 1;
            if (!isOnTheTop && !isOnTheRight)
            {
                cell.AddNeighbours(Cells[columnToTheRight, rowAbove]);
            }
        }

        private void AddNeighbourAbove(Cell cell)
        {
            var currentColumn = cell.Position.Column;
            var currentRow = cell.Position.Row;
            var rowAbove = currentRow - 1;
            var isOnTheTop = currentRow == 0;

            if (!isOnTheTop)
            {
                cell.AddNeighbours(Cells[currentColumn, rowAbove]);
            }
        }

        private void AddNeighbourOnTheAboveLeftDiagonal(Cell cell)
        {
            var currentColumn = cell.Position.Column;
            var currentRow = cell.Position.Row;
            var columnToTheLeft = currentColumn - 1;
            var rowAbove = currentRow - 1;
            var isOnTheTop = currentRow == 0;
            var isOnTheLeft = currentColumn == 0;
            if (!isOnTheLeft && !isOnTheTop)
            {
                cell.AddNeighbours(Cells[columnToTheLeft, rowAbove]);
            }
        }

        private void AddNeighbourToTheLeft(Cell cell)
        {
            var currentColumn = cell.Position.Column;
            var currentRow = cell.Position.Row;
            var columnToTheLeft = currentColumn - 1;
            var isOnTheLeft = currentColumn == 0;
            if (!isOnTheLeft)
            {
                cell.AddNeighbours(Cells[columnToTheLeft, currentRow]);
            }
        }

        private void AddNeighbourBelowLeftDiagnol(Cell cell)
        {
            var currentColumn = cell.Position.Column;
            var currentRow = cell.Position.Row;
            var columnToTheLeft = currentColumn - 1;
            var rowBelow = currentRow + 1;
            var isOnTheLeft = currentColumn == 0;
            var isOnTheBottom = currentRow == RowSize - 1;
            if (!isOnTheLeft && !isOnTheBottom)
            {
                cell.AddNeighbours(Cells[columnToTheLeft, rowBelow]);
            }
        }

        private void AddNeighbourBelow(Cell cell)
        {
            var currentColumn = cell.Position.Column;
            var currentRow = cell.Position.Row;
            var rowBelow = currentRow + 1;
            var isOnTheBottom = currentRow == RowSize - 1;
            if (!isOnTheBottom)
            {
                cell.AddNeighbours(Cells[currentColumn, rowBelow]);
            }
        }

        private void AddNeighbourOnTheBelowRightDiagonal(Cell cell)
        {
            var currentColumn = cell.Position.Column;
            var currentRow = cell.Position.Row;
            var columnToTheRight = currentColumn + 1;
            var rowBelow = currentRow + 1;
            var isOnTheRight = currentColumn == ColumnSize - 1;
            var isOnTheBottom = currentRow == RowSize - 1;

            if (!isOnTheRight && !isOnTheBottom)
            {
                cell.AddNeighbours(Cells[columnToTheRight, rowBelow]);
            }
        }

        private void AddNeighbourOnTheRight(Cell cell)
        {
            var currentColumn = cell.Position.Column;
            var currentRow = cell.Position.Row;
            var columnToTheRight = currentColumn + 1;
            var isOnTheRight = currentColumn == ColumnSize - 1;

            if (!isOnTheRight)
            {
                cell.AddNeighbours(Cells[columnToTheRight, currentRow]);
            }
        }

        private void InitialiseLives()
        {
            Cells = new Cell[ColumnSize, RowSize];
            ForeachPosition(position =>
            {
                var newLife = new Cell(position);
                Cells[position.Column, position.Row] = newLife;
                CalculateLifeExpectancies += (sender, args) => newLife.CalculateLifeExpectancy();
                TransferLifeStates += (sender, args) => newLife.TransferLifeState();
            });
        }

        private void ForeachPosition(Action<Position> doAction)
        {
            for (var rowIndex = 0; rowIndex < RowSize; rowIndex++)
            {
                for (var columnIndex = 0; columnIndex < ColumnSize; columnIndex++)
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
