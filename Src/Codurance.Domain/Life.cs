using System;
using System.Collections.Generic;
using System.Linq;
using static GameOfLife.Domain.ErrorMessages;

namespace GameOfLife.Domain
{
    public class Life
    {
        private readonly List<Life> neighbours = new List<Life>(8);

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
        internal LifeState ExpectedLifeState { get; private set; }
        public IReadOnlyList<Life> Neighbours => neighbours?.AsReadOnly();

        public void BringToLife()
        {
            this.CurrentLifeState = LifeState.Alive;
        }

        public void Kill()
        {
            this.CurrentLifeState = LifeState.Dead;
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
            return this.neighbours.Find(neighbour =>
                neighbour.Position.Column == column && 
                neighbour.Position.Row == row);
        }

        public bool HasNeighbour(int column, int row)
        {
            return this.GetNeighbour(column, row) != null;
        }

        public LifeState CalculateLifeExpectancy()
        {
            // TODO: Validate minimum number of neighbours before continuing
            ExpectedLifeState = CurrentLifeState;
            var aliveStateCount =
                this.neighbours.Count(each => each.CurrentLifeState == LifeState.Alive);
            if (CurrentLifeState == LifeState.Alive && (aliveStateCount < 2 || aliveStateCount > 3))
            {
                ExpectedLifeState = LifeState.Dead;
            }
            else if (CurrentLifeState == LifeState.Dead && aliveStateCount == 3)
            {
                ExpectedLifeState = LifeState.Alive;
            }

            return ExpectedLifeState;
        }
    }
}
