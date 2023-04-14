using geca.caterpillar.service.Model;
using geca.caterpillar.service.Services;
using geca.caterpillar.service.Services.Implementation;
using System.Drawing;

class Program
{
    public static void Main(string[] args)
    {
        var service = new CaterpillarService();

        //set starting point
        Point startingPoint = new Point();
        startingPoint.X = 0; startingPoint.Y = 29;

        //Initialize map
        var map = service.InitializeMap();
        Caterpillar caterpillar = new Caterpillar(map, startingPoint);
        while (!caterpillar.gameEnded)
        {
            service.PrintGameBoard(map, caterpillar);

            //get input from user
            var direction = service.GetUserInput();

            //move head
            caterpillar = service.MoveHead(direction, caterpillar);

            if (!service.CheckCrash(caterpillar._map, caterpillar))
            {
                //check boost
                caterpillar = service.Boost(caterpillar._map, caterpillar);

                caterpillar = service.CollectSpice(caterpillar._map, caterpillar);


                caterpillar = service.MoveTail(caterpillar);
                service.PrintGameBoard(caterpillar._map, caterpillar);
            }

            else
            {
                caterpillar = service.EndGame(caterpillar);
            }
        }
    }
}

//public class MyProgram
//{
//    private readonly ICaterpillarService _caterpillarService;

//    public MyProgram(ICaterpillarService service)
//    {
//        _caterpillarService = service;
//    }
//}