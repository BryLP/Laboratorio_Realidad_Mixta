using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObject : MonoBehaviour
{

    public GameObject objectToSpawn;
    public Transform posicion;

    GameObject objectNemuatica;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawn(){
        objectNemuatica = Instantiate(objectToSpawn, posicion.position, objectToSpawn.transform.rotation);
    }
}
