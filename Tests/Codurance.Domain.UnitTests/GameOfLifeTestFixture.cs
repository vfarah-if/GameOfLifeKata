//using System;
//using Moq;
//using NUnit.Framework;
//using AutoFixture;
//using AutoFixture.AutoMoq;
//using AutoFixture.NUnit3;


//namespace GameOfLife.Domain.UnitTests
//{
//    [TestFixture]
//    public class GameOfLifeTestFixture
//    {
//        private IFixture fixture;
//        // private Mock<ISomeDependency> someDependencyMock;

//        [OneTimeSetUp]
//        public void OneTimeSetUp()
//        {
//            fixture = new Fixture().Customize(new AutoMoqCustomization());
//            // someDependencyMock = fixture.Freeze<Mock<ISomeDependency>>();
//        }

//        [TearDown]
//        public void TearDown()
//        {
//            // someDependencyMock.Reset();
//        }

//        [TestFixture]
//        public class FirstMethodTestFixture : GameOfLifeTestFixture
//        {
//            /*
//             * Only use Setup() for genuinely common code required by the majority of tests in the fixture.
//             * 
//            [SetUp]
//            public void Setup()
//            {
//                // someDependency
//                //  .Setup(x => x.SomeMethod(It.IsAny<string>()))
//                //  .Returns(SomeObject);
//            }
//            */

//            [Test]
//            public void FirstMethod_WhenSomeScenario_MustBehaveAsExpected()
//            {
//                // Arrange

//                // Act

//                // Assert
//                Assert.Fail("Unimplemented test case");
//            }
//        }
//    }
//}