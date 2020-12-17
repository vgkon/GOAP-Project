using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GStateMonitor : MonoBehaviour
{
    public string state;
    public float stateStrength;
    public float stateDecayRate;
    public WorldStates beliefs;
    public GameObject resourcePrefab;
    public string queueName;
    public GAction action;

    bool stateFound = false;
    float initialStrength;

    // Start is called before the first frame update
    void Awake()
    {
        beliefs = this.GetComponent<GAgent>().beliefs;
        initialStrength = stateStrength;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (action.running)
        {
            stateFound = false;
            stateStrength = initialStrength;
        }

        if(!stateFound && beliefs.HasState(state))
        {
            stateFound = true;
        }

        if (stateFound)
        {
            stateStrength -= stateDecayRate * Time.deltaTime;
            if(stateStrength <= 0)
            {
                Vector3 location = new Vector3(transform.position.x, resourcePrefab.transform.position.y, transform.position.z);
                GameObject p = Instantiate(resourcePrefab, location, resourcePrefab.transform.rotation);
                stateFound = false;
                stateStrength = initialStrength;
                beliefs.RemoveState(state);
                GWorld.Instance.GetQueue(queueName).AddResource(p);
            }
        }
    }
}
