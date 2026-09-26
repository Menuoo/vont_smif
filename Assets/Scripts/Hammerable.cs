using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hammerable : MonoBehaviour
{
    public float toughness = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HammerThis(float power)
    {
        Debug.Log("kyaaaaaaaaa :: " + power);
    }
}
