using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP
{
    public class Node
    {
        public Node parent;
        public float cost;
        public Dictionary<string, int> state;
        public GAction action;

        public Node(Node _parent, float _cost, Dictionary<string, int> allstates, GAction _action)
        {
            parent = _parent;
            cost = _cost;
            state = new Dictionary<string, int>(allstates);
            action = _action;
        }

        public Node(Node _parent, 
            float _cost, 
            Dictionary<string, int> allstates, 
            GAction _action, 
            Dictionary<string, int> _beliefStates)
        {
            parent = _parent;
            cost = _cost;
            state = new Dictionary<string, int>(allstates);
            action = _action;
            foreach (var b in _beliefStates)
            {
                if (!state.ContainsKey(b.Key))
                    state.Add(b.Key, b.Value);
            }
        }
    }

    public class GPlanner
    {
        public Queue<GAction> Plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beliefStates)
        {
            List<GAction> usableActions = new List<GAction>();
            foreach (GAction a in actions)
            {
                if (a.IsAchievable())
                {
                    usableActions.Add(a);
                }
            }
            List<Node> leaves = new List<Node>();
            Node start = new Node(null, 0, GWorld.Instance.GetWorld().GetStates(), null, beliefStates.GetStates());

            bool success = BuildGraph(start, leaves, usableActions, goal);

            if (!success)
            {
                Debug.Log("NO PLAN");
                return null;
            }

            Node cheapest = null;
            foreach (Node leaf in leaves)
            {
                if (cheapest == null)
                {
                    cheapest = leaf;
                }
                else
                {
                    if (leaf.cost < cheapest.cost)
                        cheapest = leaf;
                }
            }

            List<GAction> result = new List<GAction>();
            Node n = cheapest;
            while (n != null)
            {
                if (n.action != null)
                {
                    result.Insert(0, n.action);
                }
                n = n.parent;
            }

            Queue<GAction> queue = new Queue<GAction>();
            foreach (GAction a in result)
            {
                queue.Enqueue(a);
            }

            //Debug.Log("The plan is: ");
            //foreach (GAction a in queue)
            //{
            //    Debug.Log("Q: " + a.actionName);
            //}
            return queue;
        }

        private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<string, int> goal)
        {
            bool foundPath = false;
            foreach (GAction action in usableActions)
            {
                if (action.IsAchievableGiven(parent.state))
                {
                    Dictionary<string, int> currentState = new Dictionary<string, int>(parent.state);
                    foreach (var eff in action.effects)
                    {
                        if (!currentState.ContainsKey(eff.Key))
                            currentState.Add(eff.Key, eff.Value);
                    }
                    Node node = new Node(parent, parent.cost + action.cost, currentState, action);

                    if (GoalAchieved(goal, currentState))
                    {
                        leaves.Add(node);
                        foundPath = true;
                    }
                    else
                    {
                        List<GAction> subset = ActionSubset(usableActions, action);
                        bool found = BuildGraph(node, leaves, subset, goal);
                        if (found)
                            foundPath = true;
                    }
                }
            }
            return foundPath;
        }

        private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
        {
            foreach (var g in goal)
            {
                if (!state.ContainsKey(g.Key))
                    return false;
            }
            return true;
        }

        private List<GAction> ActionSubset(List<GAction> actions, GAction toRemove)
        {
            List<GAction> subset = new List<GAction>();
            foreach (GAction a in actions)
            {
                if (!a.Equals(toRemove))
                    subset.Add(a);
            }
            return subset;
        }
    }
}

