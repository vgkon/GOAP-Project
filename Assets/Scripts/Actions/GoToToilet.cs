using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToToilet : GAction
{
    public override bool PrePerform()
    {
        print("PrePerform GoToToilet");
        if (GetComponent<Patient>())
        {
            beliefs.ModifyState("atToilet", 1);
            beliefs.ModifyState("treatable", -1);
        }
        target = GWorld.Instance.GetQueue("toilets").RemoveResource();
        if (target == null)
        {
            print(name + ": No toilet found: RemoveToilet() returned null");
            return false;
        }
        //print(target.transform.position);
        //print(target.name);
        inventory.AddItem(target);
        FindObjectOfType<Canvas>().transform.Find("WorldStates").GetComponent<UpdateWorld>().toilets.Remove(target);
        FindObjectOfType<Canvas>().transform.Find("WorldStates").GetComponent<UpdateWorld>().toilets.Add(gameObject);
        return true;
    }

    public override bool PostPerform()
    {
        if (GetComponent<Patient>())
        {
            beliefs.ModifyState("atToilet", -1);
        }
        GWorld.Instance.GetQueue("toilets").AddResource(target);
        FindObjectOfType<Canvas>().transform.Find("WorldStates").GetComponent<UpdateWorld>().toilets.Remove(gameObject);
        FindObjectOfType<Canvas>().transform.Find("WorldStates").GetComponent<UpdateWorld>().toilets.Add(target);
        inventory.RemoveItem(target);
        GetComponent<GAgent>().Relief();
        return true;
    }
}
