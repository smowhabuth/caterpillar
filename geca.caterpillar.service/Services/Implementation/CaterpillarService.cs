using geca.caterpillar.service.Enum;
using geca.caterpillar.service.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geca.caterpillar.service.Services.Implementation
{
    public class CaterpillarService : ICaterpillarService
    {

        public char[,] InitializeMap()
        {
            // Define the map dimensions
            int width = 30;
            int height = 30;

            // Create a 2D array to store the map
            char[,] map = new char[height, width];

            // Populate the map with the provided symbols
            string[] rows = new string[] {
            "$*********$**********$********",
            "***$*******B*************#****",
            "************************#*****",
            "***#**************************",
            "**$*************************#*",
            "$$***#************************",
            "**************$***************",
            "**********$*********$*****#***",
            "********************$*******$*",
            "*********#****$***************",
            "**B*********$*****************",
            "*************$$****B**********",
            "****$************************B",
            "**********************#*******",
            "***********************$***B**",
            "********$***$*****************",
            "************$*****************",
            "*********$********************",
            "*********************#********",
            "*******$**********************",
            "*#***$****************#*******",
            "****#****$****$********B******",
            "***#**$********************$**",
            "***************#**************",
            "***********$******************",
            "****B****#******B*************",
            "***$***************$*****B****",
            "**********$*********#*$*******",
            "**************#********B******",
            "s**********$*********#*B******"
            };
            for (int y = 0; y < height; y++)
            {
                string row = rows[y];
                for (int x = 0; x < width; x++)
                {
                    map[y, x] = row[x];
                }
            }

            // Print out the map for verification
            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    Console.Write(map[y, x]);
                }
                Console.WriteLine();
            }

            return map;
        }
        public Caterpillar Boost(char[,] map, Caterpillar caterpillar)
        {
            if (map[caterpillar.Head.X, caterpillar.Head.Y] == (char)SquareContent.Booster)
            {
                caterpillar.speedBoosted = true;
                caterpillar.speedBoostRemaining = 5; // or any number of moves you want the boost to last
                //map[caterpillar.Head.X, caterpillar.Head.Y] = (char)SquareContent.Empty;
            }

            caterpillar._map = map;

            return caterpillar;
        }

        public bool CheckCrash(char[,] map, Caterpillar caterpillar)
        {
            return map[caterpillar.Head.X, caterpillar.Head.Y] == (char)SquareContent.Obstacle;
        }

        public Caterpillar CollectSpice(char[,] map, Caterpillar caterpillar)
        {
            var xxx = map[caterpillar.Head.X, caterpillar.Head.Y];
            Console.WriteLine(xxx);
            if (map[caterpillar.Head.X, caterpillar.Head.Y] == (char)SquareContent.Spice)
            {
                caterpillar.SpiceCollected++;
                map[caterpillar.Head.X, caterpillar.Head.Y] = (char)SquareContent.Empty;
            }

            caterpillar._map = map;

            return caterpillar;
        }

        public Caterpillar EndGame(Caterpillar caterpillar)
        {
            Console.WriteLine("Game over. You crashed into an obstacle.");
            caterpillar.gameEnded = true;
            return caterpillar;
        }

        public Direction GetUserInput()
        {
            Console.Write("Enter a direction to move the caterpillar's head (up/down/left/right): ");
            string userInput = Console.ReadLine().ToLower();

            while (userInput != "up" && userInput != "down" && userInput != "left" && userInput != "right" )
            {
                Console.WriteLine("Invalid input. Please enter a valid direction.");
                Console.Write("Enter a direction to move the caterpillar's head (up/down/left/right): ");
                userInput = Console.ReadLine().ToLower();
            }

            Direction direction = new Direction();

            switch (userInput)
            {
                case "up":
                    direction = Direction.Up;
                    break;
                case "down":
                    direction = Direction.Down;
                    break;
                case "right":
                    direction = Direction.Right;
                    break;
                case "left":
                    direction = Direction.Left;
                    break;
            }
            return direction;
        }

        public Caterpillar MoveHead(Direction direction, Caterpillar caterpillar)
        {
            Point newHead = new Point(caterpillar.Head.X, caterpillar.Head.Y);
            switch (direction)
            {
                case Direction.Up:
                    newHead.Y--;
                    break;
                case Direction.Down:
                    newHead.Y++;
                    break;
                case Direction.Left:
                    newHead.X--;
                    break;
                case Direction.Right:
                    newHead.X++;
                    break;
            }

            if (!IsWithinBounds(newHead, caterpillar) || caterpillar._map[newHead.Y, newHead.X] == '#')
            {
                return null;
            }

            caterpillar._tail.Enqueue(caterpillar.Head);
            caterpillar.Head = newHead;
            return caterpillar;
        }

        public Caterpillar MoveTail(Caterpillar caterpillar)
        {
            if (caterpillar.Head.Y <=27)
            {
                Point previousTailPosition = caterpillar._tail.Dequeue();
                Point secondToLastTailPosition = caterpillar._tail.Peek();
                Direction tailDirection = GetDirection(previousTailPosition, secondToLastTailPosition);

                if (previousTailPosition.X == caterpillar.Head.X && previousTailPosition.Y == caterpillar.Head.Y)
                {
                    caterpillar._tail.Enqueue(previousTailPosition);
                    return caterpillar;
                }

                switch (tailDirection)
                {
                    case Direction.Up:
                        caterpillar._tail.Enqueue(new Point(previousTailPosition.X - 1, previousTailPosition.Y));
                        break;
                    case Direction.Down:
                        caterpillar._tail.Enqueue(new Point(previousTailPosition.X + 1, previousTailPosition.Y));
                        break;
                    case Direction.Left:
                        caterpillar._tail.Enqueue(new Point(previousTailPosition.X, previousTailPosition.Y - 1));
                        break;
                    case Direction.Right:
                        caterpillar._tail.Enqueue(new Point(previousTailPosition.X, previousTailPosition.Y + 1));
                        break;
                }
            }
            
            return caterpillar;
        }

        public void PrintGameBoard(char[,] map, Caterpillar caterpillar)
        {
            Console.Clear();

            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    if (row == caterpillar.Head.Y && col == caterpillar.Head.X && !(row == 29 && col == 0))
                    {
                        Console.Write("H");
                    }
                    else if (caterpillar._tail.Any(position => position.Y == row && position.X == col) && !(row == 29 && col == 0))
                    {
                        Console.Write("T");
                    }
                    else if (row == 29 && col == 0)
                    {
                        Console.Write("s");
                    }
                    else
                    {

                        Console.Write(map[row, col]);
                        //var blabla = (char)map[row, col];
                        //switch (blabla)
                        //{
                        //    case (char)SquareContent.Obstacle:
                        //        Console.Write("X ");
                        //        break;
                        //    case (char)SquareContent.Spice:
                        //        Console.Write("* ");
                        //        break;
                        //    case (char)SquareContent.Booster:
                        //        Console.Write("B ");
                        //        break;
                        //}
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine($"Spice collected: {caterpillar.SpiceCollected}/{caterpillar.spiceRequired}");
            Console.WriteLine($"Boosted moves left: {caterpillar.BoostedMovesLeft}");
        }

        public void UpdateGameBoard(char[,] map, Caterpillar caterpillar)
        {
            Console.Clear();
            PrintGameBoard(map, caterpillar);
            Console.WriteLine("Spice collected: " + caterpillar.SpiceCollected);
        }

        public bool WinGame(Caterpillar caterpillar)
        {
            Console.WriteLine("Congratulations! You collected enough spice to win the game.");
            caterpillar.gameEnded = true;

            return true;
        }

        public static Direction GetDirection(Point start, Point end)
        {
            if (end.X < start.X)
            {
                return Direction.Up;
            }
            else if (end.X > start.X)
            {
                return Direction.Down;
            }
            else if (end.Y < start.Y)
            {
                return Direction.Left;
            }
            else
            {
                return Direction.Right;
            }
        }

        private bool IsWithinBounds(Point point, Caterpillar caterpillar)
        {
            return point.X >= 0 && point.X < caterpillar._mapWidth && point.Y >= 0 && point.Y < caterpillar._mapHeight;
        }

        public void TestConsole()
        {
            Console.Clear();
            Console.WriteLine("Helloooooooooooooooo");
        }
    }
}
