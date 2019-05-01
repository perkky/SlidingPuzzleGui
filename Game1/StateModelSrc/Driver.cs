/*using System;
using StateModel.InformedSearch;
using StateModel.Interface;
using StateModel.BoardGame;

namespace StateModel
{
    public class Driver
    {
        public static void Main(String[] args)
        {
            SlidingPuzzle puzzle = new SlidingPuzzle();
            //puzzle.SetState("{1,2,3,4,8,6,7,5,0}");
            puzzle.ShuffleBoard();
            var path = A_StarSearch<string, SlidingPuzzleAction>.Search(
                            puzzle,
                            "{1,2,3,4,5,6,7,8,0}");

            System.Console.WriteLine("Num steps = " + path.Count);
            while (path.Count > 0)
            {
                System.Console.WriteLine(path.Pop());
            }
        }


    }

}*/

