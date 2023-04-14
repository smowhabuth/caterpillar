using geca.caterpillar.service.Enum;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geca.caterpillar.service.Model
{
    public class Caterpillar
    {
        public bool speedBoosted { get; set; }
        public int speedBoostRemaining { get; set; }
        public bool gameEnded { get; set; }
        public int spiceRequired { get; set; }
        public char[,] _map { get; set; }
        public int _mapWidth { get; set; }
        public int _mapHeight { get; set; }
        public Queue<Point> _tail { get; set; }
        public int SpiceCollected { get; set; }
        public int BoostedMovesLeft { get; set; }
        public Point Head { get; set; }

        public Caterpillar(char[,] map, Point startingPosition)
        {
            _map = map;
            _mapWidth = map.GetLength(1);
            _mapHeight = map.GetLength(0);
            Head = startingPosition;
            _tail = new Queue<Point>();
        }
    }
}
