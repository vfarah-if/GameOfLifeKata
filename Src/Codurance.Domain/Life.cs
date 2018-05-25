using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static GameOfLife.Domain.ErrorMessages;

namespace GameOfLife.Domain
{
    public class Life
    {
        private const int MIN_NEIGHBOURCOUNT = 3;
        private readonly List<Life> neighbours = new List<Life>(8);
        private LifeState expectedLifeState;

        public Life(int x, int y, LifeState currentLifeState = LifeState.Dead)
            : this(new Position(x, y), currentLifeState)
        {
        }

        public Life(Position position, LifeState currentLifeState = LifeState.Dead)
        {
            Position = position;
            CurrentLifeState = currentLifeState;
        }

        public Position Position { get; }
        public LifeState CurrentLifeState { get; private set; }        
        public IReadOnlyList<Life> Neighbours => neighbours?.AsReadOnly();

        public void BringToLife()
        {
            CurrentLifeState = LifeState.Alive;
        }

        public void Kill()
        {
            CurrentLifeState = LifeState.Dead;
        }

        public void AddNeighbours(params Life[] lives)
        {
            if (lives.Length == 0)
            {
                throw new ArgumentException(EmptyCollection, nameof(lives));
            }

            if (neighbours.Count + lives.Length > 8)
            {
                throw new NotSupportedException(MaximumSurroundingLivesReached);
            }

            neighbours.AddRange(lives);
        }

        public Life GetNeighbour(int column, int row)
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
