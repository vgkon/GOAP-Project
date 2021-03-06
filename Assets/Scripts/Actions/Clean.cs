﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clean : GAction
{
    public override bool PrePerform()
    {
        print("PrePerform Clean");
        target = GWorld.Instance.GetQueue("puddles").RemoveResource();
        if (target == null)
        {
            print(name + ": No toilet found: RemoveToilet() returned null");
            return false;
        }
        inventory.AddItem(target);
        return true;
    }

    public override bool PostPerform()
    {
        inventory.RemoveItem(target);
        Destroy(target);
        return true;
    }
}
