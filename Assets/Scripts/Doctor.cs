using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doctor : GAgent
{
    public float energy = 100f;
    //public float bladder = 100f;
    SubGoal s1, s2, s3;

    new void Start()
    {
        base.Start();
        s1 = new SubGoal("research", 1, false);
        goals.Add(s1, 3);
        s2 = new SubGoal("rested", 1, false);
        goals.Add(s2, 1);
        s3 = new SubGoal("relief", 1, false);
        goals.Add(s3, 4);

        Invoke("Exhaustion", Random.Range(0, 2));
        //Invoke("BladderStrain", Random.Range(0, 1));
    }

    private void Exhaustion()
    {
        energy -= 1f;
        Invoke("Exhaustion", Random.Range(2, 10));
        if (energy <= 0)
        {
            beliefs.ModifyState("exhausted", 1);
            goals[s2] = 100;
        }
    }
    public void Exhaustion(float v)
    {
        energy -= v;
        if (energy <= 0)
        {
            beliefs.ModifyState("exhausted", 1);
            goals[s2] = 100;
        }
    }
    public void Rest()
    {
        energy = 100;
        beliefs.ModifyState("exhausted", -1);
        goals[s2] = 1;
    }
    /*
    private void BladderStrain()
    {
        bladder -= 1f;
        Invoke("BladderStrain", Random.Range(2, 10));
        if (bladder <= 0)
        {
            beliefs.ModifyState("needsToGo", 1);
            goals[s3] = 100;
        }
    }


    public void Relief()
    {
        bladder = 100;
        beliefs.ModifyState("needsToGo", -1);
        goals[s3] = 2;
    }
    */

}
