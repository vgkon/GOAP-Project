using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drive : MonoBehaviour
{
    Camera cam;
    public float speed = 10.0f;
    public float rotationSpeed = 20.0f;
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        cam.gameObject.transform.LookAt(transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        float translation = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        float translation2 = Input.GetAxis("Horizontal") * speed * Time.deltaTime;

        transform.Translate(0, 0, translation);
        transform.Translate(translation2, 0, 0);

        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(0, rotationSpeed * Time.deltaTime, 0);
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(0, -rotationSpeed * Time.deltaTime, 0);
        }

        float camHeight = cam.gameObject.transform.position.y;
        if (Input.GetKey(KeyCode.Z) && camHeight > 10)
        {
            cam.gameObject.transform.Translate(0, 0, speed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.X) && camHeight < 40)
        {
            cam.gameObject.transform.Translate(0, 0, -speed * Time.deltaTime);
        }

        float angle = Vector3.Angle(cam.gameObject.transform.forward, Vector3.up);
        
        if (Input.GetKey(KeyCode.R) && angle < 175)
        {
            cam.gameObject.transform.Translate(Vector3.up * rotationSpeed * Time.deltaTime);
            cam.gameObject.transform.LookAt(transform.position);
        }
        if (Input.GetKey(KeyCode.F) && angle > 95)
        {
            cam.gameObject.transform.Translate(Vector3.down * rotationSpeed * Time.deltaTime);
            cam.gameObject.transform.LookAt(transform.position);
        }

    }
}
