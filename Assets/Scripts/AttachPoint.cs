using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachPoint : MonoBehaviour
{
    // TODO :::
    // need a direction parameter - where to point the other attached object
    // nvm, just invert the attach point (((     ROTATE AROUND Y AXIS     ))) if its looking the opposite way of the front



    [SerializeField] int order = 10; // lower order executes earlier -- it is what the other item will get attached to
    
    
    Collider coll;
    bool isAttached = false;

    private void Start()
    {
        coll = GetComponent<Collider>();

        Rigidbody rb = GetComponentInParent<Rigidbody>();
        if (rb == null)
            transform.parent.gameObject.AddComponent<Rigidbody>();
    }


    private void OnTriggerEnter(Collider other)
    {
        AttachPoint otherPoint = other.GetComponent<AttachPoint>();

        if (otherPoint != null)
        { 
            int otherOrder = otherPoint.order;
            if (order < otherOrder)
            {
                AttachOtherToThis(otherPoint);
            }
            else if (order == otherOrder)
            {
                AttachOtherToThis(otherPoint);
            }

        }
    }

    void AttachOtherToThis(AttachPoint other)
    {
        if (isAttached)
            return;

        this.GotAttached(false);
        other.GotAttached(true);

        other.transform.parent.parent = this.transform;
        other.transform.parent.localPosition = -other.transform.localPosition;
        other.transform.parent.localRotation = Quaternion.identity;
    }

    public void GotAttached(bool turnOffRigid)
    {
        isAttached = true;
        coll.enabled = false;

        if (turnOffRigid)
        {
            Rigidbody rb = transform.parent.gameObject.GetComponentInParent<Rigidbody>();
            if (rb != null)
                Destroy(rb);
        }
    }
}
