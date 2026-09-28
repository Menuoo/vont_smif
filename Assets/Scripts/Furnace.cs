using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : MonoBehaviour
{

    void OnTriggerEnter(Collider other)
    {
        print("its in");
    }

    void OnTriggerExit(Collider other)
    {
        print("its out");
    }
    void OnTriggerStay(Collider other)
    {
        Heatable obj = other.gameObject.GetComponent<Heatable>();

        if(obj !=null)
        {
            obj.changeTemperature(Time.fixedDeltaTime/5);
        }
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
