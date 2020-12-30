using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject patientPrefab;
    public int maxPatients = int.MaxValue;
    int numPatients = 0;
    public float speed = 1f;

    void Start()
    {
        Invoke("SpawnPatient", Random.Range(1, 10) / speed);
    }

    void SpawnPatient()
    {
        numPatients++;
        Instantiate(patientPrefab, transform.position, Quaternion.identity);
        Invoke("SpawnPatient", Random.Range(1, 10) / speed);
    }
}