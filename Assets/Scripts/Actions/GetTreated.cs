using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetTreated : GAction
{
    public override bool PrePerform()
    {
        target = inventory.FindItemWithTag("Cubicle");
        if(target == null)
        {
            return false;
        }
        beliefs.ModifyState("treatable", -1);
        return true;
    }

    public override bool PostPerform()
    {
        GWorld.Instance.GetWorld().ModifyState("Treated", 1);
        beliefs.ModifyState("isCured", 1);
        inventory.RemoveItem(target);
        transform.Find("StateLight").gameObject.GetComponent<Light>().color = Color.white;
        return true;
    }
}
