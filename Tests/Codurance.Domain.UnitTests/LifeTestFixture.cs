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
    public class WhenUsingALifeToPlayGameOfLife
    {
        private Life subject;
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
        public void ShouldSetItsPositionInLife(Position position)
        { 
            subject = new Life(position);

            subject.Position.Should().Be(position);
        }

        [Test, AutoData]
        public void ShouldSetACurrentLifeState(Position position, LifeState currentLifeState)
        {
            subject = new Life(position, currentLifeState);

            subject.CurrentLifeState.Should().Be(currentLifeState);
        }

        [Test, AutoData]
        public void ShouldSetTheDefaultLifeStateToDead(Position position)
        {
            subject = new Life(position);

            subject.CurrentLifeState.Should().Be(LifeState.Dead);
        }

        private Life CreateLife(LifeState lifeState = LifeState.Dead)
        {
            var x = fixture.Create<int>();
            var y = fixture.Create<int>();
            return new Life(x, y, lifeState);
        }

        private IEnumerable<Life> GetThreeAliveOnlyLives()
        {
            yield return CreateLife(LifeState.Alive);
            yield return CreateLife(LifeState.Alive);
            yield return CreateLife(LifeState.Alive);
            yield return CreateLife();
        }


        private IEnumerable<Life> GetTwoAliveOnlyLives()
        {
            yield return CreateLife(LifeState.Alive);
            yield return CreateLife(LifeState.Alive);
            yield return CreateLife();
            yield return CreateLife();
        }

        private IEnumerable<Life> GetMoreThanThreeALiveOnlyLives()
        {
            yield return CreateLife(LifeState.Alive);
            yield return CreateLife(LifeState.Alive);
            yield return CreateLife(LifeState.Alive);
            yield return CreateLife(LifeState.Alive);
            yield return CreateLife();
            yield return CreateLife();
        }

        private IEnumerable<Life> GetOneALiveOnlyLives()
        {
            yield return CreateLife(LifeState.Alive);
            yield return CreateLife();
            yield return CreateLife();
        }

        [TestFixture]
        public class AndAllowingTheCurrentStateToBeSeeded : WhenUsingALifeToPlayGameOfLife
        {

            [SetUp]
            public void SetupPositionAndTest()
            {
                subject = CreateLife(LifeState.Dead);
            }

            [Test]
            public void ShouldMakeLifeAlive()
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
        public class AndConfiguringSurroundingLives : WhenUsingALifeToPlayGameOfLife
        {
            private Life[] surroundingLives;

            [SetUp]
            public void SetupSurroundingLifesAndTest()
            {
                surroundingLives = fixture.CreateMany<Life>().ToArray();
                subject = CreateLife();
            }

            [Test]
            public void ShouldAddNeighbours()
            {
                subject.AddNeighbours(surroundingLives);

                foreach (var surroundingLife in surroundingLives)
                {
                    subject.Neighbours.Should().Contain(surroundingLife);
                }
            }

            [Test]
            public void ShouldThrowNotSupportedExceptionStatingSupportForMaximumEightSurroundingLives()
            {
                var nineSurroundingLives = new Life[] {
                        CreateLife(), CreateLife(), CreateLife() ,
                        CreateLife(), CreateLife(), CreateLife() ,
                        CreateLife(), CreateLife(), CreateLife() };

                Action action = () => subject.AddNeighbours(nineSurroundingLives);

                action.Should().Throw<NotSupportedException>().WithMessage("Maximum eight surrounding lives.");
            }
        }

        [TestFixture]
        public class AndCalculatingTheLifeExpectancyOfALivingLife : WhenUsingALifeToPlayGameOfLife
        {
            [SetUp]
            public void SetupLifeWithAliveState()
            {
                subject = CreateLife(LifeState.Alive);
            }

            [Test]
            public void ShouldThrowAnArgumentOutOfRangeExceptionWhenOnlyOneNeighbourIsAdded()
            {
                subject.AddNeighbours(CreateLife());

                Action action = () => subject.CalculateLifeExpectancy();

                action.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Test]
            public void ShouldThrowAnArgumentOutOfRangeExceptionWhenOnlyTwoNeighbourIsAdded()
            {
                subject.AddNeighbours(CreateLife(), CreateLife());

                Action action = () => subject.CalculateLifeExpectancy();

                action.Should().Throw<ArgumentOutOfRangeException>();
            }

            [Test]
            public void ShouldCalculateStateAsDeadWhenNeighbourUnderPopulationOccurs()
            {
                Life[] neighbours = GetOneALiveOnlyLives().ToArray();
                subject.AddNeighbours(neighbours);

                var actual = subject.CalculateLifeExpectancy();

                actual.Should().Be(LifeState.Dead);

            }

            [Test]
            public void ShouldCalculateStateAsDeadWhenNeighbourOverPopulationOccurs()
            {
                Life[] neighbours = GetMoreThanThreeALiveOnlyLives().ToArray();
                subject.AddNeighbours(neighbours);

                var actual = subject.CalculateLifeExpectancy();

                actual.Should().Be(LifeState.Dead);
            }

            [Test]
            public void ShouldContinueToTheNextGenerationWhenThereAreTwoAliveNeighbours()
            {
                Life[] neighbours = GetTwoAliveOnlyLives().ToArray();
                subject.AddNeighbours(neighbours);

                var actual = subject.CalculateLifeExpectancy();

                actual.Should().Be(LifeState.Alive);
            }

            [Test]
            public void ShouldContinueToTheNextGenerationWhenThereAreThreeAliveNeighbours()
            {
                Life[] neighbours = GetThreeAliveOnlyLives().ToArray();
                subject.AddNeighbours(neighbours);

                var actual = subject.CalculateLifeExpectancy();

                actual.Should().Be(LifeState.Alive);
            }
        }

        [TestFixture]
        public class AndCalculatingTheLifeExpectancyOfADeadLife : WhenUsingALifeToPlayGameOfLife
        {
            [SetUp]
            public void SetupLifeWithADeadStatus()
            {
                subject = new Life(fixture.Create<Position>(), LifeState.Dead);
            }

            [Test]
            public void ShouldReproduceALifeWhenThereAreThreeAliveSurroundingLives()
            {
                Life[] neighbours = GetThreeAliveOnlyLives().ToArray();
                subject.AddNeighbours(neighbours);

                subject.CalculateLifeExpectancy();
                subject.TransferLifeState();

                subject.CurrentLifeState.Should().Be(LifeState.Alive);
            }
        }
    }
}
