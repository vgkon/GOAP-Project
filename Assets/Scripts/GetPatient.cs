using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetPatient : GAction
{
    GameObject resource;
    public override bool PrePerform()
    {

        target = GWorld.Instance.GetQueue("patients").RemoveResource();
        if (target == null )
        {
            print("Nurse(GetPatient): Couldn't get patient: Null patient");
            return false;
        }
        if (!target.GetComponent<Patient>().Occupied(true))
        {
            GWorld.Instance.GetQueue("patients").AddResource(target);
            print("Patient at toilet");
            return false;
        }


        resource = GWorld.Instance.GetQueue("cubicles").RemoveResource();
        if (resource == null)
        {
            GWorld.Instance.GetQueue("patients").AddResource(target);
            target = null;
            print("Nurse(GetPatient): Couldn't get cubicle: Null cubicle");
            print("Nurse(GetPatient): Couldn't get cubicle: Let go of patient");
            return false;
        }
        inventory.AddItem(resource);
        target.GetComponent<Patient>().beliefs.ModifyState("waiting", -1);
        target.GetComponent<Patient>().transform.Find("StateLight").gameObject.GetComponent<Light>().color = Color.blue;
        FindObjectOfType<Canvas>().transform.Find("WorldStates").GetComponent<UpdateWorld>().patients.Remove(target);
        return true;
    }

    public override bool PostPerform()
    {
        if(target != null)
        {
            target.GetComponent<GAgent>().inventory.AddItem(resource);
        }
        target.GetComponent<GAgent>().Occupied(false);
        return true;
    }


 /* */
}
