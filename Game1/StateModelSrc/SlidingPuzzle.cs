using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using StateModel.Interface;

namespace StateModel.BoardGame
{
    public class SlidingPuzzle : 
        IAtomicState<string,SlidingPuzzleAction>
    {
        // Class fields
        public const int DEFAULT_SIZE = 3;
        public int EmptyPos { get; private set; }
        public int[] Board { get; private set; }

        // Constructors
        public SlidingPuzzle()
        {
            Board = new int[(int)Math.Pow(DEFAULT_SIZE, 2)];

            for (int i = 0; i < Board.Length; i++)
            {
                Board[i] = i;
            }

            EmptyPos = 0;
        }

        public SlidingPuzzle(int inSize)
        {
            Board = new int[(int)Math.Pow(inSize, 2)];

            for (int i = 0; i < Board.Length; i++)
            {
                Board[i] = i;
            }

            EmptyPos = 0;
        }

        public SlidingPuzzle(int[] board)
        {
            // Check if length of board is a perfect square
            ValidateBoard(board);

            EmptyPos = Array.IndexOf(board, 0);
            Board = (int[]) board.Clone();
        }

        // Methods

        public int GetSize()
        {
            return (int) Math.Sqrt(Board.Length);
        }

        public void SwapTile(int from, int to)
        {
            int temp;

            ValidateMove(from,to);

            temp = Board[from];
            Board[from] = Board[to];
            Board[to] = temp;

            if (Board[from] == 0)
            {
                EmptyPos = from;
            }
            else
            {
                EmptyPos = to;
            }

        }

        public void ShuffleBoard()
        {
            //References: Fisher-Yates shuffle 
            //https://www.dotnetperls.com/fisher-yates-shuffle

            Random _random = new Random();
            int n = Board.Length;

            for (int i = 0; i < n; i++)
            {
                // Use Next on random instance with an argument.
                // ... The argument is an exclusive bound.
                //     So we will not go past the end of the array.
                int r = i + _random.Next(n - i);
                int t = Board[r];

                if (t == 0)
                {
                    EmptyPos = i;
                }

                Board[r] = Board[i];
                Board[i] = t;
            }
        }

        public int[] CreateBoard(string state)
        {
            int[] board;

            string[] data = state.Trim(new char[] { '{', '}' }).Split(',');
            int i = 0;
            board = new int[data.Length];
            foreach(string value in data)
            {
                int number = Int32.Parse(value);
                board[i] = number;
                i++;
            }

            ValidateBoard(board);

            return board;
        }

        public double Heuristic1(string goalState)
        {
            double est = 0;
            int[] goal = CreateBoard(goalState);

            if(goal.Length != Board.Length)
            {
                string msg = String.Format("Dimension must be the same: {0}",
                                goal.Length);
                throw new ArgumentException(msg);
            }

            for(int i = 0; i < goal.Length; i++)
            {
                var value = goal[i];
                est += Math.Abs(i - Array.IndexOf(Board, value));
            }

            return est;
        }

        public override String ToString()
        {
            return "{" + string.Join(",", Board) + "}";
        }

        // Interface Methods

        public string GetState()
        {
            return ToString();
        }

        public void SetState(string state)
        {
            Board = CreateBoard(state);
            EmptyPos = Array.IndexOf(Board, 0);
        }

        public List<SlidingPuzzleAction> GetActions()
        {
            List<SlidingPuzzleAction> actionL = new List<SlidingPuzzleAction>();

            int emptyRow = GetRow(EmptyPos);
            int emptyCol = GetCol(EmptyPos);

            //Cell is not at left edge
            if (emptyCol > 0)
            {
                actionL.Add(new SlidingPuzzleAction(EmptyPos, EmptyPos - 1));
            }

            //Cell is not at right edge
            if (emptyCol < GetSize() - 1)
            {
                actionL.Add(new SlidingPuzzleAction(EmptyPos, EmptyPos + 1));
            }

            //Cell is not at top edge
            if (emptyRow > 0)
            {
                actionL.Add(new SlidingPuzzleAction(EmptyPos,
                EmptyPos - GetSize()));
            }

            //Cell is not at bottom edge
            if (emptyRow < GetSize() - 1)
            {
                actionL.Add(new SlidingPuzzleAction(EmptyPos,
                EmptyPos + GetSize()));
            }
            return actionL;
        }

        public string TransitionState(SlidingPuzzleAction action)
        {
            string tranState;

            SwapTile(action.From, action.To);
            tranState = ToString();
            SwapTile(action.From, action.To);



            return tranState;
        }

        public double PathCost(SlidingPuzzleAction action)
        {
            ValidateMove(action.From, action.To);
            return 1.0;
        }

        public double H(String goalState, String hType)
        {
            switch(hType)
            {
                case "Default":
                    return Heuristic1(goalState);

                default:
                    string msg = "Invalid Heuristic type: %s";
                    throw new ArgumentException(String.Format(msg, hType));
            }
        }

        //Private Methods

        private void ValidateMove(int from, int to)
        {
            if (from < 0||from >= Board.Length||to < 0||to > Board.Length)
            {
                String message = "Index Out of Bounds";
                throw new IndexOutOfRangeException(message);
            }

            if (!(from == EmptyPos||to == EmptyPos))
            {
                String message = "Illegal Move, non-empty tiles";
                throw new ArgumentException(message);
            }

            if (!IsAdj(from, to))
            {
                String message = "Illegal Move, non-adjacent tiles";
                throw new ArgumentException(message);
            }
        }

        private void ValidateBoard(int[] board)
        {
            if ((Math.Abs(Math.Sqrt(board.Length) % 1) > 0.001))
            {
                string msg = String.Format("Length must be perfect square: {0}",
                    board.Length);

                throw new ArgumentException(msg);
            }

            // Check if all number exists, and there is no duplicates
            for (int i = 0; i < board.Length; i++)
            {
                if (Array.IndexOf(board, i) < 0)
                {
                    string msg = String.Format("Board does not contain {0}", i);
                    throw new ArgumentException(msg);
                }
            }
        }

        private Boolean IsAdj(int from, int to)
        {
            double dist = 0;

            dist += Math.Pow((GetRow(from) - GetRow(to)), 2);
            dist += Math.Pow((GetCol(from) - GetCol(to)), 2);

            return Math.Abs(Math.Sqrt(dist) - 1.0) < 0.001;
        }

        private int GetRow(int idx)
        {
            return idx / this.GetSize();
        }

        private int GetCol(int idx)
        {
            return idx % this.GetSize();
        }

        public bool Equals(string boardState)
        {
            var puzzle = CreateBoard(boardState);
            return Enumerable.SequenceEqual(puzzle, Board);
        }
    }
}
