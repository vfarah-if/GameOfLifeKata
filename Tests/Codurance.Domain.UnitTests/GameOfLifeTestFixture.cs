using System;
using NUnit.Framework;
using AutoFixture;
using FluentAssertions;
using static System.Environment;


namespace GameOfLife.Domain.UnitTests
{
    // Scenarios
    // (X) The "game" is a zero-player game, meaning that its evolution is determined by its initial state, requiring no further input.One interacts with the Game of Life by creating an initial configuration and observing how it evolves, or, for advanced "players", by creating patterns with particular properties. This is a simplified version allowing 3 distinct seeds, a diamond shape, a square, and a cross shape.
    // (X) The initial pattern constitutes the** seed** of the system.The first generation is created by applying the above rules simultaneously to every cell in the seed—births and deaths occur simultaneously, and the discrete moment at which this happens is sometimes called a** tick** (in other words, each generation is a pure function of the preceding one). The rules continue to be applied repeatedly to create further generations.
    // (X) *Conway chose his rules carefully*, after considerable experimentation, to meet these criteria:
    // (X) 1. There should be no **explosive growth**.
    // (X) 2. There should exist** small initial patterns** with chaotic, unpredictable outcomes.
    // (X) 3. There should be potential for von Neumann universal constructors.

    // (X) Should create a game with a Size of the grid defining how many lives can interact with this grid or a matrix by the size of the matrix
    // (X) Should initialise the lives to dead 
    // (X) Should build all the relationships based on the position of the matrix
    // (X) Should have a seeding method that can generate some chaotic data
    // (X) Should have a method that calculate the state of each life based on this data
    // (X) Should have a method that will change the state all at the same time once the state has changed
    // (X) Should be able to repeat this several times using seeds with expected outputs on each round
    // (X) Once everything working simply, run more efficiently in parallel
    // Add more seeding examples from Wiki increasing the size of the grid and the complexity
    // Should Have an onFinished event or action to work with after it is all done

    [TestFixture]
    public class WhenPlayingGameOfLife
    {
        private IFixture fixture;
        private GameOfLife subject;
        private uint matrixSize;

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

        [TestFixture]
        public class AndSettingTheSizeOfTheLivesMatrix : WhenPlayingGameOfLife
        {
            [SetUp]
            public void SetupTheGameAndMatrix()
            {
                matrixSize = GameOfLife.MINIMUM_MATRIX_SIZE + fixture.Create<uint>();
                subject = new GameOfLife(matrixSize);
            }

            [Test]
            public void ShouldSetTheMatrixByTheConstranedSize()
            {
                subject.MatrixSize.Should().Be(matrixSize);
            }

            [TestCase((uint) 1)]
            [TestCase((uint) 0)]
            public void ShouldThrowArgumentOutOfRangeException(uint badMatrixSize)
            {
                Action action = () => new GameOfLife(badMatrixSize);

                action.Should().Throw<ArgumentOutOfRangeException>()
                    .WithMessage("Specified argument was out of the range of valid values."
                                 + NewLine +
                                 "Parameter name: Life can not thrive in such a small eco system.");
            }


            [Test]
            public void ShouldBuildAMatrixFilledWithDeadLife()
            {
                int expectedMatrixLength = Convert.ToInt32(matrixSize * matrixSize);

                subject.Lives.Length.Should().Be(expectedMatrixLength);
                for (var rowIndex = 0; rowIndex < matrixSize; rowIndex++)
                {
                    for (var colIndex = 0; colIndex < matrixSize; colIndex++)
                    {
                        subject.Lives[colIndex, rowIndex].Should().BeOfType<Life>();
                        subject.Lives[colIndex, rowIndex].CurrentLifeState.Should().Be(LifeState.Dead);
                    }
                }
            }
        }

        [TestFixture]
        public class AndBuildingTheRelationshipsWithinEachLife : WhenPlayingGameOfLife
        {
            [SetUp]
            public void SetupTheGameAndMatrix()
            {
                matrixSize = 3;
                subject = new GameOfLife(matrixSize);
            }
        }

        [TestFixture]
        public class AndValidatingTheTopRow : AndBuildingTheRelationshipsWithinEachLife
        {

            [Test]
            public void ShouldBuildTheTopLeftCornerWithThreeNeighbours()
            {
                int expectedCount = 3;
                var lifeUnderTest = subject.Lives[0, 0];

                lifeUnderTest.Neighbours.Count.Should().Be(expectedCount);
                var hasRightPosition = lifeUnderTest.HasNeighbour(1, 0);
                var hasDiagonalPosition = lifeUnderTest.HasNeighbour(1, 1);
                var hasBottomPosition = lifeUnderTest.HasNeighbour(0, 1);

                hasRightPosition.Should().BeTrue();
                hasDiagonalPosition.Should().BeTrue();
                hasBottomPosition.Should().BeTrue();
            }

            [Test]
            public void ShouldBuildTheTopMiddleWithFiveNeighbours()
            {
                int expectedCount = 5;
                var lifeUnderTest = subject.Lives[1, 0];

                lifeUnderTest.Neighbours.Count.Should().Be(expectedCount);
                var hasRightPosition = lifeUnderTest.HasNeighbour(2, 0);
                var hasRightDiagonalPosition = lifeUnderTest.HasNeighbour(2, 1);
                var hasBottomPosition = lifeUnderTest.HasNeighbour(1, 1);
                var hasLeftDiagonalPosition = lifeUnderTest.HasNeighbour(0, 1);
                var hasLeftPosition = lifeUnderTest.HasNeighbour(0, 0);

                hasRightPosition.Should().BeTrue();
                hasRightDiagonalPosition.Should().BeTrue();
                hasBottomPosition.Should().BeTrue();
                hasLeftDiagonalPosition.Should().BeTrue();
                hasLeftPosition.Should().BeTrue();
            }

            [Test]
            public void ShouldBuildTheTopRightCornerWithThreeNeighbours()
            {
                int expectedCount = 3;
                var lifeUnderTest = subject.Lives[2, 0];

                lifeUnderTest.Neighbours.Count.Should().Be(expectedCount);
                var hasLeftPosition = lifeUnderTest.HasNeighbour(1, 0);
                var hasDiagonalPosition = lifeUnderTest.HasNeighbour(1, 1);
                var hasBottomPosition = lifeUnderTest.HasNeighbour(2, 1);

                hasLeftPosition.Should().BeTrue();
                hasDiagonalPosition.Should().BeTrue();
                hasBottomPosition.Should().BeTrue();
            }
        }

        [TestFixture]
        public class AndValidatingTheLeftRow : AndBuildingTheRelationshipsWithinEachLife
        {
            [Test]
            public void ShouldBuildTheLeftMiddleWithFiveNeighbours()
            {
                int expectedCount = 5;
                var lifeUnderTest = subject.Lives[0, 1];

                lifeUnderTest.Neighbours.Count.Should().Be(expectedCount);
                var hasRightPosition = lifeUnderTest.HasNeighbour(1, 1);
                var hasBottomRightDiagonalPosition = lifeUnderTest.HasNeighbour(1, 2);
                var hasBottomPosition = lifeUnderTest.HasNeighbour(0, 2);
                var hasTopPosition = lifeUnderTest.HasNeighbour(0, 0);
                var hasTopDiagnolPosition = lifeUnderTest.HasNeighbour(1, 0);

                hasRightPosition.Should().BeTrue();
                hasBottomRightDiagonalPosition.Should().BeTrue();
                hasBottomPosition.Should().BeTrue();
                hasTopPosition.Should().BeTrue();
                hasTopDiagnolPosition.Should().BeTrue();
            }

            [Test]
            public void ShouldBuildTheBottomLeftCornerWithThreeNeighbours()
            {
                int expectedCount = 3;
                var lifeUnderTest = subject.Lives[0, 2];

                lifeUnderTest.Neighbours.Count.Should().Be(expectedCount);
                var hasRightPosition = lifeUnderTest.HasNeighbour(1, 2);
                var hasTopPosition = lifeUnderTest.HasNeighbour(0, 1);
                var hasTopDiagonalPosition = lifeUnderTest.HasNeighbour(1, 1);

                hasRightPosition.Should().BeTrue();
                hasTopPosition.Should().BeTrue();
                hasTopDiagonalPosition.Should().BeTrue();
            }
        }

        [TestFixture]
        public class AndValidatingTheBottomRow : AndBuildingTheRelationshipsWithinEachLife
        {
            [Test]
            public void ShouldBuildTheLeftMiddleWithFiveNeighbours()
            {
                int expectedCount = 5;
                var lifeUnderTest = subject.Lives[1, 2];

                lifeUnderTest.Neighbours.Count.Should().Be(expectedCount);
                var hasRightPosition = lifeUnderTest.HasNeighbour(2, 2);
                var hasLeftPosition = lifeUnderTest.HasNeighbour(0, 2);
                var hasTopLeftDiagonalPosition = lifeUnderTest.HasNeighbour(0, 1);
                var hasTopPosition = lifeUnderTest.HasNeighbour(1, 1);
                var hasTopRightDiagnolPosition = lifeUnderTest.HasNeighbour(2, 1);

                hasRightPosition.Should().BeTrue();
                hasLeftPosition.Should().BeTrue();
                hasTopLeftDiagonalPosition.Should().BeTrue();
                hasTopPosition.Should().BeTrue();
                hasTopRightDiagnolPosition.Should().BeTrue();
            }

            [Test]
            public void ShouldBuildTheBottomRightCornerWithThreeNeighbours()
            {
                int expectedCount = 3;
                var lifeUnderTest = subject.Lives[2, 2];

                lifeUnderTest.Neighbours.Count.Should().Be(expectedCount);
                var hasLeftPosition = lifeUnderTest.HasNeighbour(1, 2);
                var hasLeftTopDiagonalPosition = lifeUnderTest.HasNeighbour(1, 1);
                var hasTopPosition = lifeUnderTest.HasNeighbour(2, 1);

                hasLeftPosition.Should().BeTrue();
                hasLeftTopDiagonalPosition.Should().BeTrue();
                hasTopPosition.Should().BeTrue();
            }
        }

        [TestFixture]
        public class AndValidatingTheRightRow : AndBuildingTheRelationshipsWithinEachLife
        {
            [Test]
            public void ShouldBuildTheRightMiddleWithFiveNeighbours()
            {
                int expectedCount = 5;
                var lifeUnderTest = subject.Lives[2, 1];

                lifeUnderTest.Neighbours.Count.Should().Be(expectedCount);
                var hasBottomPosition = lifeUnderTest.HasNeighbour(2, 2);
                var hasLeftBottomDiagonalPosition = lifeUnderTest.HasNeighbour(1, 2);
                var hasLeftPosition = lifeUnderTest.HasNeighbour(1, 1);
                var hasTopLeftDiagonalPosition = lifeUnderTest.HasNeighbour(1, 0);
                var hasTopPosition = lifeUnderTest.HasNeighbour(2, 0);

                hasBottomPosition.Should().BeTrue();
                hasLeftBottomDiagonalPosition.Should().BeTrue();
                hasLeftPosition.Should().BeTrue();
                hasTopLeftDiagonalPosition.Should().BeTrue();
                hasTopPosition.Should().BeTrue();
            }
        }

        [TestFixture]
        public class AndValidatingTheMiddle : AndBuildingTheRelationshipsWithinEachLife
        {
            [Test]
            public void ShouldBuildTheMiddleWithEightNeighbours()
            {
                int expectedCount = 8;
                var lifeUnderTest = subject.Lives[1, 1];

                lifeUnderTest.Neighbours.Count.Should().Be(expectedCount);
                var hasRightPosition = lifeUnderTest.HasNeighbour(2, 1);
                var hasRightBottomDiagonalPosition = lifeUnderTest.HasNeighbour(2, 2);
                var hasBottomPosition = lifeUnderTest.HasNeighbour(1, 2);
                var hasBottomLeftDiagonalPosition = lifeUnderTest.HasNeighbour(0, 2);
                var hasLeftPosition = lifeUnderTest.HasNeighbour(0, 1);
                var hasLeftTopDiagonalPosition = lifeUnderTest.HasNeighbour(0, 0);
                var hasTopPosition = lifeUnderTest.HasNeighbour(1, 0);
                var hasTopRightDiagonalPosition = lifeUnderTest.HasNeighbour(2, 0);

                hasRightPosition.Should().BeTrue();
                hasRightBottomDiagonalPosition.Should().BeTrue();
                hasBottomPosition.Should().BeTrue();
                hasBottomLeftDiagonalPosition.Should().BeTrue();
                hasLeftPosition.Should().BeTrue();
                hasLeftTopDiagonalPosition.Should().BeTrue();
                hasTopPosition.Should().BeTrue();
                hasTopRightDiagonalPosition.Should().BeTrue();
            }
        }

        [TestFixture]
        public class AndPlayingWithA5By5Matrix : WhenPlayingGameOfLife
        {
            [SetUp]
            public void SetupTheGameWithAFiveByFiveLifeMatrixAndBlinkerSeed()
            {
                matrixSize = 5;
                subject = new GameOfLife(matrixSize);
                ShouldHaveDefaultDeadEverything();
            }

            private void ShouldHaveDefaultDeadEverything()
            {
                var expectedGridLayout =
                    "| [-](0,0) || [-](1,0) || [-](2,0) || [-](3,0) || [-](4,0) |" + NewLine +
                    "| [-](0,1) || [-](1,1) || [-](2,1) || [-](3,1) || [-](4,1) |" + NewLine +
                    "| [-](0,2) || [-](1,2) || [-](2,2) || [-](3,2) || [-](4,2) |" + NewLine +
                    "| [-](0,3) || [-](1,3) || [-](2,3) || [-](3,3) || [-](4,3) |" + NewLine +
                    "| [-](0,4) || [-](1,4) || [-](2,4) || [-](3,4) || [-](4,4) |" + NewLine;

                subject.Lives[1, 2].CurrentLifeState.Should().Be(LifeState.Dead);
                subject.Lives[2, 2].CurrentLifeState.Should().Be(LifeState.Dead);
                subject.Lives[3, 2].CurrentLifeState.Should().Be(LifeState.Dead);
                subject.ToString().Should().Be(expectedGridLayout);
            }
        }

        [TestFixture]
        public class AndSeedingDataByConfiguringTheBlinkerOscilattorPattern : AndPlayingWithA5By5Matrix
        {
            [SetUp]
            public void SetupBlinkerOscilattorData()
            {
                subject.SeedLife(new Position(1, 2), new Position(2, 2), new Position(3, 2));
            }

            [Test]
            public void ShouldSeedLifeByBlinkerPositions()
            {
                ShouldHaveAnAliveHorizontalBlink();
            }

            [Test]
            public void ShouldThrowIndexOutOfRangeExceptionForPositionsThatDoNotExist()
            {
                Action action = () => subject.SeedLife(new Position(5, 5));

                action.Should()
                    .Throw<IndexOutOfRangeException>()
                    .WithMessage("Index was outside the bounds of the array.");
            }

            [Test]
            public void ShouldCalculateLifeExpectancyBasedOnOneOscilationInANonChaoticWay()
            {
                subject.Generate();

                ShouldHaveAnAliveVerticalBlink();
            }

            [Test]
            public void ShouldCalculateLifeExpectancyBasedOnThreeOscilationsInANonChaoticWay()
            {
                subject.Generate();
                ShouldHaveAnAliveVerticalBlink();

                subject.Generate();

                ShouldHaveAnAliveHorizontalBlink();

                subject.Generate();

                ShouldHaveAnAliveVerticalBlink();
            }

            private void ShouldHaveAnAliveVerticalBlink()
            {
                var expectedGridLayout =
                    "| [-](0,0) || [-](1,0) || [-](2,0) || [-](3,0) || [-](4,0) |" + NewLine +
                    "| [-](0,1) || [-](1,1) || [+](2,1) || [-](3,1) || [-](4,1) |" + NewLine +
                    "| [-](0,2) || [-](1,2) || [+](2,2) || [-](3,2) || [-](4,2) |" + NewLine +
                    "| [-](0,3) || [-](1,3) || [+](2,3) || [-](3,3) || [-](4,3) |" + NewLine +
                    "| [-](0,4) || [-](1,4) || [-](2,4) || [-](3,4) || [-](4,4) |" + NewLine;
                subject.Lives[2, 1].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.Lives[2, 2].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.Lives[2, 3].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.ToString().Should().Be(expectedGridLayout);
            }


            private void ShouldHaveAnAliveHorizontalBlink()
            {
                var expectedGridLayout =
                    "| [-](0,0) || [-](1,0) || [-](2,0) || [-](3,0) || [-](4,0) |" + NewLine +
                    "| [-](0,1) || [-](1,1) || [-](2,1) || [-](3,1) || [-](4,1) |" + NewLine +
                    "| [-](0,2) || [+](1,2) || [+](2,2) || [+](3,2) || [-](4,2) |" + NewLine +
                    "| [-](0,3) || [-](1,3) || [-](2,3) || [-](3,3) || [-](4,3) |" + NewLine +
                    "| [-](0,4) || [-](1,4) || [-](2,4) || [-](3,4) || [-](4,4) |" + NewLine;

                subject.Lives[1, 2].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.Lives[2, 2].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.Lives[3, 2].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.ToString().Should().Be(expectedGridLayout);
            }
        }

        [TestFixture]
        public class AndSeedingDataByConfiguringAStillLifeTubPattern : AndPlayingWithA5By5Matrix
        {
            [SetUp]
            public void SetupSeed()
            {
                subject.SeedLife(new Position(2, 1), new Position(1, 2), new Position(3, 2), new Position(2, 3));
            }

            [Test]
            public void ShouldGenerateAStillLifeTubNoMatterHowManyTimesThisIsGenerated()
            {
                try
                {
                    subject.GenerateFinished += OnGenerateFinished;

                    subject.Generate();
                    subject.Generate();
                    subject.Generate();
                }
                finally
                {
                    subject.GenerateFinished -= OnGenerateFinished;
                }
            }

            private void OnGenerateFinished(object sender, EventArgs e)
            {
                ShouldAlwaysHaveAStillLifeTubPattern();
            }

            private void ShouldAlwaysHaveAStillLifeTubPattern()
            {
                var expectedGridLayout =
                    "| [-](0,0) || [-](1,0) || [-](2,0) || [-](3,0) || [-](4,0) |" + NewLine +
                    "| [-](0,1) || [-](1,1) || [+](2,1) || [-](3,1) || [-](4,1) |" + NewLine +
                    "| [-](0,2) || [+](1,2) || [-](2,2) || [+](3,2) || [-](4,2) |" + NewLine +
                    "| [-](0,3) || [-](1,3) || [+](2,3) || [-](3,3) || [-](4,3) |" + NewLine +
                    "| [-](0,4) || [-](1,4) || [-](2,4) || [-](3,4) || [-](4,4) |" + NewLine;

                subject.Lives[2, 1].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.Lives[1, 2].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.Lives[3, 2].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.Lives[2, 3].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.ToString().Should().Be(expectedGridLayout);
            }
        }
    }
}