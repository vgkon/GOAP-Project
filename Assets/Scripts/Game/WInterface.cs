using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WInterface : MonoBehaviour
{
    GameObject focusObj;
    public GameObject newResourcePrefab;
    Vector3 goalPos;
    public NavMeshSurface surface;
    public GameObject hospital;
    Vector3 clickOffset = Vector3.zero;
    bool offsetCalc = false;
    bool deleteResource = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void MouseOnHoverTrash()
    {
        deleteResource = true;
    }

    public void MouseOffHoverTrash()
    {
        deleteResource = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out hit))
            {
                return;
            }
            
            if(hit.transform.gameObject.tag == "Toilet")
            {
                focusObj = hit.transform.gameObject;
            }
            else 
            {
                goalPos = hit.point;
                focusObj = Instantiate(newResourcePrefab, goalPos, newResourcePrefab.transform.rotation);
            }

            focusObj.GetComponent<Collider>().enabled = false;
            
        }
        else if (focusObj && Input.GetMouseButtonUp(0))
        {
            if (deleteResource)
            {
                GWorld.Instance.GetQueue("toilets").RemoveResource(focusObj);
                Destroy(focusObj);
            }
            else
            {
                focusObj.transform.parent = hospital.transform;
                GWorld.Instance.GetQueue("toilets").AddResource(focusObj);

                focusObj.GetComponent<Collider>().enabled = true;
            }

            surface.BuildNavMesh();

            
            offsetCalc = false;
            clickOffset = Vector3.zero;
            focusObj = null;
        }
        else if (focusObj && Input.GetMouseButton(0))
        {
            RaycastHit hitMove;
            Ray rayMove = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(rayMove, out hitMove))
            {
                return;
            }

            if (!offsetCalc)
            {
                clickOffset = hitMove.point - focusObj.transform.position;
                offsetCalc = true;
            }
            goalPos = hitMove.point - clickOffset;
            focusObj.transform.position = goalPos;
        }

        if (focusObj && Input.GetKeyDown(KeyCode.Less) || Input.GetKeyDown(KeyCode.Comma))
        {
            focusObj.transform.Rotate(0, 90, 0);
        }
        if (focusObj && Input.GetKeyDown(KeyCode.Greater) || Input.GetKeyDown(KeyCode.Period))
        {
            focusObj.transform.Rotate(0, -90, 0);
        }
        
    }
}
