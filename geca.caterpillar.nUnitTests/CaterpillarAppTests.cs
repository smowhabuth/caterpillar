using geca.caterpillar.service.Enum;
using geca.caterpillar.service.Model;
using geca.caterpillar.service.Services;
using System.Drawing;

namespace geca.caterpillar.nUnitTests
{
    public class CaterpillarAppTests
    {
        private char[,] map;
        private Point StartingPosition;
        private readonly ICaterpillarService _caterpillarService;

        public CaterpillarAppTests(ICaterpillarService service)
        {
            _caterpillarService = service;
        }

        [SetUp]
        public void Setup()
        {
            // Arrange
            map = _caterpillarService.InitializeMap();
        }

        #region MapTests

        [Test]
        public void MapDimensions_AreCorrect()
        {
            // Act
            int height = map.GetLength(0);
            int width = map.GetLength(1);

            // Assert
            Assert.AreEqual(30, height);
            Assert.AreEqual(30, width);
        }

        [Test]
        public void Map_StartsAndEndsWithS()
        {
            // Assert
            Assert.AreEqual('s', map[29, 0]);
            Assert.AreEqual('s', map[0, 29]);
        }

        [Test]
        public void Map_HasCorrectNumberOfBoosters()
        {
            // Arrange
            int expectedBoosters = 3;

            // Act
            int actualBoosters = 0;
            for (int y = 0; y < 30; y++)
            {
                for (int x = 0; x < 30; x++)
                {
                    if (map[y, x] == 'B')
                    {
                        actualBoosters++;
                    }
                }
            }

            // Assert
            Assert.AreEqual(expectedBoosters, actualBoosters);
        }
        #endregion // end of Map Tests

        #region BoosterTests
        [Test]
        public void Boost_ShouldActivateSpeedBoost_WhenCaterpillarHeadIsOnBoosterSquare()
        {
            // Arrange
            _caterpillarService.InitializeMap();
            StartingPosition = new Point(1, 29);
            Caterpillar caterpillar = new Caterpillar(map, StartingPosition);

            // Act
            Caterpillar result = _caterpillarService.Boost(map, caterpillar);

            // Assert
            Assert.IsTrue(result.speedBoosted);
            Assert.AreEqual(5, result.speedBoostRemaining);
        }
        #endregion

        #region CaterpillarCrashTests
        [Test]
        public void CheckCrash_ReturnsTrueWhenCaterpillarCrashes()
        {
            // Arrange
            _caterpillarService.InitializeMap();
            Caterpillar caterpillar = new Caterpillar(map, new Point(14, 0));

            // Act
            bool actualResult = _caterpillarService.CheckCrash(map, caterpillar);

            // Assert
            Assert.IsTrue(actualResult);
        }
        #endregion

        #region CollectSpiceTests
        [Test]
        public void CollectSpice_ShouldIncreaseSpiceCollected_WhenCaterpillarOnSpice()
        {
            // Arrange
            _caterpillarService.InitializeMap();
            var caterpillar = new Caterpillar(map, new Point(1, 1));
            map[caterpillar.Head.X, caterpillar.Head.Y] = (char)SquareContent.Spice;
            var expectedSpiceCollected = 1;

            // Act
            var result = _caterpillarService.CollectSpice(map, caterpillar);

            // Assert
            Assert.AreEqual(expectedSpiceCollected, result.SpiceCollected);
        }

        [Test]
        public void CollectSpice_ShouldNotIncreaseSpiceCollected_WhenCaterpillarNotOnSpice()
        {
            // Arrange
            _caterpillarService.InitializeMap();
            var caterpillar = new Caterpillar(map, new Point(1, 1));
            var expectedSpiceCollected = 0;

            // Act
            var result = _caterpillarService.CollectSpice(map, caterpillar);

            // Assert
            Assert.AreEqual(expectedSpiceCollected, result.SpiceCollected);
        }
        #endregion

        #region GetUserInput
        [Test]
        public void GetUserInput_ShouldReturnValidDirection()
        {
            // Arrange
            var input = "up";
            var expectedDirection = Direction.Up;

            var reader = new StringReader(input);
            Console.SetIn(reader);

            // Act
            var actualDirection = _caterpillarService.GetUserInput();

            // Assert
            Assert.AreEqual(expectedDirection, actualDirection);
        }
        #endregion

        #region MoveHead
        [Test]
        public void MoveHead_ValidInput_CaterpillarMoved()
        {
            // Arrange
            var caterpillar = new Caterpillar(map, new Point(3, 3));
            caterpillar.Head = new Point(3, 3);
            caterpillar._tail.Enqueue(new Point(3, 4));
            caterpillar._map[3, 3] = ' ';
            caterpillar._map[3, 4] = 'o';

            // Act
            var result = _caterpillarService.MoveHead(Direction.Down, caterpillar);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(new Point(3, 4), result.Head);
            Assert.AreEqual(new Point(3, 3), result._tail.Peek());
            Assert.AreEqual('o', result._map[3, 4]);
            Assert.AreEqual(' ', result._map[3, 3]);
        }
        #endregion

        #region MoveTail
        [Test]
        public void MoveTail_ValidCaterpillar_ReturnsCaterpillarWithMovedTail()
        {
            // Arrange
            var caterpillar = new Caterpillar(map,new Point(5, 5));

            caterpillar._tail = new Queue<Point>(new[]
                {
                new Point(5, 4),
                new Point(5, 3),
                new Point(5, 2)
            });

            // Act
            var result = _caterpillarService.MoveTail(caterpillar);

            // Assert
            Assert.AreEqual(4, result._tail.Count);
            Assert.AreEqual(new Point(5, 3), result._tail.Peek());
            Assert.AreEqual(new Point(5, 4), result._tail.Dequeue());
        }

        [Test]
        public void MoveTail_NullCaterpillar_ReturnsNull()
        {
            // Arrange
            var caterpillar = new Caterpillar(map, new Point(0, 0));

            caterpillar._tail = new Queue<Point>(new[]
                {
                new Point(5, 4),
                new Point(5, 3),
                new Point(5, 2)
            });
        

            // Act
            var result = _caterpillarService.MoveTail(null);

            // Assert
            Assert.IsNull(result);
        }
        #endregion

        #region PrintGameboard
        [Test]
        public void TestPrintGameBoard()
        {
            char[,] map = new char[10, 10];
            for (int i = 0; i < 10; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    map[i, j] = '.';
                }
            }

            Caterpillar caterpillar = new Caterpillar(map, new Point(4, 4));
            caterpillar._tail.Enqueue(new Point(4, 5));
            caterpillar._tail.Enqueue(new Point(4, 6));
            caterpillar._tail.Enqueue(new Point(3, 6));

            StringWriter consoleOutput = new StringWriter();
            Console.SetOut(consoleOutput);

           _caterpillarService.PrintGameBoard(map, caterpillar);

            string expectedOutput = "H . . . . . . . . . \n. . . . . . . . . . \n. . . . . . . . . . \n. . . . . . . . . . \n. . T T H . . . . . \n. . . . . . . . . . \n. . . . . . . . . . \n. . . . . . . . . . \n. . . . . . . . . . \n. . . . . . . . . . \nSpice collected: 4/2\nBoosted moves left: 0\n";

            Assert.AreEqual(expectedOutput, consoleOutput.ToString());
        }
        #endregion
    }
}