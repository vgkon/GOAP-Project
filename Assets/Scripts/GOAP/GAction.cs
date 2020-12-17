using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class GAction : MonoBehaviour
{
    protected Vector3 offset;

    public string actionName = "Action";
    public float cost = 1.0f;
    public GameObject target;
    public string targetTag;
    public float duration = 0;
    public WorldState[] preConditions;
    public WorldState[] afterEffects;
    public NavMeshAgent agent;

    public Dictionary<string, int> preconditions;   //could be WorldStates class
    public Dictionary<string, int> effects;         //could be WorldStates class

    public WorldStates beliefs;

    public GInventory inventory;

    public bool running = false;

    /*public GAction()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
    }
    */

    private void Awake()
    {
        preconditions = new Dictionary<string, int>();
        effects = new Dictionary<string, int>();
        agent = gameObject.GetComponent<NavMeshAgent>();

        if(preConditions != null)
        {
            foreach(WorldState w in preConditions)
            {
                preconditions.Add(w.key, w.value);
            }
        }
        if (afterEffects != null)
        {
            foreach (WorldState w in afterEffects)
            {
                effects.Add(w.key, w.value);
            }
        }

        inventory = GetComponent<GAgent>().inventory;
        beliefs = GetComponent<GAgent>().beliefs;
    }

    public bool isAchievable()
    {
        return true;
    }

    public bool isAchievableGiven(Dictionary<string, int> conditions)
    {
        /*print("Action " + actionName);

        foreach (KeyValuePair<string, int> c in conditions)
        {
            print("Condition : Key : " + c.Key + " Value :" + c.Value);
        }*/

        foreach (KeyValuePair<string, int> p in preconditions)
        {
            //print("Precondition : Key : " + p.Key + " Value :" + p.Value);
            if (!conditions.ContainsKey(p.Key))
            {
                //print("Precondition : " + p.Key + " not met");
                return false;
            }
        }
        //Debug.Break();
        return true;
    }

    public Vector3 GetOffset()
    {
        return offset;
    }

    public abstract bool PrePerform();
    public abstract bool PostPerform();
}
