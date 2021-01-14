using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WInterface : MonoBehaviour
{
    GameObject focusObj;
    ResourceData foData;
    GameObject newResourcePrefab;
    public GameObject[] allResources;
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

    public void ActivateToilet()
    {
        newResourcePrefab = allResources[0];
    }

    public void ActivateCubicle()
    {
        newResourcePrefab = allResources[1];
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

            Resource r = hit.transform.gameObject.GetComponent<Resource>();

            if(r != null)
            {
                focusObj = hit.transform.gameObject;
                foData = r.info;
            }
            else if(newResourcePrefab != null)
            {
                goalPos = hit.point;
                focusObj = Instantiate(newResourcePrefab, goalPos, newResourcePrefab.transform.rotation);
                foData = focusObj.GetComponent<Resource>().info;
            }

            if(focusObj) focusObj.GetComponent<Collider>().enabled = false;
            
        }
        else if (focusObj && Input.GetMouseButtonUp(0))
        {
            if (deleteResource)
            {
                GWorld.Instance.GetQueue(foData.resourceQueue).RemoveResource(focusObj);
                Destroy(focusObj);
            }
            else
            {
                focusObj.transform.parent = hospital.transform;
                GWorld.Instance.GetQueue(foData.resourceQueue).AddResource(focusObj);

                focusObj.GetComponent<Collider>().enabled = true;
            }

            surface.BuildNavMesh();

            
            offsetCalc = false;
            clickOffset = Vector3.zero;
            focusObj = null;
        }
        else if (focusObj && Input.GetMouseButton(0))
        {
            int layerMask = 1 << 8;
            RaycastHit hitMove;
            Ray rayMove = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(rayMove, out hitMove, Mathf.Infinity, layerMask))
            {
                print("whaaat");
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