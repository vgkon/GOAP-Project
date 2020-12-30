using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class ResourceQueue
{
    public Queue<GameObject> que = new Queue<GameObject>();
    public string tag;
    public string modState;

    public ResourceQueue(string t, string ms, WorldStates w)
    {
        tag = t;
        modState = ms;
        if(tag != "")
        {
            GameObject[] resources = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject r in resources)
                if(!que.Contains(r))que.Enqueue(r);
        }

        if(modState != "")
        {
            w.ModifyState(modState, que.Count);
        }
    }

    public void AddResource(GameObject r)
    {
        if (!que.Contains(r))
        {
            que.Enqueue(r);
            GWorld.Instance.GetWorld().ModifyState(modState, 1);
        }
    }

    public GameObject RemoveResource()
    {
        if (que.Count == 0) return null;
        GWorld.Instance.GetWorld().ModifyState(modState, -1);
        return que.Dequeue();
    }
    public void RemoveResource(GameObject r)
    {
        que = new Queue<GameObject>(que.Where(p => p != r));
        GWorld.Instance.GetWorld().ModifyState(modState, -1);
    }
}


public sealed class GWorld
{
    private static readonly GWorld instance = new GWorld();
    private static WorldStates world;
    private static ResourceQueue puddles;
    private static ResourceQueue patients;
    private static ResourceQueue cubicles;
    private static ResourceQueue offices;
    private static ResourceQueue toilets;
    private static Dictionary<string, ResourceQueue> resources = new Dictionary<string, ResourceQueue>();

    static GWorld()
    {
        //Time.timeScale = 10;
        world = new WorldStates();
        patients = new ResourceQueue("", "Waiting", world);
        resources.Add("patients", patients);
        cubicles = new ResourceQueue("Cubicle", "Free Cubicles", world);
        resources.Add("cubicles", cubicles);
        offices = new ResourceQueue("Office", "Free Offices", world);
        resources.Add("offices", offices);
        toilets = new ResourceQueue("Toilet", "Free Toilets", world);
        resources.Add("toilets", toilets);
        toilets = new ResourceQueue("Puddle", "Puddles", world);
        resources.Add("puddles", toilets);

    }

    public ResourceQueue GetQueue(string type)
    {
        return resources[type];
    }

    private GWorld()
    {
    }

    public static GWorld Instance
    {
        get { return instance; }
    }

    public WorldStates GetWorld()
    {
        return world;
    }

 
}
