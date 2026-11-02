using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

public class Smelter : MonoBehaviour
{
    public Material[] mat;
    public Renderer prefab;
    MetalType whichMetal;

    [Range(0, 100)]
    public int metalCount = 0;
  
    void OnTriggerEnter(Collider other)
    {
        Heatable heatable = other.GetComponent<Heatable>();
        if (heatable != null)
            whichMetal = heatable.metal;
        print("Object is \'"+whichMetal+"\'");

        this.metalCount += 50;

        print("Metal: "+this.metalCount);
        if(this.metalCount == 100)
        {
            Renderer newObject = Instantiate(prefab, transform.position+transform.forward,quaternion.identity);
            newObject.material = mat[(int)whichMetal];
            this.metalCount = 0;
        }
                
        Destroy(other.gameObject); // make sure player hands donut touch it lol :)


    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
