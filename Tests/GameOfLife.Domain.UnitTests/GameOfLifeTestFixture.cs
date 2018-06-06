using System;
using NUnit.Framework;
using AutoFixture;
using FluentAssertions;
using static System.Environment;


namespace GameOfLife.Domain.UnitTests
{

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
        public class AndSettingTheSizeOfTheCellsMatrix : WhenPlayingGameOfLife
        {
            [SetUp]
            public void SetupTheGameAndMatrix()
            {
                matrixSize = GameOfLife.MINIMUM_MATRIX_SIZE + fixture.Create<uint>();
                subject = new GameOfLife(matrixSize);
            }

            [Test]
            public void ShouldSetTheMatrixByTheConstrainedSize()
            {
                subject.ColumnSize.Should().Be(matrixSize);
                subject.RowSize.Should().Be(matrixSize);
            }

            [TestCase((uint) 1)]
            [TestCase((uint) 0)]
            public void ShouldThrowArgumentOutOfRangeException(uint badMatrixSize)
            {
                Action action = () => new GameOfLife(badMatrixSize);

                action.Should().Throw<ArgumentOutOfRangeException>()
                    .WithMessage("Specified argument was out of the range of valid values."
                                 + NewLine +
                                 "Parameter name: Life can not thrive in such a small ecosystem.");
            }


            [Test]
            public void ShouldBuildAMatrixFilledWithDeadLife()
            {
                int expectedMatrixLength = Convert.ToInt32(matrixSize * matrixSize);

                subject.Cells.Length.Should().Be(expectedMatrixLength);
                for (var rowIndex = 0; rowIndex < matrixSize; rowIndex++)
                {
                    for (var colIndex = 0; colIndex < matrixSize; colIndex++)
                    {
                        subject.Cells[colIndex, rowIndex].Should().BeOfType<Cell>();
                        subject.Cells[colIndex, rowIndex].CurrentLifeState.Should().Be(LifeState.Dead);
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
                var lifeUnderTest = subject.Cells[0, 0];

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
                var lifeUnderTest = subject.Cells[1, 0];

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
                var lifeUnderTest = subject.Cells[2, 0];

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
                var lifeUnderTest = subject.Cells[0, 1];

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
                var lifeUnderTest = subject.Cells[0, 2];

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
                var lifeUnderTest = subject.Cells[1, 2];

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
                var lifeUnderTest = subject.Cells[2, 2];

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
                var lifeUnderTest = subject.Cells[2, 1];

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
                var lifeUnderTest = subject.Cells[1, 1];

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
                    "| [-](0,4) || [-](1,4) || [-](2,4) || [-](3,4) || [-](4,4) |";
                subject.Cells[1, 2].CurrentLifeState.Should().Be(LifeState.Dead);
                subject.Cells[2, 2].CurrentLifeState.Should().Be(LifeState.Dead);
                subject.Cells[3, 2].CurrentLifeState.Should().Be(LifeState.Dead);
                subject.ToString().Should().Be(expectedGridLayout);
            }
        }

        [TestFixture]
        public class AndPlayingWithA6By6Matrix : WhenPlayingGameOfLife
        {
            [SetUp]
            public void SetupTheGameWithAFiveByFiveLifeMatrixAndBlinkerSeed()
            {
                matrixSize = 6;
                subject = new GameOfLife(matrixSize);
                ShouldHaveDefaultSixBySixGrid();
            }

            private void ShouldHaveDefaultSixBySixGrid()
            {
                var expectedGridLayout =
                    "| [-](0,0) || [-](1,0) || [-](2,0) || [-](3,0) || [-](4,0) || [-](5,0) |" + NewLine +
                    "| [-](0,1) || [-](1,1) || [-](2,1) || [-](3,1) || [-](4,1) || [-](5,1) |" + NewLine +
                    "| [-](0,2) || [-](1,2) || [-](2,2) || [-](3,2) || [-](4,2) || [-](5,2) |" + NewLine +
                    "| [-](0,3) || [-](1,3) || [-](2,3) || [-](3,3) || [-](4,3) || [-](5,3) |" + NewLine +
                    "| [-](0,4) || [-](1,4) || [-](2,4) || [-](3,4) || [-](4,4) || [-](5,4) |" + NewLine +
                    "| [-](0,5) || [-](1,5) || [-](2,5) || [-](3,5) || [-](4,5) || [-](5,5) |" ;

                var actualGridLayout = subject.ToString();

                actualGridLayout.Should().Be(expectedGridLayout);
            }
        }

        [TestFixture]
        public class AndPlayingWithA18By11Matrix : WhenPlayingGameOfLife
        {
            private uint columnSize;
            private uint rowSize;

            [SetUp]
            public void SetupTheGameWithAElevenByEighteenLifeMatrix()
            {
                columnSize = 18;
                rowSize = 11;
                subject = new GameOfLife(columnSize, rowSize);
                ShouldHaveDefaultElevenByEighteenGrid();
            }

            private void ShouldHaveDefaultElevenByEighteenGrid()
            {
                var expectedGridLayout =
                    "| [-](00,00) || [-](01,00) || [-](02,00) || [-](03,00) || [-](04,00) || [-](05,00) || [-](06,00) || [-](07,00) || [-](08,00) || [-](09,00) || [-](10,00) || [-](11,00) || [-](12,00) || [-](13,00) || [-](14,00) || [-](15,00) || [-](16,00) || [-](17,00) |" + NewLine +
                    "| [-](00,01) || [-](01,01) || [-](02,01) || [-](03,01) || [-](04,01) || [-](05,01) || [-](06,01) || [-](07,01) || [-](08,01) || [-](09,01) || [-](10,01) || [-](11,01) || [-](12,01) || [-](13,01) || [-](14,01) || [-](15,01) || [-](16,01) || [-](17,01) |" + NewLine +
                    "| [-](00,02) || [-](01,02) || [-](02,02) || [-](03,02) || [-](04,02) || [-](05,02) || [-](06,02) || [-](07,02) || [-](08,02) || [-](09,02) || [-](10,02) || [-](11,02) || [-](12,02) || [-](13,02) || [-](14,02) || [-](15,02) || [-](16,02) || [-](17,02) |" + NewLine +
                    "| [-](00,03) || [-](01,03) || [-](02,03) || [-](03,03) || [-](04,03) || [-](05,03) || [-](06,03) || [-](07,03) || [-](08,03) || [-](09,03) || [-](10,03) || [-](11,03) || [-](12,03) || [-](13,03) || [-](14,03) || [-](15,03) || [-](16,03) || [-](17,03) |" + NewLine +
                    "| [-](00,04) || [-](01,04) || [-](02,04) || [-](03,04) || [-](04,04) || [-](05,04) || [-](06,04) || [-](07,04) || [-](08,04) || [-](09,04) || [-](10,04) || [-](11,04) || [-](12,04) || [-](13,04) || [-](14,04) || [-](15,04) || [-](16,04) || [-](17,04) |" + NewLine +
                    "| [-](00,05) || [-](01,05) || [-](02,05) || [-](03,05) || [-](04,05) || [-](05,05) || [-](06,05) || [-](07,05) || [-](08,05) || [-](09,05) || [-](10,05) || [-](11,05) || [-](12,05) || [-](13,05) || [-](14,05) || [-](15,05) || [-](16,05) || [-](17,05) |" + NewLine +
                    "| [-](00,06) || [-](01,06) || [-](02,06) || [-](03,06) || [-](04,06) || [-](05,06) || [-](06,06) || [-](07,06) || [-](08,06) || [-](09,06) || [-](10,06) || [-](11,06) || [-](12,06) || [-](13,06) || [-](14,06) || [-](15,06) || [-](16,06) || [-](17,06) |" + NewLine +
                    "| [-](00,07) || [-](01,07) || [-](02,07) || [-](03,07) || [-](04,07) || [-](05,07) || [-](06,07) || [-](07,07) || [-](08,07) || [-](09,07) || [-](10,07) || [-](11,07) || [-](12,07) || [-](13,07) || [-](14,07) || [-](15,07) || [-](16,07) || [-](17,07) |" + NewLine +
                    "| [-](00,08) || [-](01,08) || [-](02,08) || [-](03,08) || [-](04,08) || [-](05,08) || [-](06,08) || [-](07,08) || [-](08,08) || [-](09,08) || [-](10,08) || [-](11,08) || [-](12,08) || [-](13,08) || [-](14,08) || [-](15,08) || [-](16,08) || [-](17,08) |" + NewLine +
                    "| [-](00,09) || [-](01,09) || [-](02,09) || [-](03,09) || [-](04,09) || [-](05,09) || [-](06,09) || [-](07,09) || [-](08,09) || [-](09,09) || [-](10,09) || [-](11,09) || [-](12,09) || [-](13,09) || [-](14,09) || [-](15,09) || [-](16,09) || [-](17,09) |" + NewLine +
                    "| [-](00,10) || [-](01,10) || [-](02,10) || [-](03,10) || [-](04,10) || [-](05,10) || [-](06,10) || [-](07,10) || [-](08,10) || [-](09,10) || [-](10,10) || [-](11,10) || [-](12,10) || [-](13,10) || [-](14,10) || [-](15,10) || [-](16,10) || [-](17,10) |";

                var actualGridLayout = subject.ToString();

                actualGridLayout.Should().Be(expectedGridLayout);
            }
        }

        [TestFixture]
        public class AndSeedingPentadecathlonDataByConfiguringTheOscilattorPattern : AndPlayingWithA18By11Matrix
        {
            // Pattern represents
            //   +     +
            // ++ ++ ++ ++
            //   +     +
            private readonly string ORIGINAL_SEEDED_PATTERN = 
            "| [-](00,00) || [-](01,00) || [-](02,00) || [-](03,00) || [-](04,00) || [-](05,00) || [-](06,00) || [-](07,00) || [-](08,00) || [-](09,00) || [-](10,00) || [-](11,00) || [-](12,00) || [-](13,00) || [-](14,00) || [-](15,00) || [-](16,00) || [-](17,00) |" + NewLine +
            "| [-](00,01) || [-](01,01) || [-](02,01) || [-](03,01) || [-](04,01) || [-](05,01) || [-](06,01) || [-](07,01) || [-](08,01) || [-](09,01) || [-](10,01) || [-](11,01) || [-](12,01) || [-](13,01) || [-](14,01) || [-](15,01) || [-](16,01) || [-](17,01) |" + NewLine +
            "| [-](00,02) || [-](01,02) || [-](02,02) || [-](03,02) || [-](04,02) || [-](05,02) || [-](06,02) || [-](07,02) || [-](08,02) || [-](09,02) || [-](10,02) || [-](11,02) || [-](12,02) || [-](13,02) || [-](14,02) || [-](15,02) || [-](16,02) || [-](17,02) |" + NewLine +

            "| [-](00,03) || [-](01,03) || [-](02,03) || [-](03,03) || [-](04,03) || [-](05,03) || [-](06,03) || [-](07,03) || [-](08,03) || [-](09,03) || [-](10,03) || [-](11,03) || [-](12,03) || [-](13,03) || [-](14,03) || [-](15,03) || [-](16,03) || [-](17,03) |" + NewLine +
            "| [-](00,04) || [-](01,04) || [-](02,04) || [-](03,04) || [-](04,04) || [-](05,04) || [+](06,04) || [-](07,04) || [-](08,04) || [-](09,04) || [-](10,04) || [+](11,04) || [-](12,04) || [-](13,04) || [-](14,04) || [-](15,04) || [-](16,04) || [-](17,04) |" + NewLine +
            "| [-](00,05) || [-](01,05) || [-](02,05) || [-](03,05) || [+](04,05) || [+](05,05) || [-](06,05) || [+](07,05) || [+](08,05) || [+](09,05) || [+](10,05) || [-](11,05) || [+](12,05) || [+](13,05) || [-](14,05) || [-](15,05) || [-](16,05) || [-](17,05) |" + NewLine +
            "| [-](00,06) || [-](01,06) || [-](02,06) || [-](03,06) || [-](04,06) || [-](05,06) || [+](06,06) || [-](07,06) || [-](08,06) || [-](09,06) || [-](10,06) || [+](11,06) || [-](12,06) || [-](13,06) || [-](14,06) || [-](15,06) || [-](16,06) || [-](17,06) |" + NewLine +
            "| [-](00,07) || [-](01,07) || [-](02,07) || [-](03,07) || [-](04,07) || [-](05,07) || [-](06,07) || [-](07,07) || [-](08,07) || [-](09,07) || [-](10,07) || [-](11,07) || [-](12,07) || [-](13,07) || [-](14,07) || [-](15,07) || [-](16,07) || [-](17,07) |" + NewLine +

            "| [-](00,08) || [-](01,08) || [-](02,08) || [-](03,08) || [-](04,08) || [-](05,08) || [-](06,08) || [-](07,08) || [-](08,08) || [-](09,08) || [-](10,08) || [-](11,08) || [-](12,08) || [-](13,08) || [-](14,08) || [-](15,08) || [-](16,08) || [-](17,08) |" + NewLine +
            "| [-](00,09) || [-](01,09) || [-](02,09) || [-](03,09) || [-](04,09) || [-](05,09) || [-](06,09) || [-](07,09) || [-](08,09) || [-](09,09) || [-](10,09) || [-](11,09) || [-](12,09) || [-](13,09) || [-](14,09) || [-](15,09) || [-](16,09) || [-](17,09) |" + NewLine +
            "| [-](00,10) || [-](01,10) || [-](02,10) || [-](03,10) || [-](04,10) || [-](05,10) || [-](06,10) || [-](07,10) || [-](08,10) || [-](09,10) || [-](10,10) || [-](11,10) || [-](12,10) || [-](13,10) || [-](14,10) || [-](15,10) || [-](16,10) || [-](17,10) |";

            [SetUp]
            public void SetupPentadecathlonOscilattorSeedData()
            {
                //   +     +
                // ++ ++ ++ ++
                //   +     +
                subject.SeedLife(new Position(6, 4), new Position(11,4));
                subject.SeedLife(new Position(4, 5), new Position(5, 5), new Position(7, 5), new Position(8,5), new Position(9,5), new Position(10,5),  new Position(12, 5), new Position(13, 5));
                subject.SeedLife(new Position(6, 6), new Position(11, 6));
                ShouldHaveTheStartSeededPentadecathlonData();
            }

            [Test]
            public void ShouldNotHaveEqualColumnAndRowSize()
            {
                subject.ColumnSize.Should().NotBe(subject.RowSize);
                subject.ColumnSize.Should().Be(18);
                subject.RowSize.Should().Be(11);
            }

            [Test]
            public void ShouldBeAbleToOscillateInPeriodsOf15InorderToComeBackToTheOriginalSeededPattern()
            {
                for (var i = 1; i <= 45; i++)
                {
                    subject.Generate();
                    switch (i)
                    {
                        case 15:
                        case 30:
                        case 45:
                            ShouldHaveTheStartSeededPentadecathlonData();
                            break;
                        default:
                            ShouldNotBeTheStartSeededPentadecathlonData();
                            break;
                    }
                }               
            }

            private void ShouldHaveTheStartSeededPentadecathlonData()
            {
                var expectedGridLayout = ORIGINAL_SEEDED_PATTERN;
                subject.ToString().Should().Be(expectedGridLayout);
            }

            private void ShouldNotBeTheStartSeededPentadecathlonData()
            {
                var expectedGridLayout = ORIGINAL_SEEDED_PATTERN;
                subject.ToString().Should().NotBe(expectedGridLayout);
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
                    "| [-](0,4) || [-](1,4) || [-](2,4) || [-](3,4) || [-](4,4) |";
                subject.Cells[2, 1].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.Cells[2, 2].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.Cells[2, 3].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.ToString().Should().Be(expectedGridLayout);
            }


            private void ShouldHaveAnAliveHorizontalBlink()
            {
                var expectedGridLayout =
                    "| [-](0,0) || [-](1,0) || [-](2,0) || [-](3,0) || [-](4,0) |" + NewLine +
                    "| [-](0,1) || [-](1,1) || [-](2,1) || [-](3,1) || [-](4,1) |" + NewLine +
                    "| [-](0,2) || [+](1,2) || [+](2,2) || [+](3,2) || [-](4,2) |" + NewLine +
                    "| [-](0,3) || [-](1,3) || [-](2,3) || [-](3,3) || [-](4,3) |" + NewLine +
                    "| [-](0,4) || [-](1,4) || [-](2,4) || [-](3,4) || [-](4,4) |";

                subject.Cells[1, 2].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.Cells[2, 2].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.Cells[3, 2].CurrentLifeState.Should().Be(LifeState.Alive);
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
                    "| [-](0,4) || [-](1,4) || [-](2,4) || [-](3,4) || [-](4,4) |";

                subject.Cells[2, 1].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.Cells[1, 2].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.Cells[3, 2].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.Cells[2, 3].CurrentLifeState.Should().Be(LifeState.Alive);
                subject.ToString().Should().Be(expectedGridLayout);
            }
        }

        [TestFixture]
        public class AndSeedingDataByConfiguringTheToadOscilattorPattern : AndPlayingWithA6By6Matrix
        {
            [SetUp]
            public void SetupToadOscilattorData()
            {
                subject.SeedLife(new Position(2, 2), new Position(3, 2), new Position(4, 2));
                subject.SeedLife(new Position(1, 3), new Position(2, 3), new Position(3, 3));
            }

            [Test]
            public void ShouldSeedLifeByBlinkerPositions()
            {
                ShouldHaveTheOriginalToadBlink();

                subject.Generate();
                ShouldHaveTheAlternativeToadBlink();

                subject.Generate();
                ShouldHaveTheOriginalToadBlink();

                subject.Generate();
                ShouldHaveTheAlternativeToadBlink();
            }

            private void ShouldHaveTheAlternativeToadBlink()
            {
                var expectedGridLayout =
                    "| [-](0,0) || [-](1,0) || [-](2,0) || [-](3,0) || [-](4,0) || [-](5,0) |" + NewLine +
                    "| [-](0,1) || [-](1,1) || [-](2,1) || [+](3,1) || [-](4,1) || [-](5,1) |" + NewLine +
                    "| [-](0,2) || [+](1,2) || [-](2,2) || [-](3,2) || [+](4,2) || [-](5,2) |" + NewLine +
                    "| [-](0,3) || [+](1,3) || [-](2,3) || [-](3,3) || [+](4,3) || [-](5,3) |" + NewLine +
                    "| [-](0,4) || [-](1,4) || [+](2,4) || [-](3,4) || [-](4,4) || [-](5,4) |" + NewLine +
                    "| [-](0,5) || [-](1,5) || [-](2,5) || [-](3,5) || [-](4,5) || [-](5,5) |";
                subject.ToString().Should().Be(expectedGridLayout);
            }

            private void ShouldHaveTheOriginalToadBlink()
            {
                var expectedGridLayout =
                    "| [-](0,0) || [-](1,0) || [-](2,0) || [-](3,0) || [-](4,0) || [-](5,0) |" + NewLine +
                    "| [-](0,1) || [-](1,1) || [-](2,1) || [-](3,1) || [-](4,1) || [-](5,1) |" + NewLine +
                    "| [-](0,2) || [-](1,2) || [+](2,2) || [+](3,2) || [+](4,2) || [-](5,2) |" + NewLine +
                    "| [-](0,3) || [+](1,3) || [+](2,3) || [+](3,3) || [-](4,3) || [-](5,3) |" + NewLine +
                    "| [-](0,4) || [-](1,4) || [-](2,4) || [-](3,4) || [-](4,4) || [-](5,4) |" + NewLine +
                    "| [-](0,5) || [-](1,5) || [-](2,5) || [-](3,5) || [-](4,5) || [-](5,5) |";
                subject.ToString().Should().Be(expectedGridLayout);
            }
        }
    }
}