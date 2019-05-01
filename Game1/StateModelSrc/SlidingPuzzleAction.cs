using System;
using System.Collections.Generic;

namespace StateModel.BoardGame
{
    public class SlidingPuzzleAction
    {
        public int From { get; set; }
        public int To { get; set; }

        public SlidingPuzzleAction()
        {
            From = 0;
            To = 0;
        }

        public SlidingPuzzleAction(int inFrom,int inTo)
        {
            From = inFrom;
            To = inTo;
        }

        public override string ToString()
        {
            return String.Format("{0},{1}", From, To);
        }

        public override bool Equals(object obj)
        {
            var action = obj as SlidingPuzzleAction;
            return action != null &&
                   From == action.From &&
                   To == action.To;
        }

        /*public override int GetHashCode()
        {
            return HashCode.Combine(From, To);
        }*/

        public static bool operator 
        ==(SlidingPuzzleAction action1, SlidingPuzzleAction action2)
        {
            return EqualityComparer<SlidingPuzzleAction>.
            Default.Equals(action1, action2);
        }

        public static bool operator 
        !=(SlidingPuzzleAction action1, SlidingPuzzleAction action2)
        {
            return !(action1 == action2);
        }
    }
}
