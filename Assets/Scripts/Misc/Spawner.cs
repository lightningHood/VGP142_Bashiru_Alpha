using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner: MonoBehaviour
{


    public GameObject[] spawnArray;


    // Start is called before the first frame update
    void Start()
    {
        Instantiate(spawnArray[Random.Range(0, spawnArray.Length)], transform.position, transform.rotation);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
