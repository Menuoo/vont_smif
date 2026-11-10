using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastArea : MonoBehaviour
{
    public Transform pos;
    public Cast currentCast;

    private void OnTriggerStay(Collider other)
    {
        //Debug.Log("who up in this bih");
        Cast cast = other.GetComponent<Cast>();
        if (cast != null && currentCast == null && cast.canAttach)
        {
            /*Debug.Log("who up in this bih");
            currentCast = cast;
            currentCast.transform.SetParent(pos, false);
            //currentCast.coll.isTrigger = true;
            //currentCast.GetComponent<Rigidbody>().useGravity = false;
            currentCast.GetComponent<Rigidbody>().isKinematic = true;
            currentCast.GetComponent<Rigidbody>().velocity = Vector3.zero;
            currentCast.GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
            currentCast.transform.localPosition = Vector3.zero;
            currentCast.transform.rotation = Quaternion.identity;
            currentCast.dada = this;*/
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Cast cast = other.GetComponent<Cast>();
        if (cast != null && currentCast == null)
        {
            Debug.Log("cast friend :)");
            currentCast = cast;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Cast cast = other.GetComponent<Cast>();
        if (cast == currentCast)
        {
            Debug.Log("cast enemy :(");
            currentCast = null;
        }
    }
}
