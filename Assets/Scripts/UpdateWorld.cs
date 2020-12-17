using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateWorld : MonoBehaviour
{
    public List<GameObject> patients;
    public List<GameObject> toilets;
    public Text states;

    private void Start()
    {
        var ts = GameObject.FindGameObjectsWithTag("Toilet");
        foreach(GameObject t in ts)
        {
            toilets.Add(t);
        }
    }
    void LateUpdate()
    {
        Dictionary<string, int> worldStates = GWorld.Instance.GetWorld().GetStates();
        states.text = "";
        foreach(KeyValuePair<string, int> s in worldStates)
        {
            states.text += s.Key + ", " + s.Value + "\n";
        }
    }
}
