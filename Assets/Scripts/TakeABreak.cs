using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeABreak : GAction
{
    public override bool PrePerform()
    {
        target = GameObject.FindGameObjectWithTag("Lounge");
        return true;
    }

    public override bool PostPerform()
    {
        if(GetComponent<Nurse>()) GetComponent<Nurse>().Rest();
        if (GetComponent<Doctor>()) GetComponent<Doctor>().Rest();
        return true;
    }
}
