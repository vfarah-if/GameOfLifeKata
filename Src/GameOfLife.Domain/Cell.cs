using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static GameOfLife.Domain.ErrorMessages;

namespace GameOfLife.Domain
{
    public class Cell
    {
        private const int MIN_NEIGHBOURCOUNT = 3;
        private readonly List<Cell> neighbours = new List<Cell>(8);
        private LifeState expectedLifeState;

        public Cell(int x, int y, LifeState currentLifeState = LifeState.Dead)
            : this(new Position(x, y), currentLifeState)
        {
        }

        public Cell(Position position, LifeState currentLifeState = LifeState.Dead)
        {
            Position = position;
            CurrentLifeState = currentLifeState;
        }

        public Position Position { get; }
        public LifeState CurrentLifeState { get; private set; }        
        public IReadOnlyList<Cell> Neighbours => neighbours?.AsReadOnly();

        public void BringToLife()
        {
            CurrentLifeState = LifeState.Alive;
        }

        public void Kill()
        {
            CurrentLifeState = LifeState.Dead;
        }

        public void AddNeighbours(params Cell[] cells)
        {
            if (cells.Length == 0)
            {
                throw new ArgumentException(EmptyCollection, nameof(cells));
            }

            if (neighbours.Count + cells.Length > 8)
            {
                throw new NotSupportedException(MaximumSurroundingCellsReached);
            }

            neighbours.AddRange(cells);
        }

        public Cell GetNeighbour(int column, int row)
        {
            return neighbours.Find(neighbour =>
                neighbour.Position.Column == column && 
                neighbour.Position.Row == row);
        }

        public bool HasNeighbour(int column, int row)
        {
            return GetNeighbour(column, row) != null;
        }

        public LifeState CalculateLifeExpectancy()
        {
            if (Neighbours.Count < MIN_NEIGHBOURCOUNT)
            {
                throw new ArgumentOutOfRangeException(nameof(Neighbours), Neighbours, NotEnoughNeighbours);
            }
            expectedLifeState = CurrentLifeState;
            var aliveStateCount =
                neighbours.Count(each => each.CurrentLifeState == LifeState.Alive);
            if (CurrentLifeState == LifeState.Alive && (aliveStateCount < 2 || aliveStateCount > 3))
            {
                expectedLifeState = LifeState.Dead;
            }
            else if (CurrentLifeState == LifeState.Dead && aliveStateCount == 3)
            {
                expectedLifeState = LifeState.Alive;
            }

            Debug.WriteLine($"Calculated Expected Life Status of from '{Enum.GetName(typeof(LifeState), CurrentLifeState)}'" +
                $" to '{Enum.GetName(typeof(LifeState), expectedLifeState)} for {Position.ToString()}'");

            return expectedLifeState;
        }

        public void TransferLifeState()
        {
            CurrentLifeState = expectedLifeState;
        }
    }
}
