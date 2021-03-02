using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class TargetSpawn : MonoBehaviour
{

   
    // Start is called before the first frame update
    void Start()
    {
        // init random spheres
        foreach (int value in Enumerable.Range(1, 100))
        {            
            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            primitive.transform.parent = this.gameObject.transform;
            primitive.name = "Sphere" + value;
            primitive.transform.localPosition = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(0.0f, 2.0f), Random.Range(-3.0f, 3.0f));
            primitive.transform.localScale = new Vector3(Random.Range(0.05f, 0.2f), Random.Range(0.05f, 0.2f), Random.Range(0.05f, 0.2f));
            primitive.GetComponent<MeshRenderer>().material.color = Color.blue;
        }

        // init random cubes
        foreach (int value in Enumerable.Range(1, 100))
        {
            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cube);
            primitive.transform.parent = this.gameObject.transform;
            primitive.name = "Cube" + value;
            primitive.transform.localPosition = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(0.0f, 2.0f), Random.Range(-3.0f, 3.0f));
            primitive.transform.localScale = new Vector3(Random.Range(0.05f, 0.2f), Random.Range(0.05f, 0.2f), Random.Range(0.05f, 0.2f));
            primitive.GetComponent<MeshRenderer>().material.color = Color.red;
        }

        // init random cylinders
        foreach (int value in Enumerable.Range(1, 100))
        {
            GameObject primitive = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            primitive.transform.parent = this.gameObject.transform;
            primitive.name = "Cylinder" + value;
            primitive.transform.localPosition = new Vector3(Random.Range(-3.0f, 3.0f), Random.Range(0.0f, 2.0f), Random.Range(-3.0f, 3.0f));
            primitive.transform.localScale = new Vector3(Random.Range(0.05f, 0.2f), Random.Range(0.05f, 0.2f), Random.Range(0.05f, 0.2f));
            primitive.GetComponent<MeshRenderer>().material.color = Color.green;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}
