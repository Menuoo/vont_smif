using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class AttachPoint : MonoBehaviour
{
    // ::: TODO :::
    // need a direction parameter - where to point the other attached object
    // nvm, just invert the attach point (((     ROTATE AROUND Y AXIS     ))) if its looking the opposite way of the front


    // ::: NOTES :::
    // *  after attaching, deformation is impossbile, as all of it is redirected towards the root (whatever has a rigidbody). can be fixed by propagating the method call to children
    // *  cant grab the attached children


    [SerializeField] int order = 10; // lower order executes earlier -- it is what the other item will get attached to

    Collider coll;
    XRGrabInteractable xrGrab;
    public AttachRoot root;


    bool isAttached = false;

    private void Start()
    {
        coll = GetComponent<Collider>();


        Rigidbody rb = GetComponentInParent<Rigidbody>();
        if (rb == null)
            transform.parent.gameObject.AddComponent<Rigidbody>();


        xrGrab = transform.parent.gameObject.GetComponentInParent<XRGrabInteractable>();
        if (xrGrab == null)
        {
            XRGrabInteractable xrGrab = transform.parent.gameObject.AddComponent<XRGrabInteractable>();
            xrGrab.movementType = XRBaseInteractable.MovementType.VelocityTracking;
        }
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

        if (root != null)
        {
            Debug.Log("a------------");
            foreach (Collider c in other.root.accruedColliders)
            {
                root.accruedColliders.Add(c);
                xrGrab.colliders.Add(c);

                Debug.Log(c.gameObject.name);
            }

            /*root.accruedColliders.Add(other.transform.parent.GetComponent<Collider>());
            xrGrab.colliders.Add(other.transform.parent.GetComponent<Collider>());*/

            if (xrGrab != null)
            {
                xrGrab.interactionManager.UnregisterInteractable(xrGrab.GetComponent<IXRInteractable>());
                xrGrab.interactionManager.RegisterInteractable(xrGrab.GetComponent<IXRInteractable>());
            }
        }

        other.root = root;

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
            xrGrab = transform.parent.gameObject.GetComponentInParent<XRGrabInteractable>();
            if (xrGrab != null)
                Destroy(xrGrab);

            XRBaseGrabTransformer grabT = transform.parent.gameObject.GetComponentInParent<XRBaseGrabTransformer>();
            if (grabT != null)
                Destroy(grabT);

            Rigidbody rb = transform.parent.gameObject.GetComponentInParent<Rigidbody>();
            if (rb != null)
                Destroy(rb);
        }
    }
}
