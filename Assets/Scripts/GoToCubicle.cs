using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoToCubicle : GAction
{
    public float exhaustion = 10;
    public override bool PrePerform()
    {
        target = inventory.FindItemWithTag("Cubicle");
        GWorld.Instance.GetWorld().ModifyState("Treating Patients", 1);
        if (target == null)
        {
            print("Nurse(TreatPatient): Couldn't find Cubicle: null Cubicle");
            return false;
        }
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("Treating Patients", -1);
        GWorld.Instance.GetQueue("cubicles").AddResource(target);
        inventory.RemoveItem(target);
        GetComponent<Nurse>().Exhaustion(exhaustion);
        //target = null;
        return true;
    }
}
