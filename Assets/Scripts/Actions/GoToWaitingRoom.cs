using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToWaitingRoom : GAction
{
    public override bool PrePerform()
    {
        offset = new Vector3(Random.Range(-5, 6), 0, Random.Range(-9, 4));
        if (beliefs.HasState("isCured"))
        {
            targetTag = "Home";
        }
        return true;
    }

    public override bool PostPerform()
    {
        if (beliefs.HasState("isCured")) 
        {
            return true;
        }
        GWorld.Instance.GetQueue("patients").AddResource(gameObject);
        target = null;
        beliefs.ModifyState("treatable", 1);
        beliefs.ModifyState("waiting", 1);
        transform.Find("StateLight").gameObject.SetActive(true);
        transform.Find("StateLight").gameObject.GetComponent<Light>().color = Color.red;
        if (!FindObjectOfType<Canvas>().transform.Find("WorldStates").GetComponent<UpdateWorld>().patients.Contains(gameObject))
        {
            FindObjectOfType<Canvas>().transform.Find("WorldStates").GetComponent<UpdateWorld>().patients.Add(gameObject);
        }
        return true;
    }
}
