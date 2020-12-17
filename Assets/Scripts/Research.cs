using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Research : GAction
{
    public float exhaustion = 10;
    public override bool PrePerform()
    {
        //print("Starting Research");
        target = GWorld.Instance.GetQueue("offices").RemoveResource();
        if (target == null)
        {
            print(name + ": No office found: RemoveOffice() returned null");
            return false;
        }
        inventory.AddItem(target);
        return true;
    }

    public override bool PostPerform()
    {
        //print("Research Finished");
        GWorld.Instance.GetQueue("offices").AddResource(target);
        inventory.RemoveItem(target);

        GetComponent<Doctor>().Exhaustion(exhaustion);
        return true;
    }
}
