using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLife.Domain
{
    public class Life
    {
        private readonly List<Life> surroundingLives = new List<Life>(8);

        public Life(Position position, LifeState currentLifeState = LifeState.Dead)
        {
            Position = position;
            CurrentLifeState = currentLifeState;
        }

        public Position Position { get; }
        public LifeState CurrentLifeState { get; private set; }
        internal LifeState ExpectedLifeState { get; private set; }
        public IReadOnlyList<Life> SurroundingLives => surroundingLives?.AsReadOnly();

        public void BringToLife()
        {
            this.CurrentLifeState = LifeState.Alive;
        }

        public void Kill()
        {
            this.CurrentLifeState = LifeState.Dead;
        }

        public void AddSurroundingLives(params Life[] lives)
        {
            if (lives.Length == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(lives));
            }

            if (surroundingLives.Count + lives.Length > 8)
            {
                throw new NotSupportedException("Maximum eight surrounding lives");
            }

            surroundingLives.AddRange(lives);
        }

        public LifeState CalculateLifeExpectency()
        {
            ExpectedLifeState = CurrentLifeState;
            int aliveStateCount =
                this.surroundingLives.Count(each => each.CurrentLifeState == LifeState.Alive);
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
