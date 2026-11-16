using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaGracity : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        Rigidbody rigid = other.GetComponent<Rigidbody>();
        rigid.useGravity = false;
        rigid.velocity = rigid.velocity * 0.05f;
        
    }
    void OnTriggerExit(Collider other)
    {
        Rigidbody rigid = other.GetComponent<Rigidbody>();
        rigid.useGravity = true;
    }
}
