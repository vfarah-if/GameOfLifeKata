using System;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;

namespace GameOfLife.Domain.UnitTests
{   
    [TestFixture]
    public class WhenUsingACellToPlayGameOfLife
    {
        private Cell subject;
        private IFixture fixture;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            fixture = new Fixture();
        }

        [OneTimeTearDown]
        public void OneTimeTearDown()
        {
            fixture = null;
        }

        [Test, AutoData]
        public void ShouldSetItsPositionInCell(Position position)
        { 
            subject = new Cell(position);

            subject.Position.Should().Be(position);
        }

        [Test, AutoData]
        public void ShouldSetACurrentLifeState(Position position, LifeState currentLifeState)
        {
            subject = new Cell(position, currentLifeState);

            subject.CurrentLifeState.Should().Be(currentLifeState);
        }

        [Test, AutoData]
        public void ShouldSetTheDefaultLifeStateToDead(Position position)
        {
            subject = new Cell(position);

            subject.CurrentLifeState.Should().Be(LifeState.Dead);
        }

        private Cell CreateCell(LifeState lifeState = LifeState.Dead)
        {
            var x = fixture.Create<int>();
            var y = fixture.Create<int>();
            return new Cell(x, y, lifeState);
        }

        private IEnumerable<Cell> GetThreeAliveOnlyCells()
        {
            yield return CreateCell(LifeState.Alive);
            yield return CreateCell(LifeState.Alive);
            yield return CreateCell(LifeState.Alive);
            yield return CreateCell();
        }

        [TestFixture]
        public class AndAllowingTheCurrentStateToBeSeeded : WhenUsingACellToPlayGameOfLife
        {

            [SetUp]
            public void SetupPositionAndTest()
            {
                subject = CreateCell();
            }

            [Test]
            public void ShouldMakeCellAlive()
            {
                subject.BringToLife();

                subject.CurrentLifeState.Should().Be(LifeState.Alive);
            }

            [Test]
            public void ShouldKillLife()
            {
                subject.Kill();

                subject.CurrentLifeState.Should().Be(LifeState.Dead);
            }
        }

        [TestFixture]
        public class AndConfiguringNeighboursToPlayGameOfLife : WhenUsingACellToPlayGameOfLife
        {
            private Cell[] neighbours;

            [SetUp]
            public void SetupSurroundingLifesAndTest()
            {
                neighbours = fixture.CreateMany<Cell>().ToArray();
                subject = CreateCell();
            }

            [Test]
            public void ShouldAddNeighbours()
            {
                subject.AddNeighbours(neighbours);

                foreach (var surroundingLife in neighbours)
                {
                    subject.Neighbours.Should().Contain(surroundingLife);
                }
            }

            [Test]
            public void ShouldThrowNotSupportedExceptionStatingSupportForMaximumEightSurroundingCells()
            {
                neighbours = GetNineCells().ToArray();

                Action action = () => subject.AddNeighbours(neighbours);

                action.Should().Throw<NotSupportedException>().WithMessage("Maximum of eight neighbour cells.");
            }

            private IEnumerable<Cell> GetNineCells()
            {
                yield return CreateCell();
                yield return CreateCell();
                yield return CreateCell();

                yield return CreateCell();
                yield return CreateCell();
                yield return CreateCell();

                yield return CreateCell();
                yield return CreateCell();
                yield return CreateCell();
            }
        }

        [TestFixture]
        public class AndCalculatingTheCellToPlayGameOfLife : WhenUsingACellToPlayGameOfLife
        {
            [SetUp]
            public void SetupLifeWithAliveState()
            {
                subject = CreateCell(LifeState.Alive);
            }

            [Test]
            public void ShouldThrowAnArgumentOutOfRangeExceptionWhenOnlyOneNeighbourIsAdded()
            {
                subject.AddNeighbours(CreateCell());

                Action action = () => subject.CalculateLifeExpectancy();

                action.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Test]
            public void ShouldThrowAnArgumentOutOfRangeExceptionWhenOnlyTwoNeighboursIsAdded()
            {
                subject.AddNeighbours(CreateCell(), CreateCell());

                Action action = () => subject.CalculateLifeExpectancy();

                action.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Test]
            public void ShouldCalculateStateAsDeadWhenNeighbourUnderPopulationOccurs()
            {
                Cell[] neighbours = GetOneALiveOnlyCells().ToArray();
                subject.AddNeighbours(neighbours);

                var actual = subject.CalculateLifeExpectancy();

                actual.Should().Be(LifeState.Dead);
            }

            [Test]
            public void ShouldCalculateStateAsDeadWhenNeighbourOverPopulationOccurs()
            {
                Cell[] neighbours = GetMoreThanThreeALiveCells().ToArray();
                subject.AddNeighbours(neighbours);

                var actual = subject.CalculateLifeExpectancy();

                actual.Should().Be(LifeState.Dead);
            }

            [Test]
            public void ShouldContinueToTheNextGenerationWhenThereAreTwoAliveNeighbours()
            {
                Cell[] neighbours = GetTwoAliveOnlyCells().ToArray();
                subject.AddNeighbours(neighbours);

                var actual = subject.CalculateLifeExpectancy();

                actual.Should().Be(LifeState.Alive);
            }

            [Test]
            public void ShouldContinueToTheNextGenerationWhenThereAreThreeAliveNeighbours()
            {
                Cell[] neighbours = GetThreeAliveOnlyCells().ToArray();
                subject.AddNeighbours(neighbours);

                var actual = subject.CalculateLifeExpectancy();

                actual.Should().Be(LifeState.Alive);
            }

            private IEnumerable<Cell> GetOneALiveOnlyCells()
            {
                yield return CreateCell(LifeState.Alive);
                yield return CreateCell();
                yield return CreateCell();
            }

            private IEnumerable<Cell> GetTwoAliveOnlyCells()
            {
                yield return CreateCell(LifeState.Alive);
                yield return CreateCell(LifeState.Alive);
                yield return CreateCell();
                yield return CreateCell();
            }

            private IEnumerable<Cell> GetMoreThanThreeALiveCells()
            {
                yield return CreateCell(LifeState.Alive);
                yield return CreateCell(LifeState.Alive);
                yield return CreateCell(LifeState.Alive);
                yield return CreateCell(LifeState.Alive);
                yield return CreateCell();
                yield return CreateCell();
            }
        }

        [TestFixture]
        public class AndCalculatingTheCellToPlayGameOfLifeExpectancyOfACellToPlayGameOfDeadLife : WhenUsingACellToPlayGameOfLife
        {
            [SetUp]
            public void SetupLifeWithTheDefaultDeadStatus()
            {
                subject = new Cell(fixture.Create<Position>());
            }

            [Test]
            public void ShouldReproduceALifeWhenThereAreThreeAliveNeighbours()
            {
                Cell[] neighbours = GetThreeAliveOnlyCells().ToArray();
                subject.AddNeighbours(neighbours);

                subject.CalculateLifeExpectancy();
                subject.TransferLifeState();

                subject.CurrentLifeState.Should().Be(LifeState.Alive);
            }
        }
    }
}
