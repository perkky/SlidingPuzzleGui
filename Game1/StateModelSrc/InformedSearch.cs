using System;
using System.Collections.Generic;
using StateModel.Interface;
using Priority_Queue;

namespace StateModel.InformedSearch
{
    public static class A_StarSearch<THash, TAction>
    {
        public static Stack<THash> Search(
            IAtomicState<THash,TAction> curState,
            THash goalState)
        {
           Stack<THash> path = new Stack<THash>();

            // Priority Q to store the node currently with the lowest f cost
            SimplePriorityQueue<THash,double> priorityQ = 
                new SimplePriorityQueue<THash,double>();

            var start = curState.GetState();

            // Dictionary to store the total cost to get to a state from start
            var costToStart = new Dictionary<THash,double>();
            var visited = new Dictionary<THash, Boolean>();
            var parent = new Dictionary<THash, THash>();


            priorityQ.Enqueue(curState.GetState(), 
                curState.H(goalState, "Default"));

            costToStart.Add(curState.GetState(), 0);
            visited[curState.GetState()] = false;
            do
            {
                var curStateHash = priorityQ.Dequeue();

                // If not visited
                if(!visited[curStateHash])
                {
                    // Set the state as visited
                    visited[curStateHash] = true;

                    // Set the state object to the current state
                    curState.SetState(curStateHash);

                    // Generate actions
                    var actions = curState.GetActions();

                    foreach (var action in actions)
                    {
                        var tranState = curState.TransitionState(action);
                        var tranCost  = curState.PathCost(action);
                        var g = (costToStart[curStateHash] + tranCost);

                        //Set to the transition state
                        curState.SetState(tranState);

                        //Set the transtate as not visited if its not already
                        //been visited
                        if(!visited.ContainsKey(tranState))
                            visited[tranState] = false;

                        if(!parent.ContainsKey(tranState))
                            parent[tranState] = curStateHash;


                        // f = h + g
                        var totalCost = curState.H(goalState,
                            "Default") + g;

                        priorityQ.Enqueue(tranState, tranCost);
                        costToStart[tranState] = g;

                        // Set to the parent state
                        curState.SetState(curStateHash);
                    }
                }

            } while (priorityQ.Count > 0 && !curState.Equals(goalState));

            //Generate the path

            //foreach(KeyValuePair<THash,THash> d in parent)
            //{
            //    System.Console.WriteLine(d.ToString());
            //}

            //System.Console.WriteLine(parent[curState.GetState()]);
            //curState.SetState(parent[curState.GetState()]);
            //System.Console.WriteLine(parent[curState.GetState()]);
            while(!curState.GetState().Equals(start))
            {
                path.Push(curState.GetState());
                curState.SetState(parent[curState.GetState()]);
            }
            path.Push(start);

            return path;
        }
    }

}
