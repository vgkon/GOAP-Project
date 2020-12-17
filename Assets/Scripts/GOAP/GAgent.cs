using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SubGoal
{
    public Dictionary<string, int> sGoals;
    public bool remove;

    public SubGoal(string s, int i, bool r)
    {
        sGoals = new Dictionary<string, int>();
        sGoals.Add(s, i);
        remove = r;
    }
}

public class GAgent : MonoBehaviour
{
    SubGoal relief;


    public List<GAction> actions = new List<GAction>();
    public Dictionary<SubGoal, int> goals = new Dictionary<SubGoal, int>();
    public GInventory inventory = new GInventory();
    public WorldStates beliefs = new WorldStates();
    
    public float runtime = 0;

    GPlanner planner;
    Queue<GAction> actionQueue;
    public GAction curAction;
    SubGoal curGoal;

    Vector3 destination = Vector3.zero;

    public float bladderDet = 1f;

    bool occupied = false;


    public void Start()
    {

        relief = new SubGoal("relief", 1, false);
        goals.Add(relief, 1);

        GAction[] acts = GetComponents<GAction>();
        foreach(GAction a in acts)
        {
            actions.Add(a);
        }
    }

    bool invoked = false;
    void CompleteAction()
    {
        if (curAction.PostPerform())
        {
            curAction.running = false;
        }
        runtime = 0;
        invoked = false;
    }

    void LateUpdate()
    {
        runtime += Time.deltaTime;
        if (occupied) return;
        BladderStrain();
        if(curAction != null && curAction.running)
        {
            float dist = Vector3.Distance(transform.position, destination);
            //print("curAction.agent.hasPath = " + curAction.agent.hasPath);
            if(curAction.target.transform.position + curAction.GetOffset() != destination)
            {
                //print(name + " updating position");
                destination = curAction.target.transform.position + curAction.GetOffset();
            }
            if (dist <= 2f)
            {
                if (!invoked)
                {
                    Invoke("CompleteAction", curAction.duration);
                    invoked = true;
                }
            }
            return;
            /*if(curAction.agent.hasPath && curAction.agent.remainingDistance < 1f)
            {
                if (!invoked)
                {
                    Invoke("CompleteAction", curAction.duration);
                    invoked = true;
                }
            }
            return;*/
        }

        if(planner == null || actionQueue == null)
        {
            //print(name + ": New Planner");
            planner = new GPlanner();

            var sortedGoals = from entry in goals orderby entry.Value descending select entry;

            foreach (KeyValuePair<SubGoal, int> sg in sortedGoals)
            {
                //print(name + ": Planning begins");
                actionQueue = planner.Plan(actions, sg.Key.sGoals, beliefs);
                if(actionQueue != null)
                {
                    curGoal = sg.Key;
                    break;
                }
            }
        }

        if(actionQueue != null && actionQueue.Count == 0)
        {
            if (curGoal != null && curGoal.remove)
            {
                foreach(KeyValuePair<string, int> g in curGoal.sGoals)
                {
                    //print(name + ": Removing goal: " + g.Key);
                }
                goals.Remove(curGoal);
                curGoal = null;
            }
            planner = null;
        }

        if(actionQueue != null && actionQueue.Count > 0)
        {
            curAction = actionQueue.Dequeue();
            if (curAction.PrePerform())
            {
                if (curAction.target == null && curAction.targetTag != "")
                {
                    curAction.target = GameObject.FindWithTag(curAction.targetTag);
                }

                if(curAction.target != null)
                {
                    curAction.running = true;

                    destination = curAction.target.transform.position + curAction.GetOffset();
                    Transform dest = curAction.target.transform.Find("Destination");
                    if(dest != null)
                    {
                        destination = dest.position + curAction.GetOffset();
                    }
                    curAction.agent.SetDestination(destination);
                }

            }
            else
            {
                actionQueue = null;
                //print(name + ": Null actionQueue");
            }
        }
    }


    public float bladder = 100f;

    private void BladderStrain()
    {
        bladder -= Random.Range(0,6)* bladderDet * Time.deltaTime;
        if (bladder <= 0 && !beliefs.HasState("needsToGo"))
        {
            beliefs.ModifyState("needsToGo", 1);
            goals[relief] = 100;
        }
    }
    public void Relief()
    {
        bladder = 100;
        beliefs.ModifyState("needsToGo", -1);
        goals[relief] = 1;
    }

    public bool Occupied(bool o)
    {
        if (!beliefs.HasState("treatable")) return false;
        occupied = o;
        return true;
    }

    public void ResetActionQueue()
    {
        actionQueue = null;
    }
}
