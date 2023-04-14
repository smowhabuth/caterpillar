using geca.caterpillar.service.Enum;
using geca.caterpillar.service.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace geca.caterpillar.service.Services
{
    public interface ICaterpillarService
    {
        public char[,] InitializeMap();
        public Caterpillar MoveHead(Direction direction, Caterpillar caterpillar);
        public Caterpillar MoveTail(Caterpillar caterpillar);
        public Caterpillar CollectSpice(char[,] map, Caterpillar caterpillar);
        public Caterpillar Boost(char[,] map, Caterpillar caterpillar);
        public bool CheckCrash(char[,] map, Caterpillar caterpillar);
        public Direction GetUserInput();
        public Caterpillar EndGame(Caterpillar caterpillar);
        public void UpdateGameBoard(char[,] map, Caterpillar caterpillar);
        public void PrintGameBoard(char[,] map, Caterpillar caterpillar);
        public bool WinGame(Caterpillar caterpillar);

        public void TestConsole();

    }
}
