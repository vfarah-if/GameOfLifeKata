using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;

namespace GameOfLife.Domain.UnitTests
{
    // Scenarios
    // Should validate any constructor values(Generic)
    // (X) Should set the position in life (Generic)
    // (X) Should pass in a default life state as Dead (Generic)
    // (X) Should have a current life state (Specific)
    // (X) Should have neighbours to help with life (Specific)
    // Should have a projected life state based on : (Specific)
    //      (X) 1. Any live cell with *fewer than two live neighbours dies*, as if caused by **underpopulation**.
    //      (X) 2. Any live cell with *two or three live neighbours lives* on to the** next generation**.
    //      (X) 3. Any live cell with *more than three live neighbours dies*, as if by** overpopulation**.
    //      (X) 4. Any dead cell with *exactly three live neighbours becomes a live cell*, as if by** reproduction**.          

    [TestFixture]
    public class WhenUsingALifeToPlayGameOfLife
    {
        private Life subject;
        private IFixture fixture;

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            fixture = new Fixture().Customize(new AutoMoqCustomization());
        }

        [TearDown]
        public void TearDown()
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

        private IEnumerable<Life> GetThreeAliveOnlyLives()
        {
            yield return new Life(fixture.Create<Position>(), LifeState.Alive);
            yield return new Life(fixture.Create<Position>(), LifeState.Alive);
            yield return new Life(fixture.Create<Position>(), LifeState.Alive);
            yield return new Life(fixture.Create<Position>());
        }

        [TestFixture]
        public class AndAllowingTheCurrentStateToBeSeeded : WhenUsingALifeToPlayGameOfLife
        {

            [SetUp]
            public void SetupPositionAndTest()
            {
                var position = fixture.Create<Position>();
                subject = new Life(position);
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
                subject = fixture.Create<Life>();
            }

            [Test]
            public void ShouldAddSurroundingLives()
            {
                subject.AddSurroundingLives(surroundingLives);

                foreach (var surroundingLife in surroundingLives)
                {
                    subject.SurroundingLives.Should().Contain(surroundingLife);
                }
            }

            [Test]
            public void ShouldThrowNotSupportedExceptionStatingSupportForMaximumEightSurroundingLives()
            {
                var nineSurroundingLives = new Life[] {
                    fixture.Create<Life>(), fixture.Create<Life>(), fixture.Create<Life>() ,
                    fixture.Create<Life>(), fixture.Create<Life>() , fixture.Create<Life>(),
                    fixture.Create<Life>(), fixture.Create<Life>() , fixture.Create<Life>() };

                Action action = () => subject.AddSurroundingLives(nineSurroundingLives);

                action.Should().Throw<NotSupportedException>().WithMessage("Maximum eight surrounding lives");
            }
        }

        [TestFixture]
        public class AndCalculatingTheLifeExpectancyOfALivingLife : WhenUsingALifeToPlayGameOfLife
        {
            [SetUp]
            public void SetupLifeWithAliveStatus()
            {
                subject = new Life(fixture.Create<Position>(), LifeState.Alive);
            }

            // Any live cell with *fewer than two live neighbours dies*, as if caused by **underpopulation**.
            [Test]
            public void ShouldCalculateStateAsDeadWhenUnderPopulationOccurs()
            {
                Life[] surroundLives = GetOneALiveOnlyLives().ToArray();
                subject.AddSurroundingLives(surroundLives);

                var actual = subject.CalculateLifeExpectency();

                actual.Should().Be(LifeState.Dead);

            }

            // Any live cell with *more than three live neighbours dies*, as if by** overpopulation**.
            [Test]
            public void ShouldCalculateStateAsDeadWhenOverPopulationOccurs()
            {
                Life[] surroundLives = GetMoreThanThreeALiveOnlyLives().ToArray();
                subject.AddSurroundingLives(surroundLives);

                var actual = subject.CalculateLifeExpectency();

                actual.Should().Be(LifeState.Dead);
            }

            //  Any live cell with *two or three live neighbours lives* on to the** next generation**.
            [Test]
            public void ShouldContinueToTheNextGenerationWhenThereAreTwoAliveSurroundingLives()
            {
                Life[] surroundLives = GetTwoAliveOnlyLives().ToArray();
                subject.AddSurroundingLives(surroundLives);

                var actual = subject.CalculateLifeExpectency();

                actual.Should().Be(LifeState.Alive);
            }

            [Test]
            public void ShouldContinueToTheNextGenerationWhenThereAreThreeAliveSurroundingLives()
            {
                Life[] surroundLives = GetThreeAliveOnlyLives().ToArray();
                subject.AddSurroundingLives(surroundLives);

                var actual = subject.CalculateLifeExpectency();

                actual.Should().Be(LifeState.Alive);
            }

            private IEnumerable<Life> GetTwoAliveOnlyLives()
            {
                yield return new Life(fixture.Create<Position>(), LifeState.Alive);
                yield return new Life(fixture.Create<Position>(), LifeState.Alive);
                yield return new Life(fixture.Create<Position>());
                yield return new Life(fixture.Create<Position>());
            }

            private IEnumerable<Life> GetMoreThanThreeALiveOnlyLives()
            {
                yield return new Life(fixture.Create<Position>(), LifeState.Alive);
                yield return new Life(fixture.Create<Position>(), LifeState.Alive);
                yield return new Life(fixture.Create<Position>(), LifeState.Alive);
                yield return new Life(fixture.Create<Position>(), LifeState.Alive);
                yield return new Life(fixture.Create<Position>());
                yield return new Life(fixture.Create<Position>());
            }

            private IEnumerable<Life> GetOneALiveOnlyLives()
            {
                yield return new Life(fixture.Create<Position>(), LifeState.Alive);
                yield return new Life(fixture.Create<Position>());
                yield return new Life(fixture.Create<Position>());
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

            // Any dead cell with *exactly three live neighbours becomes a live cell*, as if by** reproduction**. 
            [Test]
            public void ShouldReproduceALifeWhenThereAreThreeAliveSurroundingLives()
            {
                Life[] surroundLives = GetThreeAliveOnlyLives().ToArray();
                subject.AddSurroundingLives(surroundLives);

                var actual = subject.CalculateLifeExpectency();

                actual.Should().Be(LifeState.Alive);
            }
        }
    }
  }
