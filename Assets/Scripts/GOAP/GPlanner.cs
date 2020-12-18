using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node
{
    public Node parent;
    public float cost;
    public Dictionary<string, int> states;
    public GAction action;

    public Node (Node p, float c, Dictionary<string, int> allStates, GAction a)
    {
        parent = p;
        cost = c;
        states = new Dictionary<string, int>(allStates);
        action = a;
    }
    public Node(Node p, float c, Dictionary<string, int> allStates, Dictionary<string, int> beliefStates, GAction a)
    {
        parent = p;
        cost = c;
        states = new Dictionary<string, int>(allStates);
        foreach(KeyValuePair<string,int> b in beliefStates)
        {
            if (!states.ContainsKey(b.Key))
            {
                states.Add(b.Key, b.Value);
                //Debug.Log("Added belief " + b.Key);
            }
        }
        action = a;
    }
}

public class GPlanner
{
    public Queue<GAction> Plan(List<GAction> actions, Dictionary<string, int> goal, WorldStates beliefStates)
    {
        //Debug.Log("Planning New Plan");
        List<GAction> usableActions = new List<GAction>();
        foreach(GAction a in actions)
        {
            if (a.isAchievable())
            {
                usableActions.Add(a);
            }
        }

        List<Node> leaves = new List<Node>();
        Node start = new Node(null, 0, GWorld.Instance.GetWorld().GetStates(), beliefStates.GetStates(), null);

        bool success = BuildGraph(start, leaves, usableActions, goal);

        if (!success)
        {
            //Debug.Log("NO PLAN");
            return null;
        }

        Node cheapest = null;
        foreach(Node leaf in leaves)
        {
            if(cheapest == null)
            {
                cheapest = leaf;
            }
            else if(leaf.cost < cheapest.cost)
            {
                cheapest = leaf;
            }
        }

        List<GAction> result = new List<GAction>();
        Node n = cheapest;
        while (n != null)
        {
            if(n.action != null)
            {
                result.Insert(0, n.action);
            }

            n = n.parent;
        }

        Queue<GAction> queue = new Queue<GAction>();
        foreach(GAction a in result)
        {
            queue.Enqueue(a);
        }

        /*Debug.Log("The Plan is: ");
        foreach(GAction a in queue)
        {
            Debug.Log("Q: " + a.actionName);
        }*/

        return queue;
    }

    private bool BuildGraph(Node parent, List<Node> leaves, List<GAction> usableActions, Dictionary<string, int> goal)
    {
        bool foundPath = false;
        foreach(GAction action in usableActions)
        {
            if (action.isAchievableGiven(parent.states))
            {
                Dictionary<string, int> curState = new Dictionary<string, int>(parent.states);
                foreach(KeyValuePair<string, int> eff in action.effects)
                {
                    if (!curState.ContainsKey(eff.Key))
                    {
                        curState.Add(eff.Key, eff.Value);
                    }
                }

                Node node = new Node(parent, parent.cost + action.cost, curState, action);

                if(GoalAchieved(goal, curState))
                {
                    leaves.Add(node);
                    foundPath = true;
                }
                else
                {
                    List<GAction> subset = ActionSubset(usableActions, action);
                    bool found = BuildGraph(node, leaves, subset, goal);
                    if (found)
                    {
                        foundPath = true;
                    }
                }
            }
        }

        return foundPath;

    }

    private bool GoalAchieved(Dictionary<string, int> goal, Dictionary<string, int> state)
    {
        foreach(KeyValuePair<string, int> g in goal)
        {
            if (!state.ContainsKey(g.Key))
            {
                return false;
            }
        }
        return true;
    }

    private List<GAction> ActionSubset(List<GAction> actions, GAction removeMe)
    {
        List<GAction> subset = new List<GAction>();
        foreach(GAction a in actions)
        {
            if (!a.Equals(removeMe))
            {
                subset.Add(a);
            }
        }

        return subset;
    }

}
